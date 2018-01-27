using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ebookDownloader
{
    class Program
    {
        public static string _directoryName => "downloadedPDF2";
        static void Main(string[] args)
        {
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\tPDF downloader by Admin | ver: 1.0");
            Console.ResetColor();

            var link = "http://ligman.me/2sZVmcG";
            //link = "http://ligman.me/1IW1oab";

            var directory = new System.IO.DirectoryInfo(_directoryName);
            directory.Create();
            var files = directory.GetFiles();
            //foreach (var file in files)
            //    file.Delete();

            var ebooksLinksListRaw = string.Empty;
            var nd = new NetDownloader();

            try
            {
                Console.Write("\nDownloading urls...\t");
                ebooksLinksListRaw = nd.GetContent(new Uri(link)).Result;

                if (string.IsNullOrEmpty(ebooksLinksListRaw))
                    throw new Exception("Empty ebooks list!");

                Console.WriteLine("[OK]");
            }
            catch (Exception ex)
            {
                PrintError(ex);
                return;
            }



            var ebooksLinksList = ebooksLinksListRaw.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            var ebooksUrisList = new List<Uri>();
            foreach (var jakisLink in ebooksLinksList)
            {
                if(Uri.IsWellFormedUriString(jakisLink, UriKind.Absolute))
                    ebooksUrisList.Add(new Uri(jakisLink));
            }
            Console.WriteLine($"--> Downloaded list of {ebooksUrisList.Count} ebooks");




            Console.WriteLine("\nDownloading files started...");
            try
            {
                var tasks = new List<Task>();
                //Parallel.ForEach(ebooksUrisList, (ebook) => { NetDownloader.HttpGetFile(ebook, directory); });
                foreach(var ebook in ebooksUrisList)
                {
                    nd.HttpGetFiles(ebook, directory);
                }
                
            }
            catch (Exception ex)
            {
                PrintError(ex);
                return;
            }


            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\nDownoading finished ! ! !\nPress any to exit . . .");
            Console.ReadKey();
        }

        private static void PrintError(Exception ex)
        {
            Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("[ERROR] " + ex.Message);
            Console.ResetColor();
            Console.ReadKey();
        }
    }
}
