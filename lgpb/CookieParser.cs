using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;

namespace lgpb
{
    static class CookieParser
    {
        /// <summary>
        /// 一个到多个Cookie的字符串添加到CookieCollection集合中【isGood代码】
        /// </summary>
        /// <param name="s">Cookie的字符串</param>
        /// <param name="defaultDomain">站点主机部分</param>
        public static CookieCollection Parse(string s, string defaultDomain)
        {
            CookieCollection cc = new CookieCollection();
            if (string.IsNullOrEmpty(s) || s.Length < 5 || s.IndexOf("=") < 0) return cc;
            if (string.IsNullOrEmpty(defaultDomain) || defaultDomain.Length < 5) return cc;
            s.TrimEnd(new char[] { ';' }).Trim();
            Uri urI = new Uri(defaultDomain);
            defaultDomain = urI.Host.ToString();
            //用软件截取的cookie会带有expires，要把它替换掉【isGood代码】
            if (s.IndexOf("expires=") >= 0)
            {
                s = replace(s, @"expires=[\w\s,-:]*GMT[;]?", "");
            }
            //只有一个cookie直接添加【isGood代码】
            if (s.IndexOf(";") < 0)
            {
                System.Net.Cookie c = new System.Net.Cookie(s.Substring(0, s.IndexOf("=")), s.Substring(s.IndexOf("=") + 1));
                c.Domain = defaultDomain;
                cc.Add(c);
                return cc;
            }
            //不同站点与不同路径一般是以英文道号分别【isGood代码】
            if (s.IndexOf(",") > 0)
            {
                s.TrimEnd(new char[] { ',' }).Trim();
                foreach (string s2 in s.Split(','))
                {
                    cc = strCokAddCol(s2, defaultDomain, cc);
                }
                return cc;
            }
            else //同站点与同路径,不同.Name与.Value【isGood代码】
            {
                return strCokAddCol(s, defaultDomain, cc);
            }
        }
        //添加到CookieCollection集合部分
        private static CookieCollection strCokAddCol(string s, string defaultDomain, CookieCollection cc)
        {
            try
            {
                s.TrimEnd(new char[] { ';' }).Trim();
                System.Collections.Hashtable hs = new System.Collections.Hashtable();
                foreach (string s2 in s.Split(';'))
                {
                    string s3 = s2.Trim();
                    if (s3.IndexOf("=") > 0)
                    {
                        string[] s4 = s3.Split('=');
                        hs.Add(s4[0].Trim(), s4[1].Trim());
                    }
                }
                string defaultPath = "/";
                foreach (object Key in hs.Keys)
                {
                    if (Key.ToString().ToLower() == "path")
                    {
                        defaultPath = hs[Key].ToString();
                    }
                    else if (Key.ToString().ToLower() == "domain")
                    {
                        defaultDomain = hs[Key].ToString();
                    }
                }
                //【isGood代码】
                foreach (object Key in hs.Keys)
                {
                    if (!string.IsNullOrEmpty(Key.ToString()) && !string.IsNullOrEmpty(hs[Key].ToString()))
                    {
                        if (Key.ToString().ToLower() != "path" && Key.ToString().ToLower() != "domain")
                        {
                            Cookie c = new Cookie();
                            c.Name = Key.ToString();
                            c.Value = hs[Key].ToString();
                            c.Path = defaultPath;
                            c.Domain = defaultDomain;
                            cc.Add(c);
                        }
                    }
                }
            }
            catch { }
            return cc;
        }

        /// <summary>
        /// 替换字符【isGood代码】
        /// </summary>
        /// <param name="strSource">来源</param>
        /// <param name="strRegex">表达式</param>
        /// <param name="strReplace">取代</param>
        public static string replace(string strSource, string strRegex, string strReplace)
        {
            try
            {
                Regex r;
                r = new Regex(strRegex, RegexOptions.IgnoreCase | RegexOptions.Singleline);
                string s = r.Replace(strSource, strReplace);
                return s;
            }
            catch
            {
                return strSource;
            }
        }
    }
}