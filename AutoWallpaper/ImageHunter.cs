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
        public async Task Hunter()
        {
            HttpClient client = new HttpClient();
            try
            {
                string url = "https://image.baidu.com/search/index?ct=201326592&z=&tn=baiduimage&ipn=r&word=%E5%A3%81%E7%BA%B8%20%E4%B8%8D%E5%90%8C%E9%A3%8E%E6%A0%BC%20%E7%BE%8E%E5%A5%B3&pn=0&istype=2&ie=utf-8&oe=utf-8&cl=2&lm=-1&st=-1&fr=&fmq=1536161737728_R&ic=0&se=&sme=&width=&height=&face=0";
                url = "https://www.baidu.com/";
                url = "https://image.baidu.com/search/index?tn=baiduimage&ipn=r&ct=201326592&cl=2&lm=-1&st=-1&fm=result&fr=&sf=1&fmq=1536162982584_R&pv=&ic=0&nc=1&z=&se=1&showtab=0&fb=0&width=&height=&face=0&istype=2&ie=utf-8&word=%E5%A3%81%E7%BA%B8";
                url = "https://image.baidu.com/search/index?ct=&z=&tn=baiduimage&ipn=r&word=%E7%BE%8E%E5%A5%B3&pn=0&istype=2&ie=utf-8&oe=utf-8&cl=&lm=-1&st=-1&fr=&fmq=1536172309597_R&ic=0&se=&sme=&width=1920&height=1080&face=0";
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
                        HttpResponseMessage resp2 = await client2.GetAsync(result[i]);
                        resp2.EnsureSuccessStatusCode();
                        var bytes = await resp2.Content.ReadAsByteArrayAsync();

                        if (!Directory.Exists("wallpaper"))
                        {
                            Directory.CreateDirectory("wallpaper");
                        }

                        string filename = Directory.GetCurrentDirectory() + "\\wallpaper\\" + (i + ".jpg");
                        Console.WriteLine("file downloading..........." + i);
                        File.WriteAllBytes(filename, bytes);
                        Console.WriteLine("file downloaded..........." + i);

                        // set
                        if (i % 1 == 0)
                        {
                            SetWallPaper.Set(filename);
                        }

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
        }




    }
}
