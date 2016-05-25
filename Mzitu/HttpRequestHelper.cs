using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Mzitu
{
    class HttpRequestHelper
    {
        static HttpWebRequest request;
        static HttpWebResponse response;
        public static string Post_Data_xml(string url, string postdata, CookieContainer cc, string refer)
        {
            string temp = null;
            try
            {
                byte[] byteArray = Encoding.UTF8.GetBytes(postdata); // 转化
                request = (HttpWebRequest)WebRequest.Create(new Uri(url));
                request.CookieContainer = cc;
                request.Method = "POST";
                if (refer != "")
                {
                    request.Referer = refer;
                }
                request.ProtocolVersion = new Version("1.0");
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = byteArray.Length;
                Stream newStream = request.GetRequestStream();
                // Send the data.
                newStream.Write(byteArray, 0, byteArray.Length);    //写入参数
                newStream.Close();
                response = (HttpWebResponse)request.GetResponse();
                StreamReader str = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                temp = str.ReadToEnd();
            }
            catch (Exception ex)
            {

            }
            return temp;
        }

        public static string Post_Data(string url, string postdata, CookieContainer cc, string encoding, string refer)
        {
            string temp = null;
            Encoding encod = Encoding.GetEncoding(encoding);
            try
            {
                byte[] byteArray = encod.GetBytes(postdata); // 转化
                request = (HttpWebRequest)WebRequest.Create(new Uri(url));
                request.CookieContainer = cc;
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; InfoPath.1)";
                request.Method = "POST";
                if (refer != "")
                {
                    request.Referer = refer;
                }
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = byteArray.Length;
                Stream newStream = request.GetRequestStream();
                // Send the data.
                newStream.Write(byteArray, 0, byteArray.Length);    //写入参数
                newStream.Close();
                response = (HttpWebResponse)request.GetResponse();
                StreamReader str = new StreamReader(response.GetResponseStream(), encod);
                temp = str.ReadToEnd();
            }
            catch (Exception ex)
            {
                temp = ex.ToString();
            }
            return temp;
        }
        public static string Get_Data(string url, CookieContainer cc, string encoding, string refer)
        {
            string temp = null;
            try
            {
                request = (HttpWebRequest)WebRequest.Create(new Uri(url));
                request.CookieContainer = cc;
                request.Method = "GET";
                request.UserAgent =
                    " Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/49.0.2623.87 Safari/537.36";
                if (refer != "")
                {
                    request.Referer = refer;
                }
                request.ProtocolVersion = new Version("1.0");
                response = (HttpWebResponse)request.GetResponse();
                StreamReader str = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(encoding));
                temp = str.ReadToEnd();
            }
            catch (Exception ex)
            {

            }
            return temp;
        }
        public static WebHeaderCollection Get_Header(string url, CookieContainer cc, string encoding, string refer)
        {
            WebHeaderCollection temp = null;
            try
            {
                request = (HttpWebRequest)WebRequest.Create(new Uri(url));
                request.CookieContainer = cc;
                request.Accept = "*/*";
                request.Method = "GET";
                if (refer != "")
                {
                    request.Referer = refer;
                }
                request.ProtocolVersion = new Version("1.0");
                response = (HttpWebResponse)request.GetResponse();
                StreamReader str = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(encoding));
                temp = request.Headers;
            }
            catch (Exception ex)
            {

            }
            return temp;
        }

        public static Stream Get_ImgStream(string url, CookieContainer cc)
        {
            Stream temp = null;
            try
            {
                request = (HttpWebRequest)WebRequest.Create(new Uri(url));
                request.CookieContainer = cc;
                request.Method = "GET";
                response = (HttpWebResponse)request.GetResponse();
                temp = response.GetResponseStream();
                
            }
            catch (Exception ex)
            {

            }
            return temp;
        }
        public static Stream Get_ImgStream_refer(string url, CookieContainer cc, string refer)
        {
            Stream temp = null;
            try
            {
                request = (HttpWebRequest)WebRequest.Create(new Uri(url));
                request.CookieContainer = cc;
                request.Referer = refer;
                request.Method = "GET";
                response = (HttpWebResponse)request.GetResponse();
                temp = response.GetResponseStream();
            }
            catch (Exception ex)
            {

            }
            return temp;
        }
        /// <summary>
        /// 解压数据 - 此方法可以避免一些乱码出现
        /// </summary>
        /// <param name="strea">数据流</param>
        /// <returns>解压后的数据流</returns>
        public static Stream Decompress_GZIP(Stream stream)
        {
            GZipStream gstream = new GZipStream(stream, CompressionMode.Decompress);
            return gstream;
        }
        /// <summary>
        /// 压缩数据
        /// </summary>
        /// <param name="strea">数据流</param>
        /// <returns>压缩后的数据流</returns>
        public static Stream Compress_GZIP(Stream stream)
        {
            GZipStream gstream = new GZipStream(stream, CompressionMode.Compress);
            return gstream;
        }

        public static Image DownLoadImge(string Url, bool OpenProxy = false)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(new Uri(Url));
            if (!OpenProxy)//代理
            {
                req.Proxy = null;
            }
            HttpWebResponse res = (HttpWebResponse)req.GetResponse();
            Stream stream = res.GetResponseStream();
            Image img = Image.FromStream(stream);
            stream.Close();
            stream.Dispose();
            req.Abort();
            return img;
        }
    }
}
