using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GrowStocks_Scrapper
{
    static class Program
    {

        static void Main()
        {
            Console.Title = "GrowStocks Scrapper";

            string[] line = File.ReadAllLines("item.txt", Encoding.UTF8);
     
            foreach (string itemname in line)
            {
                Thread.Sleep(1500); //rate limit bypass
                string repitemname = Regex.Replace(itemname, @"\s+", "+");
                string contents;
                try
                {
                    WebClient client = new WebClient();
                    /*WebProxy wp = new WebProxy("185.242.104.112", 8080); //rate limit bypass
                    client.Proxy = wp;*/
                    client.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36"); //cloudflare bypass
                    contents = client.DownloadString("https://growstocks.xyz/item/" + repitemname);
                    using (StringReader reader = new StringReader(contents))
                    {
                        string lines;
                        while ((lines = reader.ReadLine()) != null)
                        {
                            if (lines.Contains("<p>Price: <b>"))
                            {
                                lines = lines.Replace("<p>Price: <b>", "");
                                lines = lines.Replace("</b></p>", "");
                                lines = Regex.Replace(lines, @"\s+", "");
                                Console.WriteLine(itemname + " : " + lines);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                  //  Console.WriteLine(e.ToString());
                }

                
            }
            Console.WriteLine("Done.");
            Console.ReadKey();
        }

    }
}
