using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace lgpb
{
    public class AJAXCrawler
    {
        public CookieContainer Cookies = new CookieContainer();
        public string Url = "";
        public string Content = "";
        const string UA = "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/70.0.3538.77 Safari/537.36";

        public async Task<string> PostFormAsync()
        {
            return await Task<string>.Run(() =>
            {
                string result = null;
                try
                {
                    HttpWebRequest request = WebRequest.CreateHttp(Url);
                    request.Method = "POST";
                    request.ContentType = "application/x-www-form-urlencoded";
                    request.ContentLength = Encoding.UTF8.GetByteCount(Content);
                    request.CookieContainer = Cookies;
                    StreamWriter requestStream = new StreamWriter(request.GetRequestStream());
                    requestStream.Write(Content);
                    requestStream.Close();
                    request.AllowAutoRedirect = false;
                    request.UserAgent = UA;
                    request.Headers.Add("x-requested-with", "XMLHttpRequest");

                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    response.Cookies = Cookies.GetCookies(response.ResponseUri);
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    result = reader.ReadToEnd();
                    reader.Close();
                }
                finally
                {
                }
                return result;
            });
        }

        public string PostForm(){
            string result = null;
            try
            {
                HttpWebRequest request = WebRequest.CreateHttp(Url);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = Encoding.UTF8.GetByteCount(Content);
                request.CookieContainer = Cookies;
                StreamWriter requestStream = new StreamWriter(request.GetRequestStream());
                requestStream.Write(Content);
                requestStream.Close();
                request.AllowAutoRedirect = false;
                request.UserAgent = UA;
                request.Headers.Add("x-requested-with", "XMLHttpRequest");

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                response.Cookies = Cookies.GetCookies(response.ResponseUri);
                StreamReader reader = new StreamReader(response.GetResponseStream());
                result = reader.ReadToEnd();
                reader.Close();
            }
            catch(WebException e){
                System.Console.WriteLine(e.Message);
                return "";
            }
            finally
            {
            }
            return result;
        }

        public async Task<string> PostAsync()
        {
            return await Task<string>.Run(() =>
            {
                string result = null;
                try
                {
                    HttpWebRequest request = WebRequest.CreateHttp(Url);
                    request.Method = "POST";
                    request.ContentType = "application/x-www-form-urlencoded";
                    request.ContentLength = Encoding.UTF8.GetByteCount(Content);
                    request.CookieContainer = Cookies;
                    request.AllowAutoRedirect = false;
                    request.UserAgent = UA;
                    request.Headers.Add("x-requested-with", "XMLHttpRequest");

                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    response.Cookies = Cookies.GetCookies(response.ResponseUri);
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    result = reader.ReadToEnd();
                    reader.Close();
                }
                finally
                {
                }
                return result;
            });
        }

        public async Task<string> GetAsync()
        {
            return await Task<string>.Run(() =>
            {
                string result = null;
                try
                {
                    HttpWebRequest request = WebRequest.CreateHttp(Url);
                    request.Method = "GET";
                    request.CookieContainer = Cookies;
                    request.AllowAutoRedirect = true;
                    request.UserAgent = UA;

                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    response.Cookies = Cookies.GetCookies(response.ResponseUri);
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    result = reader.ReadToEnd();
                    reader.Close();
                }
                catch(WebException ex){
                    Debug.WriteLine(ex);
                    return "";
                }
                finally
                {
                }
                return result;
            });
        }
    }
}