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
                    try
                    {
                        Console.WriteLine("     .. waitfor: " + i);

                        var imgResp = await respList[i];
                        if (!imgResp.IsSuccessStatusCode)
                        {
                            Console.WriteLine("code: " + imgResp.StatusCode);
                            continue;
                        }
                        //img.EnsureSuccessStatusCode();
                        var bytes = await imgResp.Content.ReadAsByteArrayAsync();

                        SaveToImage(bytes, imgResp.RequestMessage.RequestUri.AbsoluteUri);
                    }
                    catch (WebException e)
                    {
                        Console.WriteLine("web ex: " + e.InnerException.Message);
                    }
                    catch (HttpRequestException e)
                    {
                        Console.WriteLine("req ex: " + e.InnerException.Message);
                        Console.WriteLine("uri: " + respList[i].Result.RequestMessage.RequestUri.AbsoluteUri);
                    }
                    catch (TaskCanceledException ex)
                    {
                        Console.WriteLine("task cancel: " + ex.Message);
                    }
                    catch (System.Exception ex)
                    {
                        Console.WriteLine("resp error: " + ex.Message);

                        Console.WriteLine("" + respList[i].Result.RequestMessage.RequestUri.AbsoluteUri);
                    }


                }
                catch (AggregateException ex)
                {
                    Console.WriteLine("...---");
                }
            }

            Console.WriteLine("total: " + respList.Count);
        }


        static int c = 0;
        public void SaveToImage(byte[] bytes, string uri)
        {
            //保存
            var imgstream = new MemoryStream(bytes);
            var img = Image.FromStream(imgstream);

            if (img.Width >= 1920 && img.Height >= 1080)
            {
                //记录hash
                var hash = CalculateHash(img);
                var existHash = FindExist(hash);
                if (existHash != null)
                {
                    Console.WriteLine("repeat");
                    Console.WriteLine("filename: " + existHash.file);
                    Console.WriteLine("uri: " + existHash.uri);
                    return;
                }

                if (!Directory.Exists("wallpaper"))
                {
                    Directory.CreateDirectory("wallpaper");
                }
                string filename = (c++ + ".jpg");
                string filepath = Directory.GetCurrentDirectory() + "\\wallpaper\\" + filename;

                File.WriteAllBytes(filepath, bytes);
                Console.WriteLine("witefile: " + filepath);

                //不存在则创建
                var hInfo = new HashInfo
                {
                    file = filename,
                    hash = hash,
                    uri = uri
                };
                imgHashList.Add(hInfo);
            }
        }

        List<HashInfo> imgHashList = new List<HashInfo>();
        class HashInfo
        {
            public string file;
            public string uri;
            public bool[] hash;
        }

        bool[] CalculateHash(Image img)
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
            return hash;
        }

        HashInfo FindExist(bool[] hash)
        {
            //与每个hash比较
            var hItr = imgHashList.GetEnumerator();
            while (hItr.MoveNext())
            {
                var curHash = hItr.Current.hash;
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
                            return hItr.Current;
                        }
                    }
                }
            }
            //打印hash
            //for (int i = 0; i < hash.Length; i++)
            //{
            //    if (hash[i])
            //    {
            //        Console.Write(".");
            //    }
            //    else
            //    {
            //        Console.Write("-");
            //    }
            //}
            //Console.WriteLine();
            return null;
        }
    }
}
