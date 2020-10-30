using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using RestSharp;
using Sinostar.API.Model;

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


        /// <summary>
        /// 创建巴西达订单
        /// </summary>
        /// <param name="jsonStr"></param>
        /// <returns></returns>
        public CreateResponse CreateOrder(string jsonStr)
        {
            var json = Json(jsonStr, "createOrder");
            return JsonConvert.DeserializeObject<CreateResponse>(json);
        }

        /// <summary>
        /// 获取跟踪号
        /// </summary>
        /// <param name="reference">创建订单返回此参数</param>
        /// <returns></returns>
        public TrackingResponse GetTrackingNumber(string reference)
        {
            string reJson = "{\"reference_no\":[\"" + reference + "\"]}";
            var json = Json(reJson, "getTrackNumber");
            return JsonConvert.DeserializeObject<TrackingResponse>(json);
        }


        #endregion


        #region private method

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

            //return response.Content;
            // todo Use regular expression matching
            var startIndexOf = response.Content.IndexOf('{');
            var endIndexOf = response.Content.LastIndexOf('}') + 1;
            var json = response.Content.Substring(startIndexOf, endIndexOf - startIndexOf);

            return json;
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

    #region 跟踪号

    public class DataItem
    {
        /// <summary>
        /// 
        /// </summary>
        public string OrderNumber { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string TrackingNumber { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string WayBillNumber { get; set; }
    }

    public class TrackingResponse
    {
        /// <summary>
        /// 
        /// </summary>
        public string ask { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<DataItem> data { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<string> Error { get; set; }
        /// <summary>
        /// 全部获取成功
        /// </summary>
        public string message { get; set; }
    }

    #endregion


}