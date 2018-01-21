using Microsoft.WindowsAzure.ServiceRuntime;
using PortfolioCommon.Access;

namespace WebRole
{
    public class WebRole : RoleEntryPoint
    {
        public override bool OnStart()
        {
            var cloudBlobAccess = new CloudBlobAccess();
            cloudBlobAccess.ClearBlobs();

            return base.OnStart();
        }
    }
}