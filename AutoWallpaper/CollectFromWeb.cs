using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.IO;

namespace AutoWallpaper
{
    class WebRegex
    {
        public static string BAIDU = "ObjURL\":\"http.*?\"";
        public static string BING = "";
    }

    class CollectFromWeb
    {
        public List<string> GetURLs(string url, string strRegex)
        {
            var files = Hunter(url, strRegex).Result;
            return files;
        }

        public async Task<List<string>> Hunter(string url, string strRegex)
        {
            List<string> fileList = new List<string>();
            List<Task<HttpResponseMessage>> respList = new List<Task<HttpResponseMessage>>();
            HttpClient client = new HttpClient();
            try
            {
                HttpResponseMessage resp = await client.GetAsync(url);
                resp.EnsureSuccessStatusCode();
                string respBody = await resp.Content.ReadAsStringAsync();
                //Console.WriteLine(respBody);

                // get urls
                var cols = Regex.Matches(respBody, strRegex);
                for (int i = 0; i < cols.Count; i++)
                {
                    fileList.Add(cols[i].Value.Replace("ObjURL\":\"", "").Replace("\"", "").Replace("\\/", "/"));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return fileList;
        }

        //void Download(List<string>)
        //{

        //    //download
        //    try
        //    {
        //        HttpClient client2 = new HttpClient();
        //        //Console.WriteLine("req:" + result[i]);
        //        Task<HttpResponseMessage> resp2 = client2.GetAsync(result[i]);
        //        respList.Add(resp2);
        //        //resp2.EnsureSuccessStatusCode();
        //        //var bytes = await resp2.Content.ReadAsByteArrayAsync();

        //        //if (!Directory.Exists("wallpaper"))
        //        //{
        //        //    Directory.CreateDirectory("wallpaper");
        //        //}

        //        //string filename = Directory.GetCurrentDirectory() + "\\wallpaper\\" + (i + ".jpg");
        //        //Console.WriteLine("file downloading..........." + i);
        //        //File.WriteAllBytes(filename, bytes);
        //        //Console.WriteLine("file downloaded..........." + i);

        //        //fileList.Add(filename);

        //    }
        //    catch (Exception e)
        //    {
        //        //Console.WriteLine("req file error. " + e.Message);
        //    }
        //}
    }
}


