using System;
using System.Deployment.Application;
using System.Threading.Tasks;

namespace Logic.Services
{
    public class UpdateService
    {
        private LogService _log;
        private RestartService _restart;
        private ApplicationDeployment _ad;

        public UpdateService(LogService log, RestartService restart)
        {
            _log = log;
            _restart = restart;
            _ad = ApplicationDeployment.CurrentDeployment;
        }

        public async Task<bool> HasUpdate()
        {
            if (!ApplicationDeployment.IsNetworkDeployed) return false;

            UpdateCheckInfo info = null;
            try
            {
                info = _ad.CheckForDetailedUpdate();
            }
            catch (DeploymentDownloadException dde)
            {
                _log.Log("Updates", "The new version of the application cannot be downloaded at this time. \n\nPlease check your network connection, or try again later. Error: " + dde.Message);
                return false;
            }
            catch (InvalidDeploymentException ide)
            {
                _log.Log("Updates", "Cannot check for a new version of the application. The ClickOnce deployment is corrupt. Please redeploy the application and try again. Error: " + ide.Message);
                return false;
            }
            catch (InvalidOperationException ioe)
            {
                _log.Log("Updates", "This application cannot be updated. It is likely not a ClickOnce application. Error: " + ioe.Message);
                return false;
            }

            return info.UpdateAvailable;
        }

        public async Task Update()
        {
            if (!ApplicationDeployment.IsNetworkDeployed) return;

            try
            {
                _ad.Update();
                _restart.Restart();
            }
            catch (DeploymentDownloadException dde)
            {
                _log.Log("Updates", "Cannot install the latest version of the application. \n\nPlease check your network connection, or try again later. Error: " + dde);
            }
        }
    }
}
