using System;
using System.IO;
using System.Collections.Generic;
using System.Threading;
using System.Drawing;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System.Text.RegularExpressions;

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
        static bool MoveNext()
        {
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

        static bool MoveNext(int w,int h)
        {
            NowX++;
            if (NowX >= w)
            {
                NowY++;
                NowX = 0;
                if (NowY >= h)
                {
                    Console.WriteLine("绘制完成");
                    return false;
                }
            }
            return true;
        }

        static string[] ColorList = {
            "rgb(0, 0, 0)", "rgb(255, 255, 255)", "rgb(170, 170, 170)", "rgb(85, 85, 85)", "rgb(254, 211, 199)", "rgb(255, 196, 206)", "rgb(250, 172, 142)", "rgb(255, 139, 131)", "rgb(244, 67, 54)", "rgb(233, 30, 99)", "rgb(226, 102, 158)", "rgb(156, 39, 176)", "rgb(103, 58, 183)", "rgb(63, 81, 181)", "rgb(0, 70, 112)", "rgb(5, 113, 151)", "rgb(33, 150, 243)", "rgb(0, 188, 212)", "rgb(59, 229, 219)", "rgb(151, 253, 220)", "rgb(22, 115, 0)", "rgb(55, 169, 60)", "rgb(137, 230, 66)", "rgb(215, 255, 7)", "rgb(255, 246, 209)", "rgb(248, 203, 140)", "rgb(255, 235, 59)", "rgb(255, 193, 7)", "rgb(255, 152, 0)", "rgb(255, 87, 34)", "rgb(184, 63, 39)", "rgb(121, 85, 72)"
        };
        static List<Rgba32> Colors = new List<Rgba32>();

        static Rgba32 LoadColorFromString(string str)
        {
            var vs = str.Substring(4, str.Length - 5).Replace(" ", "").Split(',');
            return new Rgba32(float.Parse(vs[0])/256, float.Parse(vs[1])/256, float.Parse(vs[2])/256);
        }

        static int GetColorID(Rgba32 col)
        {
            int mnval = int.MaxValue, mxpos = 0;
            for (int i = 0; i < Colors.Count; i++)
            {
                int dis = Math.Abs(Colors[i].R - col.R) + Math.Abs(Colors[i].G - col.G) + Math.Abs(Colors[i].B - col.B);
                if (dis < mnval)
                {
                    mxpos = i;
                    mnval = dis;
                }
            }
            return mxpos;
        }

        static bool Draw(AJAXCrawler crawler, int X, int Y, int Color)
        {
            crawler.Content = $"x={X}&y={Y}&color={Color}";
            string result = crawler.PostForm();
            System.Console.WriteLine(result);
            return result.Contains("200");
            // System.Console.WriteLine($"{X},{Y},{Color}");
            // return true;
        }

        static void WriteTextBackGround(List<AJAXCrawler> crawlers)
        {
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
                                if (!MoveNext())
                                {
                                    System.Console.WriteLine("操作完成");
                                    return;
                                }
                            }

                            if (Draw(i, XStart + NowX, YStart + NowY, 9))
                            {
                                Console.WriteLine($"已绘制{NowX},{NowY}");
                            }
                            else
                            {
                                Thread.Sleep(1000);
                                continue;
                            }

                            if (!MoveNext())
                            {
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
        }

        static void DrawImageBackGround(List<AJAXCrawler> crawlers)
        {
            Image<Rgba32> img = Image.Load(File.ReadAllBytes("image.png"));
            foreach (var i in ColorList)
            {
                Colors.Add(LoadColorFromString(i));
            }
            Console.WriteLine("操作开始");
            foreach (var i in crawlers)
            {
                Thread th = new Thread(() =>
                {
                    while (NowY < img.Height)
                    {
                        Thread.Sleep(0);
                        lock (Locker)
                        {
                            if (Draw(i, XStart + NowX, YStart + NowY, GetColorID(img[XStart + NowX,YStart + NowY])))
                            {
                                Console.WriteLine($"已绘制{NowX},{NowY}");
                            }
                            else
                            {
                                Thread.Sleep(1000);
                                continue;
                            }

                            if (!MoveNext(img.Width,img.Height))
                            {
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

            //WriteTextBackGround(crawlers);
            DrawImageBackGround(crawlers);

            while (true)
            {
                Console.ReadKey();
            }
        }
    }
}
