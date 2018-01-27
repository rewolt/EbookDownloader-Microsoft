using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.IO;
using System.Linq;

namespace ebookDownloader
{
    class NetDownloader
    {
        private string _content;
        private HttpClient client;

        public NetDownloader()
        {
            client = new HttpClient();
            client.Timeout = new TimeSpan(1, 0, 0);
        }

        public async Task<string> GetContent (Uri uri)
        {
            var cl = new HttpClient();
            var response = await cl.GetAsync(uri);
            var content = string.Empty;

            var cos = response.Content.Headers.ContentType;
            if (response.Content.Headers.ContentType.MediaType == "text/plain")
            {
                content = await response.Content.ReadAsStringAsync();
            }
            _content = content;
            return content;
        }

        public void HttpGetFiles(Uri url, DirectoryInfo directory)
        {
            try
            {
                using (HttpResponseMessage response = client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead).Result)
                using (Stream streamToReadFrom = response.Content.ReadAsStreamAsync().Result)
                {
                    var bookName = response.RequestMessage.RequestUri.Segments.Last();
                    var realBookFileName = new FileInfo(System.Web.HttpUtility.UrlDecode(bookName));
                    Console.Write(realBookFileName.Name + "\t");
                    //if (!realBookFileName.Extension.Equals(".doc", StringComparison.CurrentCultureIgnoreCase) &&
                    //    !realBookFileName.Extension.Equals(".docx", StringComparison.CurrentCultureIgnoreCase))
                    //{
                    //    Console.WriteLine("[Missed]");
                    //    return;
                    //}


                    string fileToWriteTo = directory.FullName + "\\" + realBookFileName.Name;

                    try
                    {
                        using (Stream streamToWriteTo = File.Open(fileToWriteTo, FileMode.Create))
                        {
                            streamToReadFrom.CopyTo(streamToWriteTo);
                        }
                        Console.WriteLine("[OK]");
                    }
                    catch (Exception ex)
                    {
                        LogEx(ex);
                    }
                }
            }
            catch (Exception ex)
            {
                LogEx(ex);
            }

        }

        private static void LogEx(Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"ERROR: {ex.Message}");
            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }
}
