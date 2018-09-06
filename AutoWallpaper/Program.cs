using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoWallpaper
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 0 && args[0].Equals("-d"))
            {
                //string url = "https://image.baidu.com/search/index?ct=&z=&tn=baiduimage&ipn=r&word=%E5%A3%81%E7%BA%B8&pn=0&istype=2&ie=utf-8&oe=utf-8&cl=&lm=-1&st=-1&fr=&fmq=1536207041692_R&ic=0&se=&sme=&width=1920&height=1080&face=0";

                string url = "https://image.baidu.com/search/acjson?tn=resultjson_com&ipn=rj&ct=201326592&is=&fp=result&queryWord=%E5%A3%81%E7%BA%B8&cl=2&lm=-1&ie=utf-8&oe=utf-8&adpicid=&st=-1&z=&ic=0&word=%E5%A3%81%E7%BA%B8&s=&se=&tab=&width=&height=&face=0&istype=2&qc=&nc=1&fr=&cg=wallpaper&pn=0&rn=200&gsm=1e&1536227978695=";


                string keyword = "美女1920%201080";
                string urlBing = "https://cn.bing.com/images/async?q=" + keyword + "&first=30&count=80&relp=35&qft=+filterui%3aimagesize-large&lostate=r&mmasync=1&dgState=x*0_y*0_h*0_c*3_i*36_r*9&IG=D0F95A9C4244401C9F17B60309634404&SFX=2&iid=images.5730";


                //Collect(url, WebRegex.BAIDU);
                Collect(urlBing, WebRegex.BING);

            }
            else
            {
                AutoChangeBackground.Begin(1000 * 3);
            }
        }

        static void Collect(string url, string pattern)
        {

            var baidu = new CollectFromWeb();
            var urls = baidu.GetURLs(url, pattern);

            SimpleAsyncDownload down = new SimpleAsyncDownload();
            down.Download(urls);
        }
    }
}
