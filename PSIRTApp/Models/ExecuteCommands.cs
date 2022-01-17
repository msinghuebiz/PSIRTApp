using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
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

                

                var client = new HttpClient();
                client.DefaultRequestHeaders.Add("cache-control", "no-cache");
                client.DefaultRequestHeaders.Add("Host", "cloudsso.cisco.com");
                client.DefaultRequestHeaders.TryAddWithoutValidation("content-type", "application/x-www-form-urlencoded");
                client.DefaultRequestHeaders.TryAddWithoutValidation    ("Content-Length", "119");
                


                
                var response = await client.PostAsync(strXMLServer, null);

                //var client = new RestClient(strXMLServer);
                //var request1 = new RestRequest();
                //request1.Method = Method.Post;
                //request1.AddHeader("cache-control", "no-cache");
                //request1.AddHeader("content-type", "application/x-www-form-urlencoded");
                //request1.AddHeader("Host", "cloudsso.cisco.com");
                //request1.AddHeader("Content-Length", "119");
                //// request1.AddParameter("application/x-www-form-urlencoded", "grant_type=client_credentials&client_id=abc&client_secret=123", ParameterType.RequestBody);
                //RestResponse response1 = client.ExecuteAsync(request1).Result;
var stringValue = await response.Content.ReadAsStringAsync();

                var result = JsonConvert.DeserializeObject<AuthResult>(stringValue);
                responseFromServer = result.access_token;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return responseFromServer;


        }


        public async Task<T> GetWebResponse<T>(string url, string command, string authToken)
        {
           
            try
            {

                var client = new HttpClient();
                client.DefaultRequestHeaders.Add("cache-control", "no-cache");
                client.DefaultRequestHeaders.Add("Accept-Encoding", " gzip,deflate");
                client.DefaultRequestHeaders.Add("Host", "api.cisco.com");
                client.DefaultRequestHeaders.Add("Accept", "*/*");
                client.DefaultRequestHeaders.TryAddWithoutValidation("content-type", "application/x-www-form-urlencoded");
                client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer " + authToken);

                //client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue ("Bearer " ,  authToken);

                var response = client.GetAsync(url + command ).Result;

                //var client = new RestClient(url+ command);
                //var request1 = new RestRequest();
                //request1.Method = Method.Get;
                //request1.AddHeader("cache-control", "no-cache");
                //request1.AddHeader("Accept-Encoding", " gzip,deflate");
                //request1.AddHeader("content-type", "application/x-www-form-urlencoded");
                //request1.AddHeader("Host", "api.cisco.com");
                //request1.AddHeader("Authorization", "Bearer " + authToken);
                //client.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
                // request1.AddParameter("application/x-www-form-urlencoded", "grant_type=client_credentials&client_id=abc&client_secret=123", ParameterType.RequestBody);
                //                RestResponse response1 = client.ExecuteAsync(request1).Result;
                var stringValue = await  response.Content.ReadAsStringAsync();


                var data = JsonConvert.DeserializeObject<T>(stringValue.ToString());


                return data;

            }
            catch (Exception ex)
            {
                throw ex;
            }


        }
    }
}
