<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
>
    <xsl:variable name="smq" select="document('Lexicon.smql')" />
    <xsl:output method="xml" indent="yes"/>
    <xsl:variable name="odxml" select="document('DataSchema.odxml')" />

    <xsl:include href="../CommonXsltTemplates.xslt"/>
    <xsl:param name="output-filename" select="'output.txt'" />

    <xsl:template match="@* | node()">
        <xsl:copy>
            <xsl:apply-templates select="@* | node()"/>
        </xsl:copy>
    </xsl:template>

    <xsl:template match="/*">
        <FileSet>
            <FileSetFiles>
                <xsl:for-each select="$smq//SMQActors/SMQActor">
                    <xsl:variable name="role-name" select="Name" />
                    <FileSetFile>
                        <RelativePath>
                            <xsl:value-of select="Name"/>
                            <xsl:text>CLIHandler.cs</xsl:text>
                        </RelativePath>
                        <OverwriteMode>Never</OverwriteMode>
                        <xsl:element name="FileContents" xml:space="preserve">using Newtonsoft.Json;
using EAPI.CLI.Lib.DataClasses;
using YP.SassyMQ.Lib.RabbitMQ;
using System.Text;

namespace CLIClassLibrary.RoleHandlers
{
    public partial class <xsl:value-of select="Name"/>CLIHandler : RoleHandlerBase&lt;SMQ<xsl:value-of select="Name"/>>
    {

        public <xsl:value-of select="Name"/>CLIHandler(string amqps, string accessToken)
            : base(amqps, accessToken)
        {
        }

        public override string Handle(string invoke, string data, string where, int maxPages, string view)
        {
            if (string.IsNullOrEmpty(data)) data = "{}";
            string result = HandlerFactory(invoke, data, where, maxPages, view);
            return result;
        }
    }
}</xsl:element>
                        <FileSetFile>
                            <RelativePath>
                                <xsl:value-of select="Name"/>
                                <xsl:text>CLIHandler.designer.cs</xsl:text>
                            </RelativePath>
                            <xsl:element name="FileContents" xml:space="preserve">using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using EAPI.CLI.Lib.DataClasses;
using YP.SassyMQ.Lib.RabbitMQ;

