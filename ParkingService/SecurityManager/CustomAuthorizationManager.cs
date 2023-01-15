using AuditingManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace SecurityManager
{
    public class CustomAuthorizationManager : ServiceAuthorizationManager
    {
        protected override bool CheckAccessCore(OperationContext operationContext)
        {
            CustomPrincipal principal = operationContext.ServiceSecurityContext.AuthorizationContext.Properties["Principal"] as CustomPrincipal;

            bool condition = principal.IsInRole("Pay") || principal.IsInRole("ParkingWorker") || principal.IsInRole("ManageZone");

            if (!condition)
            {
                try
                {
                    Audit.AuthorizationFailed(Formatter.ParseName(principal.Identity.Name), "service methods", "Does not have any of required permission.");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            else
            {
                try
                {
                    Audit.AuthorizationSuccess(Formatter.ParseName(principal.Identity.Name), "service methods");
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }
               
            }
            return condition;
        }
    }
}
