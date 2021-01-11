using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using EAPI.CLI.Lib.DataClasses;
using YP.SassyMQ.Lib.RabbitMQ;

namespace CLIClassLibrary.RoleHandlers
{
    public partial class UserCLIHandler
    {
        public override void AddHelp(StringBuilder sb, string helpTerm)
        {
            sb.AppendLine($"Help for User.");
            
            helpTerm = helpTerm.ToLower();
            var found = helpTerm == "general";
            
            if (helpTerm == "general")
            {
                sb.AppendLine();
                
                sb.AppendLine($"Portfolio: AddPortfolio");
                sb.AppendLine($"Portfolio: GetPortfolios");
                sb.AppendLine($"Portfolio: UpdatePortfolio");
                sb.AppendLine($"void: DeletePortfolio");                                            
            }
            
            sb.AppendLine($"{Environment.NewLine}Available Actions Matching: {helpTerm}");
            
            if ("addportfolio".Contains(helpTerm, StringComparison.OrdinalIgnoreCase))
            {
                sb.AppendLine($" - AddPortfolio");
                if ("addportfolio".Equals(helpTerm, StringComparison.OrdinalIgnoreCase)) 
                {
                    this.PrintAddPortfolioHelp(sb);
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
            if ("updateportfolio".Contains(helpTerm, StringComparison.OrdinalIgnoreCase))
            {
                sb.AppendLine($" - UpdatePortfolio");
                if ("updateportfolio".Equals(helpTerm, StringComparison.OrdinalIgnoreCase)) 
                {
                    this.PrintUpdatePortfolioHelp(sb);
                }
                found = true;
            }
            if ("deleteportfolio".Contains(helpTerm, StringComparison.OrdinalIgnoreCase))
            {
                sb.AppendLine($" - DeletePortfolio");
                if ("deleteportfolio".Equals(helpTerm, StringComparison.OrdinalIgnoreCase)) 
                {
                    this.PrintDeletePortfolioHelp(sb);
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
                case "addportfolio":
                    this.SMQActor.AddPortfolio(payload, (reply, bdea) =>
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

                case "updateportfolio":
                    this.SMQActor.UpdatePortfolio(payload, (reply, bdea) =>
                    {
                        result = SerializePayload(reply);
                    }).Wait(30000);
                    break;                   

                case "deleteportfolio":
                    this.SMQActor.DeletePortfolio(payload, (reply, bdea) =>
                    {
                        result = SerializePayload(reply);
                    }).Wait(30000);
                    break;                   

                default:
                    throw new Exception($"Invalid request: {invokeRequest}");
            }

            return result;
        }
        
        
        public void PrintAddPortfolioHelp(StringBuilder sb)
        {
            
                
                sb.AppendLine();
                sb.AppendLine($"* * * * * * * * * * * * * * * * * * * * * * * * * * *");
                sb.AppendLine($"* *  OBJECT DEF: Portfolio     *");
                sb.AppendLine($"* * * * * * * * * * * * * * * * * * * * * * * * * * *");
                sb.AppendLine();
                
                    sb.AppendLine($"CRUD      - PortfolioId");
                    sb.AppendLine($"CRUD      - Name");
                    sb.AppendLine($"CRUD      - Notes");
                    sb.AppendLine($"CRUD      - Attachments");
                    sb.AppendLine($"CRUD      - Status");
                    sb.AppendLine($"CRUD      - Owner");
                    sb.AppendLine($"CRUD      - Description");
                    sb.AppendLine($"CRUD      - AppUserEmailAddress");
                    sb.AppendLine($"CRUD      - IsDeleted");
                    sb.AppendLine($"CRUD      - IsPublished");
                    sb.AppendLine($"CRUD      - PublishDate");
                    sb.AppendLine($"CRUD      - AppUserName");
                    sb.AppendLine($"CRUD      - Curator");
                    sb.AppendLine($"CRUD      - CuratorName");
                    sb.AppendLine($"CRUD      - Title");
                    sb.AppendLine($"CRUD      - CreatedTimee");
                    sb.AppendLine($"CRUD      - JonsValue");
                    sb.AppendLine($"CRUD      - Tagline");
                
            
        }
        
        public void PrintGetPortfoliosHelp(StringBuilder sb)
        {
            
                
                sb.AppendLine();
                sb.AppendLine($"* * * * * * * * * * * * * * * * * * * * * * * * * * *");
                sb.AppendLine($"* *  OBJECT DEF: Portfolio     *");
                sb.AppendLine($"* * * * * * * * * * * * * * * * * * * * * * * * * * *");
                sb.AppendLine();
                
                    sb.AppendLine($"CRUD      - PortfolioId");
                    sb.AppendLine($"CRUD      - Name");
                    sb.AppendLine($"CRUD      - Notes");
                    sb.AppendLine($"CRUD      - Attachments");
                    sb.AppendLine($"CRUD      - Status");
                    sb.AppendLine($"CRUD      - Owner");
                    sb.AppendLine($"CRUD      - Description");
                    sb.AppendLine($"CRUD      - AppUserEmailAddress");
                    sb.AppendLine($"CRUD      - IsDeleted");
                    sb.AppendLine($"CRUD      - IsPublished");
                    sb.AppendLine($"CRUD      - PublishDate");
                    sb.AppendLine($"CRUD      - AppUserName");
                    sb.AppendLine($"CRUD      - Curator");
                    sb.AppendLine($"CRUD      - CuratorName");
                    sb.AppendLine($"CRUD      - Title");
                    sb.AppendLine($"CRUD      - CreatedTimee");
                    sb.AppendLine($"CRUD      - JonsValue");
                    sb.AppendLine($"CRUD      - Tagline");
                
            
        }
        
        public void PrintUpdatePortfolioHelp(StringBuilder sb)
        {
            
                
                sb.AppendLine();
                sb.AppendLine($"* * * * * * * * * * * * * * * * * * * * * * * * * * *");
                sb.AppendLine($"* *  OBJECT DEF: Portfolio     *");
                sb.AppendLine($"* * * * * * * * * * * * * * * * * * * * * * * * * * *");
                sb.AppendLine();
                
                    sb.AppendLine($"CRUD      - PortfolioId");
                    sb.AppendLine($"CRUD      - Name");
                    sb.AppendLine($"CRUD      - Notes");
                    sb.AppendLine($"CRUD      - Attachments");
                    sb.AppendLine($"CRUD      - Status");
                    sb.AppendLine($"CRUD      - Owner");
                    sb.AppendLine($"CRUD      - Description");
                    sb.AppendLine($"CRUD      - AppUserEmailAddress");
                    sb.AppendLine($"CRUD      - IsDeleted");
                    sb.AppendLine($"CRUD      - IsPublished");
                    sb.AppendLine($"CRUD      - PublishDate");
                    sb.AppendLine($"CRUD      - AppUserName");
                    sb.AppendLine($"CRUD      - Curator");
                    sb.AppendLine($"CRUD      - CuratorName");
                    sb.AppendLine($"CRUD      - Title");
                    sb.AppendLine($"CRUD      - CreatedTimee");
                    sb.AppendLine($"CRUD      - JonsValue");
                    sb.AppendLine($"CRUD      - Tagline");
                
            
        }
        
        public void PrintDeletePortfolioHelp(StringBuilder sb)
        {
            
        }
        

    }
}