namespace CLIClassLibrary.RoleHandlers
{
    public partial class <xsl:value-of select="Name"/>CLIHandler
    {
        public override void AddHelp(StringBuilder sb, string helpTerm)
        {
            sb.AppendLine($"Help for <xsl:value-of select="Name"/>.");
            
            helpTerm = helpTerm.ToLower();
            var found = helpTerm == "general";
            
            if (helpTerm == "general")
            {
                sb.AppendLine();
                <xsl:for-each select="$smq/*/SMQMessages/SMQMessage[ActorFrom = $role-name]">
                sb.AppendLine($"<xsl:choose>
                    <xsl:when test="normalize-space(RAWValues/Response) != ''"><xsl:value-of select="RAWValues/Response"/></xsl:when>
                <xsl:otherwise>void</xsl:otherwise>
                </xsl:choose>: <xsl:value-of select="Name"  />");</xsl:for-each>                                            
            }
            
            sb.AppendLine($"{Environment.NewLine}Available Actions Matching: {helpTerm}");
            <xsl:for-each select="$smq//SMQMessages/SMQMessage[ActorFrom = $role-name]">
            if ("<xsl:value-of select="translate(Name, $ucletters, $lcletters)"/>".Contains(helpTerm, StringComparison.OrdinalIgnoreCase))
            {
                sb.AppendLine($" - <xsl:value-of select="Name"/>");
                if ("<xsl:value-of select="translate(Name, $ucletters, $lcletters)"/>".Equals(helpTerm, StringComparison.OrdinalIgnoreCase)) 
                {
                    this.Print<xsl:value-of select="Name"/>Help(sb);
                }
                found = true;
            }</xsl:for-each>
                       
            if (!found)
            {
                sb.AppendLine();
                sb.AppendLine($"{Environment.NewLine}UNABLE TO FIND COMMAND: {helpTerm} not found.");
            }
        }

        private string HandlerFactory(string invokeRequest, string payloadString, string where, int maxPages, string view)
        {
            var result = "";
            var payload = JsonConvert.DeserializeObject&lt;StandardPayload>(payloadString);
            payload.SetActor(this.SMQActor);
            payload.AccessToken = this.SMQActor.AccessToken;
            payload.AirtableWhere = where;
            payload.MaxPages = maxPages;
            payload.View = view;

            switch (invokeRequest.ToLower())
            {<xsl:for-each select="$smq//SMQMessages/SMQMessage[ActorFrom = $role-name]">
                case "<xsl:value-of select="translate(Name, $ucletters, $lcletters)"/>":
                    this.SMQActor.<xsl:value-of select="Name"/>(<xsl:if test="translate(normalize-space(IsDirectMessage), $ucletters, $lcletters) = 'true'">null, </xsl:if>payload, (reply, bdea) =>
                    {
                        result = SerializePayload(reply);
                    }).Wait(30000);
                    break;                   
</xsl:for-each>
                default:
                    throw new Exception($"Invalid request: {invokeRequest}");
            }

            return result;
        }
        
        <xsl:for-each select="$smq/*/SMQMessages/SMQMessage[ActorFrom = $role-name]"><xsl:variable name="msg" select="." />
        public void Print<xsl:value-of select="Name"/>Help(StringBuilder sb)
        {
            <xsl:for-each select="$odxml//ObjectDefs/ObjectDef[Name = $msg/RAWValues/Response]"><xsl:variable name="crud" select="normalize-space(*[name() = concat($role-name,'CRUD')])" />
                <xsl:if test="$crud != 'None' and $crud != ''">
                sb.AppendLine();
                sb.AppendLine($"* * * * * * * * * * * * * * * * * * * * * * * * * * *");
                sb.AppendLine($"* *  OBJECT DEF: <xsl:value-of select="Name" />     *");
                sb.AppendLine($"* * * * * * * * * * * * * * * * * * * * * * * * * * *");
                sb.AppendLine();
                <xsl:for-each select="PropertyDefs/PropertyDef"><xsl:variable name="pd-crud" select="*[name() = concat($role-name,'CRUD')]" /><xsl:if test="string-length(normalize-space($pd-crud)) > 0">
                    sb.AppendLine($"<xsl:value-of select="$pd-crud"/>      - <xsl:value-of select="Name"/>");</xsl:if></xsl:for-each>
                </xsl:if>
            </xsl:for-each>
        }
        </xsl:for-each>

    }
}
</xsl:element>
                        </FileSetFile>
                    </FileSetFile>
                </xsl:for-each>
                <FileSetFile>
                    <RelativePath>
                        <xsl:text>RoleHandlerFactory.cs</xsl:text>
                    </RelativePath>
                    <xsl:element name="FileContents" xml:space="preserve">using SSoTme.Default.Lib.CLIHandler;
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
                <xsl:for-each select="$smq//SMQActors/SMQActor">
                    <xsl:variable name="role-name" select="Name" />
                case "<xsl:value-of select="translate(Name, $ucletters, $lcletters)" />":
                    return new <xsl:value-of select="$role-name" />CLIHandler(amqps, accessToken);
</xsl:for-each>

                default:
                    throw new Exception($"Can't find CLIHandler for {runas} actor.");
            }
        }

        public static void ListRoles(StringBuilder sbHelpBuilder)
        {
           <xsl:for-each select="$smq//SMQActors/SMQActor"><xsl:variable name="role-name" select="Name" />
            sbHelpBuilder.AppendLine(" - <xsl:value-of select="$role-name"/>");</xsl:for-each>
        }
    }
}
</xsl:element>
                </FileSetFile>
            </FileSetFiles>
        </FileSet>
    </xsl:template>
</xsl:stylesheet>
