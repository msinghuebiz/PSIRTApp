using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PSIRTApp.Models;

namespace PSIRTApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration config;
        private string clientID;
        private string clientserect;
        private string eolclientID;
        private string eolclientserect;
        private string orionURL;
        private string orionUserName;
        private string orionPassword;
        public HomeController(ILogger<HomeController> logger, IConfiguration config)
        {
            _logger = logger;
            clientID = config.GetSection("CiscoWebExUser").GetValue<string>("key");
            clientserect = config.GetSection("CiscoWebExUser").GetValue<string>("clientsecret");
            eolclientID = config.GetSection("CiscoWebExUser").GetValue<string>("EOLkey");
            eolclientserect = config.GetSection("CiscoWebExUser").GetValue<string>("EOLclientsecret");
            orionURL = config.GetSection("OrionUserCredentials").GetValue<string>("url");
            orionUserName = config.GetSection("OrionUserCredentials").GetValue<string>("username");
            orionPassword = config.GetSection("OrionUserCredentials").GetValue<string>("password");

           // var command = new ExecuteCommands();
           // var resultAuth = command.GetAuthToken("shnhwdngb9ktxgp343wv4k35", "qaJqbjCPPaJwceFbudNxkPMR").Result;

           // var result = command.GetWebResponse<EOXResultByProduct>("https://api.cisco.com/commerce/ORDER/v2/sync/getSubscriptionDetails", "", resultAuth);

            var ss = string.Empty;
        }

        public async Task<ActionResult> _PartialViewModel(string xmlLinkValue)
        {
            return PartialView("_PartialVulnDetails", xmlLinkValue);
        }

        public async Task<ActionResult> CiscoPSIRT()
        {
            {
                var model = new EOLCombinationModel();
                model.OrionList = new List<Dictionary<string, object>>();
                model.EOLList = new EOXResultByProduct();
                model.OrionDistinctList = new List<string>();
                model.VulCountList = new Dictionary<string, int>();
                return View(model);

            }
        }

        [HttpPost]
        public async Task<IActionResult> CiscoPSIRT(EOLCombinationModel filledModel)
        {
            if (!(string.IsNullOrEmpty(filledModel.SearchText)))
            {
                if ((string.IsNullOrEmpty(filledModel.SearchType)))
                {
                    // With the type . get the value as per that
                    var obj = new CombineModelClass(clientID, clientserect, eolclientID, eolclientserect, orionURL, orionUserName, orionPassword);
                    var resultTuple = await obj.GetCiscoPSIRT(filledModel.SearchType, filledModel.SearchText);
                    filledModel.EOLList = resultTuple.Item1;
                    filledModel.OrionList = resultTuple.Item2;
                    filledModel.VulCountList = resultTuple.Item3;
                    filledModel.OrionDistinctList = new List<string>();


                }
            }

            return View(filledModel);
        }

        public async Task<ActionResult> CiscoEOLSW()
        {
            {
                var model = new EOLCombinationModel();
                model.OrionList = new List<Dictionary<string, object>>();
                model.EOLList = new EOXResultByProduct();
                model.OrionDistinctList = new List<string>();
                model.VulCountList = new Dictionary<string, int>();
                return View(model);

            }
        }

        [HttpPost]
        public async Task<IActionResult> CiscoEOLSW(EOLCombinationModel filledModel)
        {
            if (!(string.IsNullOrEmpty(filledModel.SearchText)))
            {
                if ((string.IsNullOrEmpty(filledModel.SearchType)))
                {
                    // With the type . get the value as per that
                    var obj = new CombineModelClass(clientID, clientserect, eolclientID, eolclientserect, orionURL, orionUserName, orionPassword);
                    var resultTuple = await obj.GetCiscoEOLHW(filledModel.SearchType, filledModel.SearchText);
                    filledModel.EOLList = resultTuple.Item1;
                    filledModel.OrionList = resultTuple.Item2;
                    filledModel.VulCountList = resultTuple.Item3;
                    filledModel.OrionDistinctList = new List<string>();


                }
            }

            return View(filledModel);
        }

        public async Task<ActionResult> CiscoEOLHW()
        {
            {
                var model = new EOLCombinationModel();
                model.OrionList = new List<Dictionary<string, object>>();
                model.EOLList = new EOXResultByProduct();
                model.OrionDistinctList = new List<string>();
                model.VulCountList = new Dictionary<string, int>();
                return View(model);

            }
        }

        [HttpPost]
        public async Task<IActionResult> CiscoEOLHW(EOLCombinationModel filledModel)
        {
            if (!(string.IsNullOrEmpty(filledModel.SearchText)))
            {
                if ((string.IsNullOrEmpty(filledModel.SearchType)))
                {
                    // With the type . get the value as per that
                    var obj = new CombineModelClass(clientID, clientserect, eolclientID, eolclientserect, orionURL, orionUserName, orionPassword);
                    var resultTuple = await obj.GetCiscoEOLHW(filledModel.SearchType, filledModel.SearchText);
                    filledModel.EOLList = resultTuple.Item1;
                    filledModel.OrionList = resultTuple.Item2;
                    filledModel.VulCountList = resultTuple.Item3;
                    filledModel.OrionDistinctList = new List<string>();


                }
            }

            return View(filledModel);
        }

        public async Task<ActionResult> ConsolidatedReport()
        {
            {
                var model = new EOLCombinationModel();
                model.OrionList = new List<Dictionary<string, object>>();
                model.EOLList = new EOXResultByProduct();
                model.OrionDistinctList = new List<string>();
                model.VulCountList = new Dictionary<string, int>();
                return View(model);

            }
        }

        [HttpPost]
        public async Task<IActionResult> ConsolidatedReport(EOLCombinationModel filledModel)
        {
            if (!(string.IsNullOrEmpty(filledModel.SearchText)))
            {
                if ((string.IsNullOrEmpty(filledModel.SearchType)))
                {
                    // With the type . get the value as per that
                    var obj = new CombineModelClass(clientID, clientserect, eolclientID, eolclientserect,   orionURL, orionUserName, orionPassword);
                    var resultTuple = await obj.GetConsolidatedObject(filledModel.SearchType, filledModel.SearchText);
                    filledModel.EOLList = resultTuple.Item1;
                    filledModel.OrionList = resultTuple.Item2;
                    filledModel.VulCountList = resultTuple.Item3;
                    filledModel.OrionDistinctList = new List<string>();
                   

                }
            }

            return View(filledModel);
        }

        public async Task<IActionResult> EOLOrion()
        {
            var model = new EOLCombinationModel();
            model.OrionList = new List<Dictionary<string, object>>();
            model.EOLList = new EOXResultByProduct();
            model.OrionDistinctList = new List<string>();
            return View(model);
       
        }

        [HttpPost]
        public async Task<IActionResult> EOLOrion(EOLCombinationModel filledModel)
        {
            if (!(string.IsNullOrEmpty(filledModel.SearchText)))
            {
                if ((string.IsNullOrEmpty(filledModel.SearchType)))
                {
                    // With the type . get the value as per that
                    var obj = new CombineModelClass(eolclientID, eolclientserect, eolclientID, eolclientserect, orionURL, orionUserName, orionPassword);
                    var resultTuple = await obj.GetEOLOrionObject(filledModel.SearchType, filledModel.SearchText);
                    filledModel.EOLList = resultTuple.Item1;
                    filledModel.OrionList = resultTuple.Item2;
                    filledModel.OrionDistinctList = new List<string>();
                    foreach (var item in filledModel.OrionList)
                    {
                        var desc = item["Description"];
                        var ios = item["IOSVersion"];
                        var uniquedesc = string.Format("{0}~{1}", desc, ios);
                        if (!(filledModel.OrionDistinctList.Contains(uniquedesc)))
                        {
                            filledModel.OrionDistinctList.Add(uniquedesc);
                        }




                    }

                }
            }

            return View(filledModel);
        }

        public async Task<ActionResult> CombinedCustomerReport()
        {
            var model = new EOLCombinationModel();
            model.SearchText = string.Empty;
            model.SearchType = string.Empty;
            model.EOLList = new EOXResultByProduct();
            model.OrionList = new List<Dictionary<string, object>>();
            model.VuldistList = new Dictionary<string, List<VulnStructure>>();
            model.OrionDistinctList = new List<string>();
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> CombinedCustomerReport(EOLCombinationModel filledModel)
        {
            if (!(string.IsNullOrEmpty(filledModel.SearchText)))
            {
                if ((string.IsNullOrEmpty(filledModel.SearchType)))
                {
                    // With the type . get the value as per that
                    var obj = new CombineModelClass(clientID, clientserect, eolclientID, eolclientserect, orionURL, orionUserName, orionPassword);
                    var resultTuple = await obj.GetOrionVuln_Optimised(filledModel.SearchType, filledModel.SearchText);
                    filledModel.EOLList = resultTuple.Item1;
                    filledModel.OrionList = resultTuple.Item2;
                    filledModel.VuldistList = resultTuple.Item3;
                    filledModel.OrionDistinctList = new List<string>();

                    foreach (var item in filledModel.OrionList)
                    {
                        var desc = item["Description"];
                        var ios = item["IOSVersion"];
                        var uniquedesc = string.Format("{0}~{1}", desc, ios);
                        if (!(filledModel.OrionDistinctList.Contains(uniquedesc)))
                        {
                            filledModel.OrionDistinctList.Add(uniquedesc);
                        }




                    }
                }
            }

            return View(filledModel);
        }

        public async Task<IActionResult> OrionToVuln()
        {
            var model = new EOLCombinationModel();
            model.SearchText = string.Empty;
            model.SearchType = string.Empty;
            model.EOLList = new EOXResultByProduct();
            model.OrionList = new List<Dictionary<string, object>>();
            model.VuldistList = new Dictionary<string, List<VulnStructure>>();
            model.OrionDistinctList = new List<string>();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> OrionToVuln(EOLCombinationModel filledModel)
        {
            if (!(string.IsNullOrEmpty(filledModel.SearchText)))
            {
                if ((string.IsNullOrEmpty(filledModel.SearchType)))
                {
                    // With the type . get the value as per that
                    var obj = new CombineModelClass(clientID, clientserect, eolclientID, eolclientserect, orionURL, orionUserName, orionPassword);
                    var resultTuple = await obj.GetOrionVuln(filledModel.SearchType, filledModel.SearchText);
                    filledModel.EOLList = resultTuple.Item1;
                    filledModel.OrionList = resultTuple.Item2;
                    filledModel.VuldistList = resultTuple.Item3;
                    filledModel.OrionDistinctList = new List<string>();

                    foreach (var item in filledModel.OrionList)
                    {
                        var desc = item["Description"];
                        var ios = item["IOSVersion"];
                        var uniquedesc = string.Format("{0}~{1}", desc, ios);
                        if (!(filledModel.OrionDistinctList.Contains(uniquedesc)))
                        {
                            filledModel.OrionDistinctList.Add(uniquedesc);
                        }




                    }
                }
            }

            return View(filledModel);
        }


        public async Task<IActionResult> VulnToOrion()
        {
            var model = new CombinationModel();
            model.OrionList = new List<Dictionary<string, object>>();
            model.VulnList = new Vuln();
            model.OrionDistinctList = new List<string>();
            return View(model );
             
        }
        [HttpPost]
        public async Task<IActionResult> VulnToOrion(CombinationModel filledModel )
        {
            if ( ! (string.IsNullOrEmpty(filledModel.SearchText )))
            {
                if (!(string.IsNullOrEmpty(filledModel.SearchType )) )
                {
                    // With the type . get the value as per that
                    var obj = new CombineModelClass(clientID , clientserect, eolclientID, eolclientserect, orionURL,orionUserName,orionPassword );
                    var resultTuple = await obj.GetCombinationObject(filledModel.SearchType, filledModel.SearchText);
                    filledModel.VulnList = resultTuple.Item1;
                    filledModel.OrionList = resultTuple.Item2;
                    filledModel.OrionDistinctList = new List<string>();
                    
                    foreach (var item in filledModel.OrionList)
                    {
                        var desc = item["Description"];
                        var ios = item["IOSVersion"];
                        var uniquedesc = string.Format("{0}~{1}", desc, ios);
                        if (!(filledModel.OrionDistinctList.Contains(uniquedesc)))
                        {
                            filledModel.OrionDistinctList.Add(uniquedesc);
                        }

                        


                    }
                }
            }

            return View(filledModel);
        }


        public async Task<IActionResult > Orion()
        {

            string fields = "NodeID, ObjectSubType,IPAddress, IPAddressType, DynamicIP, Caption," +
                           "NodeDescription, n.Description, DNS, SysName, Vendor, SysObjectID, Location, " +
                           "Contact, VendorIcon, Icon, Status, StatusLED, StatusDescription, CustomStatus, " +
                           "IOSImage, IOSVersion, GroupStatus, StatusIcon, LastBoot, SystemUpTime, ResponseTime, " +
                           "PercentLoss, AvgResponseTime, MinResponseTime, MaxResponseTime, CPUCount, CPULoad, MemoryUsed, " +
                           "LoadAverage1, LoadAverage5, LoadAverage15, MemoryAvailable, PercentMemoryUsed, PercentMemoryAvailable, " +
                           "LastSync, LastSystemUpTimePollUtc, MachineType, IsServer, Severity, UiSeverity, ChildStatus, Allow64BitCounters, " +
                           "AgentPort, TotalMemory, CMTS, CustomPollerLastStatisticsPoll, CustomPollerLastStatisticsPollSuccess, SNMPVersion, " +
                           "PollInterval, EngineID, RediscoveryInterval, NextPoll, NextRediscovery, StatCollection, External, Community, RWCommunity, " +
                           "IP, IP_Address, IPAddressGUID, NodeName, BlockUntil, BufferNoMemThisHour, BufferNoMemToday, BufferSmMissThisHour, BufferSmMissToday, " +
                           "BufferMdMissThisHour, BufferMdMissToday, BufferBgMissThisHour, BufferBgMissToday, BufferLgMissThisHour, BufferLgMissToday, BufferHgMissThisHour, " +
                           "BufferHgMissToday, OrionIdPrefix, OrionIdColumn, SkippedPollingCycles, MinutesSinceLastSync, EntityType, n.DisplayName, Category, " +
                           "IsOrionServer,  UnManaged, UnManageFrom, UnManageUntil, DetailsUrl, Image, " +
                           "StatusIconHint,   n.InstanceType, n.Uri, n.InstanceSiteId, " +
                           "InServiceDate as C_InServiceDate, Client as C_Client, Client_Site as c_Client_Site, " +
                           "Site_Address as c_Site_Address, Site_Address_City as C_Site_Address_City,Site_Address_State as c_Site_Address_State, " +
                           "Site_Address_Country as c_Site_Address_Country, AT_CI_ID_N as c_AT_CI_ID_N, AT_CI_Product_N as c_AT_CI_Product_N, " +
                           "AT_CI_Contract as c_AT_CI_Contract,AT_CI_DeviceName as c_AT_CI_DeviceName,AT_CI_InstallDate as c_AT_CI_InstallDate, " +
                           "AT_CI_Ref_Name as c_AT_CI_Ref_Name, AT_CI_Ref_Number as c_AT_CI_Ref_Number, AT_CI_Contract_Service_Bundle as c_AT_CI_Contract_Service_Bundle, " +
                           "AT_CI_AccountName as c_AT_CI_AccountName, Category_N as c_Category_N, Client_SiteCode as c_Client_SiteCode, Client_Region as c_Client_Region, " +
                           "Site_Severity as c_Site_Severity, Site_Category as c_Site_Category, Site_Hours_Production_Support as c_Site_Hours_Production_Support, InServiceCode as c_InServiceCode, " +
                           "AT_CI_Contract_Service as c_AT_CI_Contract_Service, AT_CustomerID as c_AT_CustomerID, HW_Model as c_HW_Model, Business_Impact_Node as c_Business_Impact_Node, MS_Support_Type_N as c_MS_Support_Type_N, " +
                           "OpRisk as c_OpRisk, OpRiskCategory as c_OpRiskCategory, Contact_Site_Primary as c_Contact_Site_Primary, Contact_Site_Secondary as c_Contact_Site_Secondary,  " +
                           "Site_Address_ZipCode as c_Site_Address_ZipCode, Comments as c_Comments,   HW_SerialNumber as c_HW_SerialNumber, Jefferies_Switch as c_Jefferies_Switch, " +
                           "Report_Incidents_To_Primary as c_Report_Incidents_To_Primary, Report_Incidents_To_Secondary as c_Report_Incidents_To_Secondary, Sev1_Incident_Instructions as c_Sev1_Incident_Instructions, " +
                           "Sev2_Incident_Instructions as c_Sev2_Incident_Instructions, Sev3_Incident_Instructions as c_Sev3_Incident_Instructions,  NOC_Instructions_N as c_NOC_Instructions_N, " +
                           "Category_Report as c_Category_Report, RCM as c_RCM, AT_Prod_CIType_N as c_AT_Prod_CIType_N, Rpt_HW as c_Rpt_HW, Rpt_SW as c_Rpt_SW, " +
                           "Rpt_Liz_Notes as c_Rpt_Liz_Notes, Site_Address_2 as c_Site_Address_2, Rpt_BugID_s_ as c_Rpt_BugID_s_, GrafanaEnv as c_GrafanaEnv, Report_Escalations_To as c_Report_Escalations_To, " +
                           "Site_Region as c_Site_Region, Rpt_BugIDs_Critical as c_Rpt_BugIDs_Critical, Rpt_BugIDs_High as c_Rpt_BugIDs_High, Rpt_BugIDs_Medium as c_Rpt_BugIDs_Medium, Rpt_IOS_Recommended as c_Rpt_IOS_Recommended ";
            //ATRCM as c_ATRCM, HW_EndOfSW_Maint_RFA_Date as c_HW_EndOfSW_Maint_RFA_Date,

            var swisClient = new SwisClient(orionURL, orionUserName, orionPassword);
             string query =@"SELECT Top 100 " + fields + " FROM Orion.Nodes n  inner join Orion.NodesCustomProperties nc on nc.NodeID = n.nodeID where vendor = 'cisco' ";
            JToken queryResult = swisClient.QueryAsync(query).Result;
            JObject joValues = JObject.Parse(queryResult.ToString());

            JArray arrayObject = (JArray)joValues["results"];

            var  result = arrayObject.ToObject<IList<Dictionary<string,object>>>();

            return View(result); 
        }
            public async Task<IActionResult> IndexNXOSACI()
        {
            var model = new IndexModel();
            model.advisoryID = string.Empty;
            model.List = new Vuln();
            model.List.advisories = new List<VulnStructure>();

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> IndexNXOSACI(IndexModel filledModel)
        {
            if (!(string.IsNullOrEmpty(filledModel.advisoryID)))
            {
                filledModel.List = await GetAPIAdvisoryLists("advisories/aci?version={0}", filledModel.advisoryID);

                //var command = new ExecuteCommands();
                //var resultAuth = await command.GetAuthToken(clientID, clientserect);
                //filledModel.List = await command.GetWebResponse<Vuln>("", "advisories/aci?version=" + filledModel.advisoryID, resultAuth);
            }
            return View(filledModel);
        }
        public async Task<IActionResult> IndexNXOS()
        {
            var model = new IndexModel();
            model.advisoryID = string.Empty;
            model.List = new Vuln();
            model.List.advisories = new List<VulnStructure>();

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> IndexNXOS(IndexModel filledModel)
        {
            if (!(string.IsNullOrEmpty(filledModel.advisoryID)))
            {
                filledModel.List = await GetAPIAdvisoryLists("advisories/nxos?version={0}", filledModel.advisoryID);

                //var command = new ExecuteCommands();
                //var resultAuth = await command.GetAuthToken(clientID, clientserect);
                //filledModel.List = await command.GetWebResponse<Vuln>("", "advisories/nxos?version=" + filledModel.advisoryID, resultAuth);
            }
            return View(filledModel);
        }

        public async Task<IActionResult> IndexIOSXE()
        {
            var model = new IndexModel();
            model.advisoryID = string.Empty;
            model.List = new Vuln();
            model.List.advisories = new List<VulnStructure>();

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> IndexIOSXE(IndexModel filledModel)
        {
            if (!(string.IsNullOrEmpty(filledModel.advisoryID)))
            {
                filledModel.List = await GetAPIAdvisoryLists("advisories/iosxe?version={0}", filledModel.advisoryID);

                //var command = new ExecuteCommands();
                //var resultAuth = await command.GetAuthToken(clientID, clientserect);
                //filledModel.List = await command.GetWebResponse<Vuln>("", "advisories/iosxe?version=" + filledModel.advisoryID, resultAuth);
            }
            return View(filledModel);
        }
        public async Task<IActionResult> IndexIOS()
        {
            var model = new IndexModel();
            model.advisoryID = string.Empty;
            model.List = new Vuln();
            model.List.advisories = new List<VulnStructure>();

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> IndexIOS(IndexModel filledModel)
        {
            if (!(string.IsNullOrEmpty(filledModel.advisoryID)))
            {
                
                filledModel.List = await GetAPIAdvisoryLists("advisories/ios?version={0}", filledModel.advisoryID);

            }
            return View(filledModel);
        }

        private async Task<Vuln> GetAPIAdvisoryLists(string urlText, string searchText )
        {
            var listResult = new Vuln();
            listResult.advisories = new List<VulnStructure>();
            var splittedIDs = searchText.Split(",");
            foreach (var item in splittedIDs)
            {
                var updatedURLText = string.Format(urlText, item);
                var command = new ExecuteCommands();
                var resultAuth = await command.GetAuthToken(clientID, clientserect);
                var resultList = await command.GetWebResponse<Vuln>("https://api.cisco.com/security/", updatedURLText, resultAuth);
                if (resultList.advisories != null)
                {
                    foreach (var itemList in resultList.advisories)
                    {
                        listResult.advisories.Add(itemList);
                    }
                }

            }

            return listResult;
        }
        

        public async Task<IActionResult> IndexYear()
    {
        var model = new IndexModel();
        model.advisoryID = string.Empty;
        model.List = new Vuln();
        model.List.advisories = new List<VulnStructure>();

        return View(model);
    }
    [HttpPost]
    public async Task<IActionResult> IndexYear(IndexModel filledModel)
    {
        if (!(string.IsNullOrEmpty(filledModel.advisoryID)))
        {

                filledModel.List = await GetAPIAdvisoryLists("advisories/year/{0}", filledModel.advisoryID);

            //    var command = new ExecuteCommands();
            //var resultAuth = await command.GetAuthToken(clientID, clientserect);
            //filledModel.List = await command.GetWebResponse<Vuln>("", "advisories/year/" + filledModel.advisoryID, resultAuth);
        }
        return View(filledModel);
    }
    public async Task<IActionResult> IndexProduct()
    {
        var model = new IndexModel();
        model.advisoryID = string.Empty;
        model.List = new Vuln();
        model.List.advisories = new List<VulnStructure>();

        return View(model);
    }
    [HttpPost]
    public async Task<IActionResult> IndexProduct(IndexModel filledModel)
    {
        if (!(string.IsNullOrEmpty(filledModel.advisoryID)))
        {
                filledModel.List = await GetAPIAdvisoryLists("advisories/product?product={0}", filledModel.advisoryID);

            //    var command = new ExecuteCommands();
            //var resultAuth = await command.GetAuthToken(clientID, clientserect);
            //filledModel.List = await command.GetWebResponse<Vuln>("", "advisories/product?product=" + filledModel.advisoryID, resultAuth);
        }
        return View(filledModel);
    }
    public async Task<IActionResult> IndexLatest()
    {
        var model = new IndexModel();
        model.advisoryID = string.Empty;
        model.List = new Vuln();
        model.List.advisories = new List<VulnStructure>();

        return View(model);
    }
    [HttpPost]
    public async Task<IActionResult> IndexLatest(IndexModel filledModel)
    {
        if (!(string.IsNullOrEmpty(filledModel.advisoryID)))
        {
                filledModel.List = await GetAPIAdvisoryLists("advisories/latest/{0}", filledModel.advisoryID);

            //    var command = new ExecuteCommands();
            //var resultAuth = await command.GetAuthToken(clientID, clientserect);
            //filledModel.List = await command.GetWebResponse<Vuln>("", "advisories/latest/" + filledModel.advisoryID, resultAuth);
        }
        return View(filledModel);
    }

    public async Task<IActionResult> IndexCVE()
        {
            var model = new IndexModel();
            model.advisoryID = string.Empty;
            model.List = new Vuln();
            model.List.advisories = new List<VulnStructure>();

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> IndexCVE(IndexModel filledModel)
        {
            if (!(string.IsNullOrEmpty(filledModel.advisoryID)))
            {

                filledModel.List = await GetAPIAdvisoryLists("advisories/cve/{0}", filledModel.advisoryID);

                //var command = new ExecuteCommands();
                //var resultAuth = await command.GetAuthToken(clientID, clientserect);
                //filledModel.List = await command.GetWebResponse<Vuln>("", "advisories/cve/" + filledModel.advisoryID, resultAuth);
            }
            return View(filledModel);
        }
        public async Task<IActionResult> Index()
        {
            var model = new IndexModel();
            model.advisoryID = string.Empty;
            model.List = new Vuln();
            model.List.advisories = new List<VulnStructure>();

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Index(IndexModel filledModel )
        {
            if (string.IsNullOrEmpty(filledModel.advisoryID))
            {
                var command = new ExecuteCommands();
                var resultAuth = await command.GetAuthToken(clientID, clientserect);
                filledModel.List = await command.GetWebResponse<Vuln>("https://api.cisco.com/security/", "advisories/all", resultAuth);
            }
            else
            {
                filledModel.List = await GetAPIAdvisoryLists("advisories/advisory/{0}", filledModel.advisoryID);

                //var command = new ExecuteCommands();
                //var resultAuth = await command.GetAuthToken(clientID, clientserect);
                //filledModel.List = await command.GetWebResponse<Vuln>("", "advisories/advisory/"+filledModel.advisoryID, resultAuth);
            }
            return View(filledModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
