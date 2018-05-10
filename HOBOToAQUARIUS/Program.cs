using System.ServiceProcess;

namespace HOBOToAQUARIUS
{
    internal static class Program
    {
        #region Private Methods

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        private static void Main()
        {
            var servicesToRun = new ServiceBase[]
            {
                new HobotoAquarius()
            };
            ServiceBase.Run(servicesToRun);
        }

        #endregion Private Methods
    }
}