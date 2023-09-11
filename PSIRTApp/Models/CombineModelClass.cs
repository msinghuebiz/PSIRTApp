using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace PSIRTApp.Models
{
    public class CombineModelClass
    {
        private string _clientID;
        private string _EOLclientID;
        private string _clientserect;
        private string _EOLclientserect;
        private string _orionURL;
        private string _orionUserName;
        private string _orionPassword;
        public CombineModelClass(string clientID , string clientsecret, string eolclientID, string eolclientsecret, string orionURL, string orionUserName, string orionPassword )
        {
            _clientID = clientID;
            _EOLclientID = eolclientID;
            _clientserect = clientsecret;
            _EOLclientserect = eolclientsecret;
            _orionURL = orionURL;
            _orionUserName = orionUserName;
            _orionPassword = orionPassword;
        }

        public async Task<Tuple<EOXResultByProduct, List<Dictionary<string, object>>, Dictionary<string, List<VulnStructure>>>> GetOrionVuln_Optimised(string SearchType, string SearchText)
        {
            var listEOL = new EOXResultByProduct();
            listEOL.EOXRecord = new List<SearchByProductModel>();
            var listOrion = new List<Dictionary<string, object>>();


            string fields = "Client as C_Client,NodeName,IPAddress,HW_Model as c_HW_Model,HW_SerialNumber as c_HW_SerialNumber, Rpt_Liz_Notes as c_Rpt_Liz_Notes, Rpt_BugID_s_ as c_Rpt_BugID_s_, Rpt_BugIDs_Critical as c_Rpt_BugIDs_Critical, Rpt_BugIDs_High as c_Rpt_BugIDs_High, Rpt_BugIDs_Medium as c_Rpt_BugIDs_Medium, Rpt_IOS_Recommended as c_Rpt_IOS_Recommended, IOSVersion, n.Description ";


            //where IOSVersion = '15.2(2)E5''
            var searchQuery = " and Client like '" + SearchText + "%'";
            listOrion = await GetListOrion(fields, searchQuery);
            var HWList = new List<string>();
            var IOSVersionList = new List<string>();
            var HWlimitList = new Dictionary<int, List<string>>();
            var vulrCount = new Dictionary<string, List<VulnStructure>>();
            var count = 1;
            var loopcount = 1;
            foreach (var item in listOrion)
            {
                foreach (var itemInner in item.Keys)
                {
                    if (itemInner.ToLower() == "c_HW_Model".ToLower())
                    {
                        if (!(HWList.Contains(item[itemInner].ToString())))
                        {
                            HWList.Add(item[itemInner].ToString());
                            loopcount += 1;
                        }
                        if (loopcount > 50)
                        {

                            HWlimitList.Add(count, HWList);
                            count += 1;
                            HWList = new List<string>();
                            loopcount = 1;
                        }
                    }
                    else if (itemInner.ToLower() == "IOSVersion".ToLower())
                    {
                        var iosfromRow = item[itemInner].ToString();
                        IOSVersionList.Add(iosfromRow.Trim());

                        //var spliVal = iosfromRow.Split(",");
                        //if (spliVal.Count() > 0)
                        //{
                        //    iosfromRow = spliVal[0];
                        //}
                        //if (!(string.IsNullOrEmpty(iosfromRow)))
                        //{
                        //    if (!(IOSVersionList.Contains(iosfromRow)))
                        //    {
                        //        IOSVersionList.Add(iosfromRow.Trim());
                        //    }
                        //}
                    }
                }
            }
            if (HWlimitList.Count == 0)
            {
                HWlimitList.Add(1, HWList);

            }

            // get tye values from EOL 
            var commandVal = new SearchByProduct();
            // string url = SearchByProduct.URL;
            var command = new ExecuteCommands();
            var resultAuth = await command.GetAuthToken(_EOLclientID, _EOLclientserect);
            foreach (var item in HWlimitList)
            {
                var result = await command.GetWebResponse<EOXResultByProduct>("https://apix.cisco.com/supporttools/eox/rest/5/EOXByProductID/1/", string.Join(",", item.Value).ToString() + "?responseencoding=json", resultAuth);
                if ((result != null) && (result.EOXRecord != null))
                    listEOL.EOXRecord.AddRange(result.EOXRecord);
            }

            foreach (var currentIOSVersion in IOSVersionList)
            {

                var spliVal = currentIOSVersion.Split(",");
                foreach (var currentIOS in spliVal)
                {
                    if (!(vulrCount.ContainsKey(currentIOS)))
                    {
                        var listVuln = new Vuln();
                        listVuln.advisories = new List<VulnStructure>();
                        listVuln = await GetAPIAdvisoryLists("advisories/ios?version={0}", currentIOS.ToString());

                        if (listVuln.advisories.Count > 0)
                        {
                            // listVuln = await GetAPIAdvisoryLists("advisories/ios?version={0}", currentIOSVersion);
                            vulrCount.Add(currentIOS, listVuln.advisories);
                        }
                        else
                        {
                            vulrCount.Add(currentIOS, new List<VulnStructure>());

                        }
                    }

                }


            }


            return new Tuple<EOXResultByProduct, List<Dictionary<string, object>>, Dictionary<string, List<VulnStructure>>>(listEOL, listOrion, vulrCount);
        }

        public async Task<Tuple<EOXResultByProduct, List<Dictionary<string, object>>, Dictionary<string, List<VulnStructure>>>> GetOrionVuln(string SearchType, string SearchText)
        {
            var listEOL = new EOXResultByProduct();
            listEOL.EOXRecord = new List<SearchByProductModel>();
            var listOrion = new List<Dictionary<string, object>>();


            string fields = "Client as C_Client,NodeName,IPAddress,HW_Model as c_HW_Model,HW_SerialNumber as c_HW_SerialNumber, Rpt_Liz_Notes as c_Rpt_Liz_Notes, Rpt_BugID_s_ as c_Rpt_BugID_s_, Rpt_BugIDs_Critical as c_Rpt_BugIDs_Critical, Rpt_BugIDs_High as c_Rpt_BugIDs_High, Rpt_BugIDs_Medium as c_Rpt_BugIDs_Medium, Rpt_IOS_Recommended as c_Rpt_IOS_Recommended, IOSVersion, n.Description ";


            //where IOSVersion = '15.2(2)E5''
            var searchQuery = " and Client like '" + SearchText + "%'";
            listOrion = await GetListOrion(fields, searchQuery);
            var HWList = new List<string>();
            var IOSVersionList = new List<string>();
            var HWlimitList = new Dictionary<int, List<string>>();
            var vulrCount = new Dictionary<string, List<VulnStructure>>();
            var count = 1;
            var loopcount = 1;
            foreach (var item in listOrion)
            {
                foreach (var itemInner in item.Keys)
                {
                    if (itemInner.ToLower() == "c_HW_Model".ToLower())
                    {
                        if (!(HWList.Contains(item[itemInner].ToString())))
                        {
                            HWList.Add(item[itemInner].ToString());
                            loopcount += 1;
                        }
                        if (loopcount > 50)
                        {

                            HWlimitList.Add(count, HWList);
                            count += 1;
                            HWList = new List<string>();
                            loopcount = 1;
                        }
                    }
                    else if (itemInner.ToLower() == "IOSVersion".ToLower())
                    {
                        var iosfromRow = item[itemInner].ToString();
                        IOSVersionList.Add(iosfromRow.Trim());

                        //var spliVal = iosfromRow.Split(",");
                        //if (spliVal.Count() > 0)
                        //{
                        //    iosfromRow = spliVal[0];
                        //}
                        //if (!(string.IsNullOrEmpty(iosfromRow)))
                        //{
                        //    if (!(IOSVersionList.Contains(iosfromRow)))
                        //    {
                        //        IOSVersionList.Add(iosfromRow.Trim());
                        //    }
                        //}
                    }
                }
            }
            if (HWlimitList.Count == 0)
            {
                HWlimitList.Add(1, HWList);

            }

            // get tye values from EOL 
            var commandVal = new SearchByProduct();
            // string url = SearchByProduct.URL;
            var command = new ExecuteCommands();
            var resultAuth = await command.GetAuthToken(_EOLclientID, _EOLclientserect);
            foreach (var item in HWlimitList)
            {
                var result = await command.GetWebResponse<EOXResultByProduct>("https://apix.cisco.com/supporttools/eox/rest/5/EOXByProductID/1/", string.Join(",", item.Value).ToString() + "?responseencoding=json", resultAuth);
                if ((result != null) && (result.EOXRecord != null))
                    listEOL.EOXRecord.AddRange(result.EOXRecord);
            }

            foreach (var currentIOSVersion in IOSVersionList)
            {
                
                var spliVal = currentIOSVersion.Split(",");
                foreach (var currentIOS in spliVal)
                {
                    if (!(vulrCount.ContainsKey(currentIOS)))
                    {
                        var listVuln = new Vuln();
                        listVuln.advisories = new List<VulnStructure>();
                        listVuln = await GetAPIAdvisoryLists("advisories/ios?version={0}", currentIOS.ToString());

                        if (listVuln.advisories.Count > 0)
                        {
                            // listVuln = await GetAPIAdvisoryLists("advisories/ios?version={0}", currentIOSVersion);
                            vulrCount.Add(currentIOS, listVuln.advisories);
                        }
                        else
                        {
                            vulrCount.Add(currentIOS, new List<VulnStructure>());

                        }
                    }

                }

                
            }


            return new Tuple<EOXResultByProduct, List<Dictionary<string, object>>, Dictionary<string, List<VulnStructure>>>(listEOL, listOrion, vulrCount);
        }

        public async Task<Tuple<Vuln, List<Dictionary<string, object>>>> GetCombinationObject(string SearchType, string SearchText)
        {
            var listVuln = new Vuln();
            var listOrion = new List<Dictionary<string, object>>();

            switch (SearchType)
            {
                case "ios":
                    listVuln = await GetAPIAdvisoryLists("advisories/ios?version={0}", SearchText);
                    if (listVuln.advisories.Count > 0)
                    {
                        //where IOSVersion = '15.2(2)E5''
                        var searchQuery = " and Client like '" + SearchText + "%'";
                        listOrion = await GetListOrion("",searchQuery);
                    }

                    break;
                case "pname":
                    listVuln = await GetAPIAdvisoryLists("advisories/product?product={0}", SearchText);
                    if (listVuln.advisories.Count > 0)
                    {
                        var searchQuery = string.Empty;
                        listOrion = await GetListOrion("",searchQuery);
                    }
                    break;

                default:
                    break;
            }

            return new Tuple<Vuln, List<Dictionary<string, object>>>(listVuln, listOrion);
        }

        public async Task<Tuple<EOXResultByProduct, List<Dictionary<string, object>>, Dictionary<string, int>>> GetCiscoPSIRT(string SearchType, string SearchText)
        {
            var listEOL = new EOXResultByProduct();
            listEOL.EOXRecord = new List<SearchByProductModel>();
            var listOrion = new List<Dictionary<string, object>>();


            string fields = "Client as C_Client,NodeName,IPAddress,HW_Model as c_HW_Model,HW_SerialNumber as c_HW_SerialNumber, Rpt_Liz_Notes as c_Rpt_Liz_Notes, Rpt_BugID_s_ as c_Rpt_BugID_s_, Rpt_BugIDs_Critical as c_Rpt_BugIDs_Critical, Rpt_BugIDs_High as c_Rpt_BugIDs_High, Rpt_BugIDs_Medium as c_Rpt_BugIDs_Medium, Rpt_IOS_Recommended as c_Rpt_IOS_Recommended, IOSVersion, n.Description ";


            //where IOSVersion = '15.2(2)E5''
            var searchQuery = " and Client like '" + SearchText + "%'";
            listOrion = await GetListOrion(fields, searchQuery);
            var HWList = new List<string>();
            var IOSVersionList = new List<string>();
            var HWlimitList = new Dictionary<int, List<string>>();
            var vulrCount = new Dictionary<string, int>();
            var count = 1;
            var loopcount = 1;
            foreach (var item in listOrion)
            {
                foreach (var itemInner in item.Keys)
                {
                    if (itemInner.ToLower() == "c_HW_Model".ToLower())
                    {
                        if (!(HWList.Contains(item[itemInner].ToString())))
                        {
                            HWList.Add(item[itemInner].ToString());
                            loopcount += 1;
                        }
                        if (loopcount > 50)
                        {

                            HWlimitList.Add(count, HWList);
                            count += 1;
                            HWList = new List<string>();
                            loopcount = 1;
                        }
                    }
                    else if (itemInner.ToLower() == "IOSVersion".ToLower())
                    {
                        var iosfromRow = item[itemInner].ToString();
                        var spliVal = iosfromRow.Split(",");
                        if (spliVal.Count() > 0)
                        {
                            iosfromRow = spliVal[0];
                        }
                        if (!(string.IsNullOrEmpty(iosfromRow)))
                        {
                            if (!(IOSVersionList.Contains(iosfromRow)))
                            {
                                IOSVersionList.Add(iosfromRow.Trim());
                            }
                        }
                    }
                }
            }
            if (HWlimitList.Count == 0)
            {
                HWlimitList.Add(1, HWList);

            }

            // get tye values from EOL 
            var commandVal = new SearchByProduct();
            // string url = SearchByProduct.URL;
            var command = new ExecuteCommands();
            var resultAuth = await command.GetAuthToken(_EOLclientID, _EOLclientserect);
            foreach (var item in HWlimitList)
            {
                var result = await command.GetWebResponse<EOXResultByProduct>("https://apix.cisco.com/supporttools/eox/rest/5/EOXByProductID/1/", string.Join(",", item.Value).ToString() + "?responseencoding=json", resultAuth);
                if (result.EOXRecord != null)
                    listEOL.EOXRecord.AddRange(result.EOXRecord);
            }

            foreach (var currentIOSVersion in IOSVersionList)
            {
                var listVuln = new Vuln();
                listVuln.advisories = new List<VulnStructure>();
                // listVuln = await GetAPIAdvisoryLists("advisories/ios?version={0}", currentIOSVersion.ToString());
                listVuln = await GetAPIAdvisoryLists("advisories/ios?version={0}", currentIOSVersion);
                vulrCount.Add(currentIOSVersion, listVuln.advisories.Count());
            }


            return new Tuple<EOXResultByProduct, List<Dictionary<string, object>>, Dictionary<string, int>>(listEOL, listOrion, vulrCount);
        }

        public async Task<Tuple<EOXResultByProduct, List<Dictionary<string, object>>, Dictionary<string, int>, Dictionary<string,List<Bug>>, Dictionary<string, List<VulnStructure>>, List<string>>> GetCiscoMaster(string SearchType, string SearchText)
        {
            var listEOL = new EOXResultByProduct();
            listEOL.EOXRecord = new List<SearchByProductModel>();
            var listOrion = new List<Dictionary<string, object>>();
            var listBug = new Dictionary<string, List<Bug>>();
            var listAdvisoryList = new Dictionary<string, List<VulnStructure>>();
            var distImpact = new List<string>();


            string fields = "Rpt_SW, Client, Client_Site, Client as C_Client,NodeName,IPAddress,HW_Model as c_HW_Model,HW_SerialNumber as c_HW_SerialNumber, Rpt_Liz_Notes , Rpt_BugID_s_ as c_Rpt_BugID_s_, Rpt_BugIDs_Critical as c_Rpt_BugIDs_Critical, Rpt_BugIDs_High as c_Rpt_BugIDs_High, Rpt_BugIDs_Medium as c_Rpt_BugIDs_Medium, Rpt_IOS_Recommended as c_Rpt_IOS_Recommended, IOSVersion, n.Description, NodeDescription, StatusDescription, Rpt_Recommendation_from_Uebiz, Rpt_Recommendation_Action_Customer , Site_Category, Site_Severity";


            //where IOSVersion = '15.2(2)E5''
            var searchQuery = string.Empty;
            if (!(string.IsNullOrEmpty(SearchText)))
            {
                searchQuery = " and Client like '" + SearchText + "%'";
            }
            listOrion = await GetListOrion(fields, searchQuery);
            var HWList = new List<string>();
            var IOSVersionList = new List<string>();
            var HWlimitList = new Dictionary<int, List<string>>();
            var vulrCount = new Dictionary<string, int>();
            var count = 1;
            var loopcount = 1;
            foreach (var item in listOrion)
            {
                foreach (var itemInner in item.Keys)
                {
                    if (itemInner.ToLower() == "c_HW_Model".ToLower())
                    {
                        if (!(HWList.Contains(item[itemInner].ToString())))
                        {
                            HWList.Add(item[itemInner].ToString());
                            loopcount += 1;
                        }
                        if (loopcount > 50)
                        {

                            HWlimitList.Add(count, HWList);
                            count += 1;
                            HWList = new List<string>();
                            loopcount = 1;
                        }
                    }
                    else if (itemInner.ToLower() == "IOSVersion".ToLower())
                    {
                        var iosfromRow = item[itemInner].ToString();
                        var spliVal = iosfromRow.Split(",");
                        if (spliVal.Count() == 1)
                        {
                            spliVal = iosfromRow.Split(" ");
                        }
                        if (spliVal.Count() > 0)
                        {
                            iosfromRow = spliVal[0];
                            iosfromRow = iosfromRow.Trim().Replace(" ", "");
                        }
                        if (!(string.IsNullOrEmpty(iosfromRow)))
                        {
                            if (!(IOSVersionList.Contains(iosfromRow)))
                            {
                                IOSVersionList.Add(iosfromRow.Trim());
                            }
                        }
                    }
                }
            }
            if (HWlimitList.Count == 0)
            {
                HWlimitList.Add(1, HWList);

            }

            // get tye values from EOL 
            var commandVal = new SearchByProduct();
            // string url = SearchByProduct.URL;
            var command = new ExecuteCommands();
            var resultAuth = await command.GetAuthToken(_EOLclientID, _EOLclientserect);
           // foreach (var item in HWlimitList)
           // {
           //     var result = await command.GetWebResponse<EOXResultByProduct>("https://api.cisco.com/supporttools/eox/rest/5/EOXByProductID/1/", string.Join(",", item.Value).ToString() + "?responseencoding=json", resultAuth);
           //     if (result.EOXRecord != null)
           //         listEOL.EOXRecord.AddRange(result.EOXRecord);
           // }

           //var resultAuthBug = await command.GetAuthToken(_clientID, _clientserect);

            //foreach (var item in HWlimitList)
            //{

            //    foreach (var itemInner in item.Value)
            //    {
            //        try
            //        {
            //            var resultweb = await command.GetWebResponse<Bugs>("https://api.cisco.com/bug/v2.0/bugs/products/product_id/" + itemInner, "?page_index=1&modified_date=5", resultAuth);
            //            if (resultweb != null)
            //            {
            //                var bugs = new List<Bug>();
            //                bugs.AddRange(resultweb.bugs.ToList());
                            
            //                listBug.Add( itemInner, bugs);

            //            }
            //        }
            //        catch ( Exception ex)
            //        {

            //        }
            //    }
            //}

            foreach (var currentIOSVersion in IOSVersionList)
            {
                var listVuln = new Vuln();
                listVuln.advisories = new List<VulnStructure>();
                listVuln = await GetAPIAdvisoryLists("advisories/ios?version={0}&productNames=true", currentIOSVersion.ToString());
                if (listVuln.advisories.Count() == 0 )
                {
                    listVuln = await GetAPIAdvisoryLists("advisories/iosxe?version={0}&productNames=true", currentIOSVersion.ToString());
                }
                if (listVuln.advisories.Count() == 0)
                {
                    listVuln = await GetAPIAdvisoryLists("advisories/nxos?version={0}&productNames=true", currentIOSVersion.ToString());
                }
                if (listVuln.advisories.Count() == 0)
                {
                    listVuln = await GetAPIAdvisoryLists("advisories/aci?version={0}&productNames=true", currentIOSVersion.ToString());
                }


                // listVuln = await GetAPIAdvisoryLists("advisories/ios?version={0}", currentIOSVersion);
                vulrCount.Add(currentIOSVersion, listVuln.advisories.Count());
                listAdvisoryList.Add(currentIOSVersion, listVuln.advisories);
                var disntImpact = listVuln.advisories.Select(r => r.sir).Distinct().ToList();
                foreach (var itemImpact in disntImpact)
                {
                    if (!(distImpact.Contains(itemImpact)))
                    {
                        distImpact.Add(itemImpact);
                    }
                }
            }




            return new Tuple<EOXResultByProduct, List<Dictionary<string, object>>, Dictionary<string, int>, Dictionary<string,List<Bug>>, Dictionary<string, List<VulnStructure>>, List<string>>(listEOL, listOrion, vulrCount, listBug, listAdvisoryList, distImpact);
        }

        public async Task<Tuple<EOXResultByProduct, List<Dictionary<string, object>>, Dictionary<string, int>>> GetCiscoEOLHW(string SearchType, string SearchText)
        {
            var listEOL = new EOXResultByProduct();
            listEOL.EOXRecord = new List<SearchByProductModel>();
            var listOrion = new List<Dictionary<string, object>>();


            string fields = "Client, Client_Site, Client as C_Client,NodeName,IPAddress,HW_Model as c_HW_Model,HW_SerialNumber as c_HW_SerialNumber, Rpt_Liz_Notes as c_Rpt_Liz_Notes, Rpt_BugID_s_ as c_Rpt_BugID_s_, Rpt_BugIDs_Critical as c_Rpt_BugIDs_Critical, Rpt_BugIDs_High as c_Rpt_BugIDs_High, Rpt_BugIDs_Medium as c_Rpt_BugIDs_Medium, Rpt_IOS_Recommended as c_Rpt_IOS_Recommended, IOSVersion, n.Description, NodeDescription, StatusDescription ";


            //where IOSVersion = '15.2(2)E5''
            var searchQuery = string.Empty;
            if (!(string.IsNullOrEmpty(SearchText)))
            {
                searchQuery = " and Client like '" + SearchText + "%'";
            }
            listOrion = await GetListOrion(fields, searchQuery);
            var HWList = new List<string>();
            var IOSVersionList = new List<string>();
            var HWlimitList = new Dictionary<int, List<string>>();
            var vulrCount = new Dictionary<string, int>();
            var count = 1;
            var loopcount = 1;
            foreach (var item in listOrion)
            {
                foreach (var itemInner in item.Keys)
                {
                    if (itemInner.ToLower() == "c_HW_Model".ToLower())
                    {
                        if (!(HWList.Contains(item[itemInner].ToString())))
                        {
                            HWList.Add(item[itemInner].ToString());
                            loopcount += 1;
                        }
                        if (loopcount > 50)
                        {

                            HWlimitList.Add(count, HWList);
                            count += 1;
                            HWList = new List<string>();
                            loopcount = 1;
                        }
                    }
                    else if (itemInner.ToLower() == "IOSVersion".ToLower())
                    {
                        var iosfromRow = item[itemInner].ToString();
                        var spliVal = iosfromRow.Split(",");
                        if (spliVal.Count() > 0)
                        {
                            iosfromRow = spliVal[0];
                        }
                        if (!(string.IsNullOrEmpty(iosfromRow)))
                        {
                            if (!(IOSVersionList.Contains(iosfromRow)))
                            {
                                IOSVersionList.Add(iosfromRow.Trim());
                            }
                        }
                    }
                }
            }
            if (HWlimitList.Count == 0)
            {
                HWlimitList.Add(1, HWList);

            }

            // get tye values from EOL 
            var commandVal = new SearchByProduct();
            // string url = SearchByProduct.URL;
            var command = new ExecuteCommands();
            var resultAuth = await command.GetAuthToken(_EOLclientID, _EOLclientserect);
            foreach (var item in HWlimitList)
            {
                var result = await command.GetWebResponse<EOXResultByProduct>("https://apix.cisco.com/supporttools/eox/rest/5/EOXByProductID/1/", string.Join(",", item.Value).ToString() + "?responseencoding=json", resultAuth);
                if (result.EOXRecord != null)
                    listEOL.EOXRecord.AddRange(result.EOXRecord);
            }

            foreach (var currentIOSVersion in IOSVersionList)
            {
                var listVuln = new Vuln();
                listVuln.advisories = new List<VulnStructure>();
                //listVuln = await GetAPIAdvisoryLists("advisories/ios?version={0}", currentIOSVersion.ToString());
                // listVuln = await GetAPIAdvisoryLists("advisories/ios?version={0}", currentIOSVersion);
                //vulrCount.Add(currentIOSVersion, listVuln.advisories.Count());
            }


            return new Tuple<EOXResultByProduct, List<Dictionary<string, object>>, Dictionary<string, int>>(listEOL, listOrion, vulrCount);
        }

        public async Task<Tuple<EOXResultByProduct, List<Dictionary<string, object>>, Dictionary<string,int>>> GetConsolidatedObject(string SearchType, string SearchText)
        {
            var listEOL = new EOXResultByProduct();
            listEOL.EOXRecord = new List<SearchByProductModel>();
            var listOrion = new List<Dictionary<string, object>>();


            string fields = "Client_Site, NodeName as Node ,IPAddress, HW_Model , IOSVersion  ";

            //where IOSVersion = '15.2(2)E5''
            var searchQuery = " and Client like '" + SearchText + "%'";
            listOrion = await GetListOrion(fields, searchQuery);
            var HWList = new List<string>();
            var IOSVersionList = new List<string>();
            var HWlimitList = new Dictionary<int, List<string>>();
            var vulrCount = new Dictionary<string, int>();
            var count = 1;
            var loopcount = 1;
            foreach (var item in listOrion)
            {
                foreach (var itemInner in item.Keys)
                {
                    if (itemInner.ToLower() == "HW_Model".ToLower())
                    {
                        if (!(HWList.Contains(item[itemInner].ToString())))
                        {
                            HWList.Add(item[itemInner].ToString());
                            loopcount += 1;
                        }
                        if (loopcount > 50)
                        {

                            HWlimitList.Add(count, HWList);
                            count += 1;
                            HWList = new List<string>();
                            loopcount = 1;
                        }
                    }
                    else if (itemInner.ToLower() == "IOSVersion".ToLower())
                    {
                        var iosfromRow = item[itemInner].ToString();
                        var spliVal = iosfromRow.Split(",");
                        if (spliVal.Count() > 0)
                        {
                            iosfromRow = spliVal[0];
                        }
                        if (!(string.IsNullOrEmpty(iosfromRow)))
                        {
                            if (!(IOSVersionList.Contains(iosfromRow)))
                            {
                                IOSVersionList.Add(iosfromRow.Trim());
                            }
                        }
                    }
                }
            }
            if (HWlimitList.Count == 0)
            {
                HWlimitList.Add(1, HWList);

            }

            // get tye values from EOL 
            var commandVal = new SearchByProduct();
            // string url = SearchByProduct.URL;
            var command = new ExecuteCommands();
            var resultAuth = await command.GetAuthToken(_EOLclientID, _EOLclientserect);
            foreach (var item in HWlimitList)
            {
                var result = await command.GetWebResponse<EOXResultByProduct>("https://apix.cisco.com/supporttools/eox/rest/5/EOXByProductID/1/", string.Join(",", item.Value).ToString() + "?responseencoding=json", resultAuth);
                if (result.EOXRecord != null)
                    listEOL.EOXRecord.AddRange(result.EOXRecord);
            }

            foreach (var currentIOSVersion in IOSVersionList)
            {
                var listVuln = new Vuln();
                //listVuln = await GetAPIAdvisoryLists("advisories/ios?version={0}", currentIOSVersion.ToString());
                listVuln = await GetAPIAdvisoryLists("advisories/ios?version={0}", currentIOSVersion);
                vulrCount.Add(currentIOSVersion, listVuln.advisories.Count());
            }
           

            return new Tuple<EOXResultByProduct, List<Dictionary<string, object>>, Dictionary<string,int>>(listEOL, listOrion, vulrCount);
        }

        public async  Task<Tuple<EOXResultByProduct, List<Dictionary<string, object>>>> GetEOLOrionObject(string SearchType, string SearchText)
        {
            var listEOL = new EOXResultByProduct();
            listEOL.EOXRecord = new List<SearchByProductModel>();
            var listOrion = new List<Dictionary<string, object>>();


            string fields = "Client as C_Client,NodeName,IPAddress,HW_Model as c_HW_Model,HW_SerialNumber as c_HW_SerialNumber, Rpt_Liz_Notes as c_Rpt_Liz_Notes, Rpt_BugID_s_ as c_Rpt_BugID_s_, Rpt_BugIDs_Critical as c_Rpt_BugIDs_Critical, Rpt_BugIDs_High as c_Rpt_BugIDs_High, Rpt_BugIDs_Medium as c_Rpt_BugIDs_Medium, Rpt_IOS_Recommended as c_Rpt_IOS_Recommended, IOSVersion, n.Description, nd.SystemUpTime ";
                
            //where IOSVersion = '15.2(2)E5''
            var searchQuery = " and Client like '" + SearchText + "%'";
            listOrion = await GetListOrion(fields, searchQuery);
            var HWList = new List<string>();
            var HWlimitList = new Dictionary<int, List<string>>();
            var count = 1;
            var loopcount = 1;
            foreach (var item in listOrion)
            {
                foreach (var itemInner in item.Keys)
                {
                    if ( itemInner.ToLower() == "c_HW_Model".ToLower())
                    {
                        if (!(HWList.Contains(item[itemInner].ToString())))
                        {
                            HWList.Add(item[itemInner].ToString());
                            loopcount += 1;
                        }
                        if (loopcount > 50 )
                        {

                            HWlimitList.Add(count, HWList);
                            count += 1;
                            HWList = new List<string>();
                            loopcount = 1;
                        }
                    }
                }
            }
            if (HWlimitList.Count == 0 )
            {
                HWlimitList.Add(1, HWList);

            }

            // get tye values from EOL 
            var commandVal = new SearchByProduct();
           // string url = SearchByProduct.URL;
            var command = new ExecuteCommands();
            var resultAuth = await command.GetAuthToken(_clientID, _clientserect);
            foreach (var item in HWlimitList)
            {
                var result = await command.GetWebResponse<EOXResultByProduct>("https://apix.cisco.com/supporttools/eox/rest/5/EOXByProductID/1/", string.Join(",", item.Value).ToString() + "?responseencoding=json", resultAuth);
                if (result.EOXRecord != null )
                    listEOL.EOXRecord.AddRange(result.EOXRecord );
            }
            //var resultList = Newtonsoft.Json.JsonConvert.DeserializeObject<EOXResultByProduct>(result);

            return new Tuple<EOXResultByProduct, List<Dictionary<string, object>>>(listEOL, listOrion);
        }

        private async Task<List<Dictionary<string, object>>> GetListOrion(string inputfields , string searchQuery)
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
            if (!(string.IsNullOrEmpty(inputfields)))
            {
                fields = inputfields;
            }
            
            // string query = @"SELECT Top 100 " + fields + " FROM Orion.Nodes n  inner join Orion.NodesCustomProperties nc on nc.NodeID = n.nodeID where vendor = 'cisco' ";

            var swisClient = new SwisClient(_orionURL, _orionUserName, _orionPassword);
          //  string query = @"SELECT  " + fields + " FROM Orion.Nodes n  inner join Orion.NodesCustomProperties nc on nc.NodeID = n.nodeID inner join Orion.NodesStats nd on nd.nodeID = n.nodeID  where vendor = 'cisco' " + searchQuery;
            string query = @"SELECT  " + fields + " FROM Orion.Nodes n  inner join Orion.NodesCustomProperties nc on nc.NodeID = n.nodeID where vendor = 'cisco' " + searchQuery;
            JToken queryResult = swisClient.QueryAsync(query).Result;
            JObject joValues = JObject.Parse(queryResult.ToString());

            JArray arrayObject = (JArray)joValues["results"];

            var result = arrayObject.ToObject<IList<Dictionary<string, object>>>();

            return result.ToList();
        }

        private async Task<List<Bug>> GetAPIBugsList(string urlText, List<string> HWlimitList)
        {
            var result = new List<Bug>();
            try
            {
                var command = new ExecuteCommands();
                var resultAuth = await command.GetAuthToken(_EOLclientID, _EOLclientserect);
                foreach (var item in HWlimitList)
                {
                    var resultweb = await command.GetWebResponse<List<Bug>>("https://api.cisco.com/bug/v2.0/bugs/products/product_id/" + item , "??page_index=1&modified_date=5", resultAuth);
                    if (resultweb != null)
                        result.AddRange(resultweb);
                }
            }
            catch ( Exception ex)
            {
                throw ex;
            }

            return result;
        }

        private async Task<Vuln> GetAPIAdvisoryLists(string urlText, string searchText)
        {
            
            var listResult = new Vuln();
            try
            {
                listResult.advisories = new List<VulnStructure>();
                var splittedIDs = searchText.Split(",");
                foreach (var item in splittedIDs)
                {
                    var updatedURLText = string.Format(urlText, item);
                    var command = new ExecuteCommands();
                    var resultAuth = await command.GetAuthToken(_clientID, _clientserect);
                    var resultList = await command.GetWebResponse<Vuln>("https://api.cisco.com/security/", updatedURLText, resultAuth);
                    if (resultList.advisories != null)
                    {
                        foreach (var itemList in resultList.advisories)
                        {
                            listResult.advisories.Add(itemList);
                        }
                    }

                }
            }
            catch ( Exception ex )
            { }

            return listResult;
        }
    }
}
