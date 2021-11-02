using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace PSIRTApp.Models
{
    public class ExecuteCommands
    {
        public async Task<string> GetAuthToken(string client_ID, string clientsecret)
        {
            string responseFromServer = string.Empty;

            try
            {
                string strXMLServer = string.Format("https://cloudsso.cisco.com/as/token.oauth2?grant_type=client_credentials&client_id={0}&client_secret={1}", client_ID, clientsecret);

                var client = new RestClient(strXMLServer);
                var request1 = new RestRequest(Method.POST);
                request1.AddHeader("cache-control", "no-cache");
                request1.AddHeader("content-type", "application/x-www-form-urlencoded");
                request1.AddHeader("Host", "cloudsso.cisco.com");
                request1.AddHeader("Content-Length", "119");
                // request1.AddParameter("application/x-www-form-urlencoded", "grant_type=client_credentials&client_id=abc&client_secret=123", ParameterType.RequestBody);
                IRestResponse response1 = client.Execute(request1);

                var result = JsonConvert.DeserializeObject<AuthResult>(response1.Content.ToString());
                responseFromServer = result.access_token;
            }
            catch (Exception ex)
            {

            }
            return responseFromServer;


        }


        public async Task<T> GetWebResponse<T>(string url, string command, string authToken)
        {
           
            try
            {
                var client = new RestClient(url+ command);
                var request1 = new RestRequest(Method.GET);
                request1.AddHeader("cache-control", "no-cache");
                request1.AddHeader("Accept-Encoding", " gzip,deflate");
                request1.AddHeader("content-type", "application/x-www-form-urlencoded");
                request1.AddHeader("Host", "api.cisco.com");
                request1.AddHeader("Authorization", "Bearer " + authToken);
                client.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
                // request1.AddParameter("application/x-www-form-urlencoded", "grant_type=client_credentials&client_id=abc&client_secret=123", ParameterType.RequestBody);
                IRestResponse response1 = client.Execute(request1);

                var data = JsonConvert.DeserializeObject<T>(response1.Content);


                return data;

            }
            catch (Exception ex)
            {
                throw ex;
            }


        }
    }
}
