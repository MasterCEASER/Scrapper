using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace ScraperHelper
{
    public enum HttpMethodType
    {
        GET,
        POST
    }

    public static class Helper
    {
        private static WebProxy proxyObj = null;

        static Helper()
        {
            System.Net.ServicePointManager.Expect100Continue = false;
        }

        private static void CreateProxyObj(String pIP, int pPort, String pUser, String pPswd)
        {
            proxyObj = new WebProxy(pIP, pPort /*port*/);
            proxyObj.Credentials = new NetworkCredential(pUser, pPswd);
        }

        public static String GetPagetHTML(String pURL, HttpRequestParam pParameter)
        {
            pParameter.URL = pURL;
            return GetPagetHTML(pParameter);
        }

        public static String GetPagetHTML(HttpRequestParam pParameter)
        {
            if (proxyObj != null && pParameter.UseProxyParam)
            {
                CreateProxyObj(pParameter.IPForProxy,
                    pParameter.PortForProxy,
                    pParameter.UserForProxy,
                    pParameter.PasswordForProxy);
            }

            var _wReq = (HttpWebRequest)WebRequest.Create(pParameter.URL);
            _wReq.Method = pParameter.HttpMethod;

            if (proxyObj != null)
            {
                _wReq.Proxy = proxyObj;
            }

            _wReq.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            _wReq.ContentType = "application/x-www-form-urlencoded";
            _wReq.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1)";

            if (!String.IsNullOrEmpty(pParameter.DataToPost))
            {
                byte[] byteData = Encoding.UTF8.GetBytes(pParameter.DataToPost);
                _wReq.ContentLength = byteData.Length;

                using (StreamWriter swRequestWriter = new StreamWriter(_wReq.GetRequestStream()))
                {
                    swRequestWriter.Write(pParameter.DataToPost);
                    swRequestWriter.Close();
                }
            }

            HttpWebResponse _wResp = (HttpWebResponse)_wReq.GetResponse();
            using (System.IO.StreamReader _sr = new System.IO.StreamReader(_wResp.GetResponseStream()))
            {
                String _respStr = _sr.ReadToEnd();
                return _respStr;
            }
        }

        public static Boolean DownloadFile(String pSrcPath, String pTargPath)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    client.Proxy = proxyObj;

                    client.DownloadFile(pSrcPath, pTargPath);
                    client.Dispose();
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }

    public class HttpRequestParam
    {
        public String URL { get; set; }
        public HttpMethodType HttpMethodType { get; set; }
        public String DataToPost { get; set; }

        public CookieCollection CookieCollection { get; set; }

        public Boolean UseProxyParam { get; set; }
        public String IPForProxy { get; set; }
        public int PortForProxy { get; set; }
        public String UserForProxy { get; set; }
        public String PasswordForProxy { get; set; }

        public HttpRequestParam()
        {
            this.UseProxyParam = false;
            this.HttpMethodType = ScraperHelper.HttpMethodType.GET;
            this.CookieCollection = new CookieCollection();
            this.DataToPost = "";
        }

        public String HttpMethod
        {
            get
            {
                if (this.HttpMethodType == ScraperHelper.HttpMethodType.GET)
                    return "GET";
                else
                    return "POST";
            }
        }
    }
}
