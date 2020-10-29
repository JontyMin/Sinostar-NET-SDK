using System;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using RestSharp;

namespace Sinostar.API
{
    public class SinostarClient
    {
        private RestClient _client;

        public SinostarClient()
        {
            _client = new RestClient(ApiUrl);
        }

        /// <summary>
        /// This is the logistics service request address
        /// </summary>
        public static string ApiUrl { get; set; } = "http://www.baxida.cn/default/svc/web-service";

        /// <summary>
        /// AppId (Actually this is a token)
        /// </summary>
        private static string appKey = "your appKey";

        /// <summary>
        /// AppToken (Same as above)
        /// </summary>
        private static string appToken = "your token";

        /// <summary>
        /// This is the sdk version
        /// </summary>
        private static string sdkVersion = "BXIDA-NET-SDK-1.0.0";



        #region public method

        private string Json(string jsonStr, string method)
        {
            var requestXML = RequestXml(jsonStr, method);

            // The default request method is POST
            var request = new RestRequest(Method.POST);

            // Add request header
            request.AddHeader("Content-Type", "application/xml");

            // Add request parameter
            request.AddParameter("application.xml", requestXML, ParameterType.RequestBody);

            // UserAgent
            _client.UserAgent = sdkVersion;

            // Send request
            IRestResponse response = _client.Execute(request);

            ValidateResponse(response);

            return response.Content;


        }

        /// <summary>
        /// Validate Response
        /// </summary>
        /// <param name="response">response content</param>
        private void ValidateResponse(IRestResponse response)
        {
            if (response.ErrorException != null)
                throw response.ErrorException;

            if (!new Object[] { HttpStatusCode.OK, HttpStatusCode.Created }.Contains(response.StatusCode))
            {
                ErrorMsg error = JsonConvert.DeserializeObject<ErrorMsg>(response.Content);
                throw new SinostarException(string.Concat(error.Status, " - ", error.Message, ": ", error.Error), null);
            }
        }


        /// <summary>
        /// Requested xml
        /// </summary>
        /// <param name="jsonStr">json format request parameters</param>
        /// <param name="method">Request method name</param>
        /// <returns></returns>
        private static string RequestXml(string jsonStr, string method)
        {
            return $@"<?xml version='1.0' encoding='UTF-8'?>
        <SOAP-ENV:Envelope xmlns:SOAP-ENV='http://schemas.xmlsoap.org/soap/envelope/' xmlns:ns1='http://www.example.org/Ec/'>
        <SOAP-ENV:Body>
        <ns1:callService>			
        <paramsJson>{jsonStr}</paramsJson>
        <appToken>{appKey}</appToken>
        <appKey>{appToken}</appKey>
        <service>{method}</service>
        </ns1:callService>
        </SOAP-ENV:Body>
        </SOAP-ENV:Envelope>";
        }

        #endregion
    }
}