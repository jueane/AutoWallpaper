using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.IO;

namespace AutoWallpaper
{
    class SimpleAsyncDownload
    {
        public List<string> Download(List<string> urls)
        {
            var files = Request(urls);
            return files.Result;
        }

        public async Task<List<string>> Request(List<string> urls)
        {
            List<string> fileList = new List<string>();

            List<Task<HttpResponseMessage>> respList = new List<Task<HttpResponseMessage>>();
            // download
            for (int i = 0; i < urls.Count; i++)
            {
                try
                {
                    var client = new HttpClient();
                    var resp = client.GetAsync(urls[i]);
                    respList.Add(resp);
                }
                catch (System.Exception ex)
                {
                    Console.WriteLine("download error: " + ex.Message);
                }
            }

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
