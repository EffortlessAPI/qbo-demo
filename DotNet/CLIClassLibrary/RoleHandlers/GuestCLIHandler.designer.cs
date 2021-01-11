using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using EAPI.CLI.Lib.DataClasses;
using YP.SassyMQ.Lib.RabbitMQ;

namespace CLIClassLibrary.RoleHandlers
{
    public partial class GuestCLIHandler
    {
        public override void AddHelp(StringBuilder sb, string helpTerm)
        {
            sb.AppendLine($"Help for Guest.");
            
            helpTerm = helpTerm.ToLower();
            var found = helpTerm == "general";
            
            if (helpTerm == "general")
            {
                sb.AppendLine();
                
                sb.AppendLine($"void: RequestToken");
                sb.AppendLine($"void: ValidateTemporaryAccessToken");
                sb.AppendLine($"void: WhoAmI");
                sb.AppendLine($"void: WhoAreYou");
                sb.AppendLine($"void: StoreTempFile");
                sb.AppendLine($"Portfolio: GetPortfolios");                                            
            }
            
            sb.AppendLine($"{Environment.NewLine}Available Actions Matching: {helpTerm}");
            
            if ("requesttoken".Contains(helpTerm, StringComparison.OrdinalIgnoreCase))
            {
                sb.AppendLine($" - RequestToken");
                if ("requesttoken".Equals(helpTerm, StringComparison.OrdinalIgnoreCase)) 
                {
                    this.PrintRequestTokenHelp(sb);
                }
                found = true;
            }
            if ("validatetemporaryaccesstoken".Contains(helpTerm, StringComparison.OrdinalIgnoreCase))
            {
                sb.AppendLine($" - ValidateTemporaryAccessToken");
                if ("validatetemporaryaccesstoken".Equals(helpTerm, StringComparison.OrdinalIgnoreCase)) 
                {
                    this.PrintValidateTemporaryAccessTokenHelp(sb);
                }
                found = true;
            }
            if ("whoami".Contains(helpTerm, StringComparison.OrdinalIgnoreCase))
            {
                sb.AppendLine($" - WhoAmI");
                if ("whoami".Equals(helpTerm, StringComparison.OrdinalIgnoreCase)) 
                {
                    this.PrintWhoAmIHelp(sb);
                }
                found = true;
            }
            if ("whoareyou".Contains(helpTerm, StringComparison.OrdinalIgnoreCase))
            {
                sb.AppendLine($" - WhoAreYou");
                if ("whoareyou".Equals(helpTerm, StringComparison.OrdinalIgnoreCase)) 
                {
                    this.PrintWhoAreYouHelp(sb);
                }
                found = true;
            }
            if ("storetempfile".Contains(helpTerm, StringComparison.OrdinalIgnoreCase))
            {
                sb.AppendLine($" - StoreTempFile");
                if ("storetempfile".Equals(helpTerm, StringComparison.OrdinalIgnoreCase)) 
                {
                    this.PrintStoreTempFileHelp(sb);
                }
                found = true;
            }
            if ("getportfolios".Contains(helpTerm, StringComparison.OrdinalIgnoreCase))
            {
                sb.AppendLine($" - GetPortfolios");
                if ("getportfolios".Equals(helpTerm, StringComparison.OrdinalIgnoreCase)) 
                {
                    this.PrintGetPortfoliosHelp(sb);
                }
                found = true;
            }
                       
            if (!found)
            {
                sb.AppendLine();
                sb.AppendLine($"{Environment.NewLine}UNABLE TO FIND COMMAND: {helpTerm} not found.");
            }
        }

        private string HandlerFactory(string invokeRequest, string payloadString, string where, int maxPages, string view)
        {
            var result = "";
            var payload = JsonConvert.DeserializeObject<StandardPayload>(payloadString);
            payload.SetActor(this.SMQActor);
            payload.AccessToken = this.SMQActor.AccessToken;
            payload.AirtableWhere = where;
            payload.MaxPages = maxPages;
            payload.View = view;

            switch (invokeRequest.ToLower())
            {
                case "requesttoken":
                    this.SMQActor.RequestToken(payload, (reply, bdea) =>
                    {
                        result = SerializePayload(reply);
                    }).Wait(30000);
                    break;                   

                case "validatetemporaryaccesstoken":
                    this.SMQActor.ValidateTemporaryAccessToken(payload, (reply, bdea) =>
                    {
                        result = SerializePayload(reply);
                    }).Wait(30000);
                    break;                   

                case "whoami":
                    this.SMQActor.WhoAmI(payload, (reply, bdea) =>
                    {
                        result = SerializePayload(reply);
                    }).Wait(30000);
                    break;                   

                case "whoareyou":
                    this.SMQActor.WhoAreYou(payload, (reply, bdea) =>
                    {
                        result = SerializePayload(reply);
                    }).Wait(30000);
                    break;                   

                case "storetempfile":
                    this.SMQActor.StoreTempFile(payload, (reply, bdea) =>
                    {
                        result = SerializePayload(reply);
                    }).Wait(30000);
                    break;                   

                case "getportfolios":
                    this.SMQActor.GetPortfolios(payload, (reply, bdea) =>
                    {
                        result = SerializePayload(reply);
                    }).Wait(30000);
                    break;                   

                default:
                    throw new Exception($"Invalid request: {invokeRequest}");
            }

            return result;
        }
        
        
        public void PrintRequestTokenHelp(StringBuilder sb)
        {
            
        }
        
        public void PrintValidateTemporaryAccessTokenHelp(StringBuilder sb)
        {
            
        }
        
        public void PrintWhoAmIHelp(StringBuilder sb)
        {
            
        }
        
        public void PrintWhoAreYouHelp(StringBuilder sb)
        {
            
        }
        
        public void PrintStoreTempFileHelp(StringBuilder sb)
        {
            
        }
        
        public void PrintGetPortfoliosHelp(StringBuilder sb)
        {
            
                
                sb.AppendLine();
                sb.AppendLine($"* * * * * * * * * * * * * * * * * * * * * * * * * * *");
                sb.AppendLine($"* *  OBJECT DEF: Portfolio     *");
                sb.AppendLine($"* * * * * * * * * * * * * * * * * * * * * * * * * * *");
                sb.AppendLine();
                
                    sb.AppendLine($"R      - PortfolioId");
                    sb.AppendLine($"R      - Name");
                    sb.AppendLine($"R      - AppUserName");
                
            
        }
        

    }
}
