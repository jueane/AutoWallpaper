using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.IO;
using System.Drawing;

namespace AutoWallpaper
{
    class SimpleAsyncDownload
    {
        public void Download(List<string> urls)
        {
            Request(urls).Wait();
        }

        public async Task Request(List<string> urls)
        {

            List<Task<HttpResponseMessage>> respList = new List<Task<HttpResponseMessage>>();
            // download
            for (int i = 0; i < urls.Count; i++)
            {
                //Thread.Sleep(1000);
                try
                {
                    var client = new HttpClient();
                    var resp = client.GetAsync(urls[i]);
                    respList.Add(resp);
                }
                catch (System.Exception ex)
                {
                    Console.WriteLine("req error: " + ex.Message);
                }
            }

            //save to files
            for (int i = 0; i < respList.Count; i++)
            {
                try
                {
                    Console.WriteLine("     .. waitfor: " + i);

                    var img = await respList[i];
                    if (!img.IsSuccessStatusCode)
                    {
                        Console.WriteLine("code: " + img.StatusCode);
                        continue;
                    }
                    //img.EnsureSuccessStatusCode();
                    var bytes = await img.Content.ReadAsByteArrayAsync();
                    SaveToImage(bytes);
                }
                catch (WebException e)
                {
                    Console.WriteLine("web ex: " + e.InnerException.Message);
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine("req ex: " + e.InnerException.Message);
                }
                catch (System.Exception ex)
                {
                    Console.WriteLine("resp error: " + ex.Message);

                    Console.WriteLine("" + respList[i].Result.RequestMessage.RequestUri.AbsoluteUri);
                }
            }

            Console.WriteLine("total: " + respList.Count);
        }

        List<bool[]> imgHashList = new List<bool[]>();

        static int c = 0;
        public void SaveToImage(byte[] bytes)
        {
            //保存
            var imgstream = new MemoryStream(bytes);
            var img = Image.FromStream(imgstream);

            if (img.Width >= 1920 && img.Height >= 1080)
            {
                //记录hash
                if (IsExist(img))
                {
                    Console.WriteLine("exist");
                    return;
                }

                if (!Directory.Exists("wallpaper"))
                {
                    Directory.CreateDirectory("wallpaper");
                }
                string filename = Directory.GetCurrentDirectory() + "\\wallpaper\\" + (c++ + ".jpg");
                File.WriteAllBytes(filename, bytes);
                Console.WriteLine("witefile: " + filename);
                //Console.WriteLine("file downloaded..........." + c);

            }
        }

        bool IsExist(Image img)
        {
            bool[] hash = new bool[16 * 16];
            Bitmap bmp = new Bitmap(img, new Size(16, 16));
            for (int i = 0; i < bmp.Height; i++)
            {
                for (int j = 0; j < bmp.Width; j++)
                {
                    var pos = i * 16 + j;
                    hash[pos] = bmp.GetPixel(j, i).GetBrightness() < 0.5;
                }
            }
            //与每个hash比较
            var hItr = imgHashList.GetEnumerator();
            while (hItr.MoveNext())
            {
                var curHash = hItr.Current;
                for (int i = 0; i < hash.Length; i++)
                {
                    if (curHash[i] != hash[i])
                    {
                        break;
                    }
                    else
                    {
                        // they are same
                        if (i == hash.Length - 1)
                        {
                            Console.WriteLine("repeat");
                            return true;
                        }
                    }
                }
            }

            //if (imgHashList.Contains(hash))
            //{
            //    return true;
            //}
            for (int i = 0; i < hash.Length; i++)
            {
                if (hash[i])
                {
                    Console.Write(".");
                }
                else
                {
                    Console.Write("-");
                }
            }
            Console.WriteLine();
            imgHashList.Add(hash);
            return false;
        }
    }
}
