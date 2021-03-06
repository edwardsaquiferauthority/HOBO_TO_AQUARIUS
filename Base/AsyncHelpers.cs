﻿/****************************** Module Header ******************************\
Module Name:    AsyncHelpers [static]
Project:        Base
Summary:        A static utility class for executing asynchronous methods
                in a synchronous manner
Author[s]:      Ryan Cooper
Email[s]:       rcooper@edwardsaquifer.org
\***************************************************************************/

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Base
{
    [Obsolete(@"No longer used, there should be both async and sync method for all HOBO and AuqariusLib API calls, if not create your own")]
    public static class AsyncHelpers
    {
        #region Public Methods

        /// <summary>
        /// Execute's an async Task<T/> method which has a void return value synchronously
        /// </summary>
        /// <param name="task">Task<T/> method to execute</param>
        public static void RunSync(Func<Task> task)
        {
            var oldContext = SynchronizationContext.Current;
            var synch = new ExclusiveSynchronizationContext();
            SynchronizationContext.SetSynchronizationContext(synch);
            synch.Post(async _ =>
            {
                try
                {
                    await task();
                }
                catch (Exception e)
                {
                    synch.InnerException = e;
                    throw;
                }
                finally
                {
                    synch.EndMessageLoop();
                }
            }, null);
            synch.BeginMessageLoop();

            SynchronizationContext.SetSynchronizationContext(oldContext);
        }

        /// <summary>
        /// Execute's an async Task<T/> method which has a T return type synchronously
        /// </summary>
        /// <typeparam name="T">Return Type</typeparam>
        /// <param name="task">Task<T/> method to execute</param>
        /// <returns></returns>
        public static T RunSync<T>(Func<Task<T>> task)
        {
            var oldContext = SynchronizationContext.Current;
            var synch = new ExclusiveSynchronizationContext();
            SynchronizationContext.SetSynchronizationContext(synch);
            var ret = default(T);
            synch.Post(async _ =>
            {
                try
                {
                    ret = await task();
                }
                catch (Exception e)
                {
                    synch.InnerException = e;
                    //throw;
                }
                finally
                {
                    synch.EndMessageLoop();
                }
            }, null);
            synch.BeginMessageLoop();
            SynchronizationContext.SetSynchronizationContext(oldContext);
            return ret;
        }

        #endregion Public Methods

        #region Private Classes

        private class ExclusiveSynchronizationContext : SynchronizationContext
        {
            #region Private Fields

            private readonly Queue<Tuple<SendOrPostCallback, object>> items =
                new Queue<Tuple<SendOrPostCallback, object>>();

            private readonly AutoResetEvent workItemsWaiting = new AutoResetEvent(false);
            private bool done;

            #endregion Private Fields

            #region Public Properties

            public Exception InnerException { private get; set; }

            #endregion Public Properties

            #region Public Methods

            public void BeginMessageLoop()
            {
                while (!done)
                {
                    Tuple<SendOrPostCallback, object> task = null;
                    lock (items)
                    {
                        if (items.Count > 0) task = items.Dequeue();
                    }
                    if (task != null)
                    {
                        task.Item1(task.Item2);
                        if (InnerException != null) // the method threw an exeption
                            throw new AggregateException("AsyncHelpers.Run method threw an exception.", InnerException);
                    }
                    else
                    {
                        workItemsWaiting.WaitOne();
                    }
                }
            }

            public override SynchronizationContext CreateCopy()
            {
                return this;
            }

            public void EndMessageLoop()
            {
                Post(_ => done = true, null);
            }

            public override void Post(SendOrPostCallback d, object state)
            {
                lock (items)
                {
                    items.Enqueue(Tuple.Create(d, state));
                }
                workItemsWaiting.Set();
            }

            public override void Send(SendOrPostCallback d, object state)
            {
                throw new NotSupportedException("We cannot send to our same thread");
            }

            #endregion Public Methods
        }

        #endregion Private Classes
    }
}