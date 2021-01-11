using SSoTme.Default.Lib.CLIHandler;
using System;
using System.Collections.Generic;
using System.Text;

namespace CLIClassLibrary.RoleHandlers
{
    public static class RoleHandlerFactory
    {
        public static RoleHandlerBase CreateHandler(string runas, string amqps)
        {
            if (String.IsNullOrEmpty(runas)) runas = EAPICLIHandler.GetMostRecentUser();
            var accessToken = EAPICLIHandler.GetToken(runas);
            switch (runas.ToLower())
            {
                
                    
                case "guest":
                    return new GuestCLIHandler(amqps, accessToken);

                    
                case "crudcoordinator":
                    return new CRUDCoordinatorCLIHandler(amqps, accessToken);

                    
                case "admin":
                    return new AdminCLIHandler(amqps, accessToken);

                    
                case "user":
                    return new UserCLIHandler(amqps, accessToken);


                default:
                    throw new Exception($"Can't find CLIHandler for {runas} actor.");
            }
        }

        public static void ListRoles(StringBuilder sbHelpBuilder)
        {
           
            sbHelpBuilder.AppendLine(" - Guest");
            sbHelpBuilder.AppendLine(" - CRUDCoordinator");
            sbHelpBuilder.AppendLine(" - Admin");
            sbHelpBuilder.AppendLine(" - User");
        }
    }
}
