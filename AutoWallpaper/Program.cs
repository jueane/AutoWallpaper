﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Configuration;
using System.Deployment.Application;
using System.IO;

namespace AutoWallpaper
{
    class Program
    {
        protected static void SimpleHandler(object sender, ConsoleCancelEventArgs args)
        {
            args.Cancel = true;
        }

        static void Main(string[] args)
        {
            Console.CancelKeyPress += new ConsoleCancelEventHandler(SimpleHandler);
            if (args.Length != 0 && args[0].Equals("-d"))
            {
                //string url = "https://image.baidu.com/search/index?ct=&z=&tn=baiduimage&ipn=r&word=%E5%A3%81%E7%BA%B8&pn=0&istype=2&ie=utf-8&oe=utf-8&cl=&lm=-1&st=-1&fr=&fmq=1536207041692_R&ic=0&se=&sme=&width=1920&height=1080&face=0";

                string url = "https://image.baidu.com/search/acjson?tn=resultjson_com&ipn=rj&ct=201326592&is=&fp=result&queryWord=%E5%A3%81%E7%BA%B8&cl=2&lm=-1&ie=utf-8&oe=utf-8&adpicid=&st=-1&z=&ic=0&word=%E5%A3%81%E7%BA%B8&s=&se=&tab=&width=&height=&face=0&istype=2&qc=&nc=1&fr=&cg=wallpaper&pn=0&rn=200&gsm=1e&1536227978695=";

                string keyword = File.ReadAllText("keyword.txt");
                if (string.IsNullOrWhiteSpace(keyword))
                {
                    keyword = "美女1920%201080";
                }
                Console.WriteLine("keyword: " + keyword);
                int i = 0;
                while (true)
                {

                    string urlBaidu = "https://image.baidu.com/search/acjson?tn=resultjson_com&ipn=rj&ct=201326592&is=&fp=result&queryWord=" + keyword + "&cl=2&lm=-1&ie=utf-8&oe=utf-8&adpicid=&st=-1&z=&ic=0&word=" + keyword + "&s=&se=&tab=&width=&height=&face=0&istype=2&qc=&nc=1&fr=&pn=" + (30 * i) + "&rn=30&gsm=1e&1536347366095=";

                    Collect(urlBaidu, WebRegex.BAIDU);


                    //int begin = i * 35 + 1;
                    //int end = (i + 1) * 35;

                    //string urlBing = "https://cn.bing.com/images/async?q=" + keyword + "&first=" + begin + "&count=" + end + "&relp=35&qft=+filterui%3aimagesize-large&lostate=r&mmasync=1&dgState=x*0_y*0_h*0_c*3_i*36_r*9&IG=D0F95A9C4244401C9F17B60309634404&SFX=2&iid=images.5730";

                    //Collect(urlBing, WebRegex.BING);


                    Thread.Sleep(5000);

                    i++;
                }

            }
            else if (args.Length != 0 && args[0].Equals("-r"))
            {
                var d = DataStore.Read();
                var path = d.currentImage;
                var dir = path.Substring(0, path.LastIndexOf("\\") + 1);
                var filename = path.Substring(path.LastIndexOf("\\") + 1);

                var destpath = "Deleted\\" + dir;

                Console.WriteLine("a: " + destpath);
                Console.WriteLine("b: " + filename);

                Directory.CreateDirectory(destpath);

                if (File.Exists(path))
                    //File.Delete(d.currentImage);
                    File.Move(path, destpath + filename);
            }
            else
            {
                AutoChangeBackground.Begin();
            }
        }

        static SimpleAsyncDownload down = new SimpleAsyncDownload();

        static void Collect(string url, string pattern)
        {

            var baidu = new CollectFromWeb();
            var urls = baidu.GetURLs(url, pattern);

            down.Download(urls);
        }
    }
}
