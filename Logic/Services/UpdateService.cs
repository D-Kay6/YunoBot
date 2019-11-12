using System;
//using System.Deployment.Application;
using System.Security;
using System.Security.Permissions;
using System.Security.Policy;
using System.Threading.Tasks;

namespace Logic.Services
{
    public class UpdateService
    {
        private LogService _log;
        private RestartService _restart;
        //private ApplicationDeployment _ad;

        //public UpdateService(LogService log, RestartService restart)
        //{
        //    _log = log;
        //    _restart = restart;
        //    _ad = ApplicationDeployment.CurrentDeployment;
        //}

        //public async Task<bool> HasUpdate()
        //{
        //    if (!ApplicationDeployment.IsNetworkDeployed) return false;

        //    var appId = new ApplicationIdentity(_ad.UpdatedApplicationFullName);
        //    var unrestrictedPerms = new PermissionSet(PermissionState.Unrestricted);
        //    var appTrust = new ApplicationTrust(appId)
        //    {
        //        DefaultGrantSet = new PolicyStatement(unrestrictedPerms),
        //        IsApplicationTrustedToRun = true,
        //        Persist = true
        //    };
        //    ApplicationSecurityManager.UserApplicationTrusts.Add(appTrust);

        //    UpdateCheckInfo info = null;
        //    try
        //    {
        //        info = _ad.CheckForDetailedUpdate();
        //    }
        //    catch (DeploymentDownloadException dde)
        //    {
        //        _log.Log("Updates", "The new version of the application cannot be downloaded at this time. \n\nPlease check your network connection, or try again later. Error: " + dde.Message);
        //        return false;
        //    }
        //    catch (InvalidDeploymentException ide)
        //    {
        //        _log.Log("Updates", "Cannot check for a new version of the application. The ClickOnce deployment is corrupt. Please redeploy the application and try again. Error: " + ide.Message);
        //        return false;
        //    }
        //    catch (TrustNotGrantedException tnge)
        //    {
        //        _log.Log("Updates", "The application does not have enough permissions to auto-update. Error: " + tnge.Message);
        //        return false;
        //    }
        //    catch (InvalidOperationException ioe)
        //    {
        //        _log.Log("Updates", "This application cannot be updated. It is likely not a ClickOnce application. Error: " + ioe.Message);
        //        return false;
        //    }

        //    if (!info.UpdateAvailable) return false;

        //    _log.Log("Updates", $"A new version of the application ({info.AvailableVersion}) has been found.");
        //    return true;
        //}

        //public async Task Update()
        //{
        //    if (!ApplicationDeployment.IsNetworkDeployed) return;

        //    try
        //    {
        //        _log.Log("Updates", "Trying to update the application to the latest version.");
        //        _restart.HardRestart();
        //    }
        //    catch (DeploymentDownloadException dde)
        //    {
        //        _log.Log("Updates", "Cannot install the latest version of the application. \n\nPlease check your network connection, or try again later. Error: " + dde);
        //    }
        //}
    }
}
