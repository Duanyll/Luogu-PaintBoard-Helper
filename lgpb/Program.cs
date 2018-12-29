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
        const int XStart = 739;
        const int YStart = 235;
        private static object Locker = new object();
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
            string[] TextToWrite = File.ReadAllLines("data");
            Console.WriteLine("操作开始");
            foreach (var i in crawlers)
            {
                Thread th = new Thread(() =>
                {
                    while (NowY < TextToWrite.Length)
                    {
                        lock (Locker)
                        {
                            while (TextToWrite[NowX][NowY] == ' ')
                            {
                                NowX++;
                                if (NowX >= TextToWrite[NowX].Length)
                                {
                                    NowY++;
                                    NowX = 0;
                                    if (NowY >= TextToWrite.Length)
                                    {
                                        Console.WriteLine("绘制完成");
                                        return;
                                    }
                                }
                            }

                            i.Content = $"x={XStart + NowX}&y={YStart + NowY}&color=20";
                            string result = i.PostForm();
                            Console.WriteLine(result);
                            Console.WriteLine($"已绘制{NowX},{NowY}");

                            NowX++;
                            if (NowX >= TextToWrite[NowX].Length)
                            {
                                NowY++;
                                NowX = 0;
                                if (NowY >= TextToWrite.Length)
                                {
                                    Console.WriteLine("绘制完成");
                                    return;
                                }
                            }
                        }
                        Thread.Sleep(32000);
                    }
                    Console.WriteLine("绘制完成");
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
