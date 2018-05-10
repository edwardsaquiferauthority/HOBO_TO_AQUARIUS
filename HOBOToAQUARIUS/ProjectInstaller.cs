using System.ComponentModel;

namespace HOBOToAQUARIUS
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        #region Public Constructors

        public ProjectInstaller()
        {
            InitializeComponent();
        }

        #endregion Public Constructors
    }
}