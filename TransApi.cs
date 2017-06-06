using System;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;

namespace BaiduFanyi
{
    public class TransReult
    {
        public string error_code { get; set; }
        public string error_msg { get; set; }
        public string from { get; set; }
        public string to { get; set; }
        public TransData[] trans_result { get; set; }
        public class TransData
        {
            public string src { get; set; }
            public string dst { get; set; }
        }
    }
    
    public class TransApi
    {
        private static string TRANS_API_HOST = "http://api.fanyi.baidu.com/api/trans/vip/translate";

        private string appid;
        private string securityKey;

        public TransApi(string appid, string securityKey)
        {
            this.appid = appid;
            this.securityKey = securityKey;
        }

        public string GetTransResult(string query, string from, string to)
        {
            var url = BuildUrl(query, from, to);
            using (HttpClient client = new HttpClient())
            {
                return client.GetStringAsync(url).Result;
            }
        }

        public TransReult GetTransResult2(string query, string from, string to)
        {
            var retJson = GetTransResult(query, from, to);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<TransReult>(retJson);
        }

        private string BuildUrl(string query, string from, String to)
        {
            var sb = new StringBuilder();
            sb.Append(TRANS_API_HOST);
            //var encodeQuery = Uri.EscapeUriString(query);
            var encodeQuery = WebUtility.UrlEncode(query);
            sb.Append("?q=").Append(encodeQuery);
            sb.Append("&from=").Append(from);
            sb.Append("&to=").Append(to);
            sb.Append("&appid=").Append(appid);
            var salt = DateTime.Now.Ticks.ToString();
            sb.Append("&salt=").Append(salt);

            var signSource = new StringBuilder();
            signSource.Append(appid).Append(query).Append(salt).Append(securityKey);
            var sign = GetMd5(signSource.ToString());
            sb.Append("&sign=").Append(sign.ToLower());
            return sb.ToString();
        }

        public static string GetMd5(string source)
        {            
            using (var ih = IncrementalHash.CreateHash(HashAlgorithmName.MD5))
            {
                ih.AppendData(Encoding.UTF8.GetBytes(source));
                var bytes = ih.GetHashAndReset();
                StringBuilder sbBytes = new StringBuilder(bytes.Length * 2);
                foreach (byte b in bytes)
                {
                    sbBytes.AppendFormat("{0:X2}", b);
                }
                return sbBytes.ToString();
            }
            
            //return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(source, "MD5");
        }
    }
}
