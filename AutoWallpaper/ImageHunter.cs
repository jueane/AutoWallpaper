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
    class ImageHunter
    {
        public async Task<List<string>> Hunter()
        {
            List<string> fileList = new List<string>();
            List<Task<HttpResponseMessage>> respList = new List<Task<HttpResponseMessage>>();
            HttpClient client = new HttpClient();
            try
            {
                string url = "https://image.baidu.com/search/index?ct=&z=&tn=baiduimage&ipn=r&word=%E5%A3%81%E7%BA%B8&pn=0&istype=2&ie=utf-8&oe=utf-8&cl=&lm=-1&st=-1&fr=&fmq=1536207041692_R&ic=0&se=&sme=&width=1920&height=1080&face=0";

                HttpResponseMessage resp = await client.GetAsync(url);
                resp.EnsureSuccessStatusCode();
                string respBody = await resp.Content.ReadAsStringAsync();
                //Console.WriteLine(respBody);

                // get urls
                string str = "ObjURL\":\"http.*?\"";

                var cols = Regex.Matches(respBody, str);

                List<string> result = new List<string>();
                for (int i = 0; i < cols.Count; i++)
                {
                    result.Add(cols[i].Value.Replace("ObjURL\":\"", "").Replace("\"", "").Replace("\\/", "/"));

                    //download
                    try
                    {
                        HttpClient client2 = new HttpClient();
                        //Console.WriteLine("req:" + result[i]);
                        Task<HttpResponseMessage> resp2 = client2.GetAsync(result[i]);
                        respList.Add(resp2);
                        //resp2.EnsureSuccessStatusCode();
                        //var bytes = await resp2.Content.ReadAsByteArrayAsync();

                        //if (!Directory.Exists("wallpaper"))
                        //{
                        //    Directory.CreateDirectory("wallpaper");
                        //}

                        //string filename = Directory.GetCurrentDirectory() + "\\wallpaper\\" + (i + ".jpg");
                        //Console.WriteLine("file downloading..........." + i);
                        //File.WriteAllBytes(filename, bytes);
                        //Console.WriteLine("file downloaded..........." + i);

                        //fileList.Add(filename);

                    }
                    catch (Exception e)
                    {
                        //Console.WriteLine("req file error. " + e.Message);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            client.Dispose();

            // download
            for (int i = 0; i < respList.Count; i++)
            {
                try
                {
                    var img = await respList[i];
                    img.EnsureSuccessStatusCode();
                    var bytes = await img.Content.ReadAsByteArrayAsync();

                    if (!Directory.Exists("wallpaper"))
                    {
                        Directory.CreateDirectory("wallpaper");
                    }

                    string filename = Directory.GetCurrentDirectory() + "\\wallpaper\\" + (i + ".jpg");
                    Console.WriteLine("file downloading begin..........." + i);
                    File.WriteAllBytes(filename, bytes);
                    Console.WriteLine("file downloaded..........." + i);

                    fileList.Add(filename);
                }
                catch (System.Exception ex)
                {
                    Console.WriteLine("download error: " + ex.Message);
                }
            }


            return fileList;
        }




    }
}
