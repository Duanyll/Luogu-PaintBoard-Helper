using System;
using System.IO;
using System.Collections.Generic;
using System.Threading;

namespace lgpb
{
    class Program
    {
        static int NowX = 0;
        static int NowY = 0;
        static int XStart = 0;
        static int YStart = 0;
        static string[] TextToWrite;
        private static object Locker = new object();
        static bool MoveNext(){
            NowX++;
            if (NowX >= TextToWrite[NowY].Length)
            {
                NowY++;
                NowX = 0;
                if (NowY >= TextToWrite.Length)
                {
                    Console.WriteLine("绘制完成");
                    return false;
                }
            }
            return true;
        }

        static bool Draw(AJAXCrawler crawler,int X,int Y,int Color){
            crawler.Content = $"x={X}&y={Y}&color={Color}";
            string result = crawler.PostForm();
            System.Console.WriteLine(result);
            return result.Contains("200");
        }
        static void Main(string[] args)
        {
            List<AJAXCrawler> crawlers = new List<AJAXCrawler>();
            Console.WriteLine("请输入Cookie,一行一个,空行截止");
            string now;
            while ((now = Console.ReadLine()) != "")
            {
                AJAXCrawler crawler = new AJAXCrawler();
                crawler.Url = "https://www.luogu.org/paintBoard/paint";
                crawler.Cookies.Add(CookieParser.Parse(now, "https://luogu.org"));
                crawlers.Add(crawler);
            }
            XStart = int.Parse(args[0].Trim());
            YStart = int.Parse(args[1].Trim());
            if (args.Length > 2)
            {
                NowX = int.Parse(args[2].Trim());
                NowY = int.Parse(args[3].Trim());
            }
            TextToWrite = File.ReadAllLines("data");
            Console.WriteLine("操作开始");
            foreach (var i in crawlers)
            {
                Thread th = new Thread(() =>
                {
                    while (NowY < TextToWrite.Length)
                    {
                        Thread.Sleep(0);
                        lock (Locker)
                        {
                            while (TextToWrite[NowY][NowX] == ' ')
                            {
                                if(!MoveNext()){
                                    System.Console.WriteLine("操作完成");
                                    return;
                                }
                            }

                            if(Draw(i,XStart+NowX,YStart+NowY,9)){
                                Console.WriteLine($"已绘制{NowX},{NowY}");
                            }else{
                                Thread.Sleep(1000);
                                continue;
                            }                            

                            if(!MoveNext()){
                                System.Console.WriteLine("操作完成");
                                return;
                            }
                        }
                        Thread.Sleep(32000);
                    }
                    Console.WriteLine("操作完成");
                });
                th.Start();
            }
            while (true)
            {
                Console.ReadKey();
            }
        }
    }
}
