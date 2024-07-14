using System.Net;
using Dkeshri.SystemDesign.LowLevel.Interfaces;

namespace Dkeshri.SystemDesign.LowLevel.Basic.Async
{
    public class WebsiteDataModel
    {
        public string Url { get; set; }
        public string Data { get; set; }

    }
    public class AsyncEx : IExecute
    {
        private List<string> ListOfWebsite()
        {
            List<string> websites = new List<string>();
            websites.Add("https://www.yahoo.com/");
            websites.Add("https://www.google.com/");
            websites.Add("https://www.microsoft.com/en-in/");
            websites.Add("https://edition.cnn.com/");
            websites.Add("https://www.codeproject.com/");
            websites.Add("https://stackoverflow.com/");
            return websites;
        }
        private void ConsoleWebsiteInfo(WebsiteDataModel data)
        {
            System.Console.WriteLine($"{data.Url} downloaded: {data.Data.Length} charecters long.");
        }
        private WebsiteDataModel DownloadWebsite(string url)
        {
            WebsiteDataModel websiteDataModel = new WebsiteDataModel();
            WebClient client = new WebClient();
            websiteDataModel.Url = url;
            websiteDataModel.Data = client.DownloadString(url);
            return websiteDataModel;
        }
        private async Task<WebsiteDataModel> DownloadWebsiteAsync(string url)
        {
            WebsiteDataModel websiteDataModel = new WebsiteDataModel();
            WebClient client = new WebClient();
            websiteDataModel.Url = url;
            websiteDataModel.Data = await client.DownloadStringTaskAsync(url);
            return websiteDataModel;
        }
        private void runDownload()
        {
            List<string> websites = ListOfWebsite();
            foreach (string url in websites)
            {
                WebsiteDataModel results = DownloadWebsite(url);
                ConsoleWebsiteInfo(results);
            }
        }
        private async Task runDownloadAsync()
        {
            List<string> websites = ListOfWebsite();
            foreach (string url in websites)
            {
                WebsiteDataModel results = await Task.Run(() => DownloadWebsite(url));
                ConsoleWebsiteInfo(results);
            }
        }
        private async Task runDownloadParallelAsync()
        {
            List<string> websites = ListOfWebsite();
            List<Task<WebsiteDataModel>> tasks = new List<Task<WebsiteDataModel>>();
            foreach (string url in websites)
            {
                //tasks.Add(Task.Run(() => DownloadWebsite(url)));
                tasks.Add(Task.Run(() => DownloadWebsiteAsync(url)));
            }
            WebsiteDataModel[] Results = await Task.WhenAll(tasks);
            foreach (WebsiteDataModel websiteDataModel in Results)
            {
                ConsoleWebsiteInfo(websiteDataModel);
            }

        }
        public async Task RunAsync()
        {
            int opt;
            do
            {
                System.Console.WriteLine("1 For syncronous Execution");
                System.Console.WriteLine("2 For Asyncronous Execution");
                System.Console.WriteLine("3 For Asyncronous Parallel Execution");
                System.Console.WriteLine("0 For Exit");
                Console.Write("Enter the option: ");
                opt = Convert.ToInt32(Console.ReadLine());
                var watch = System.Diagnostics.Stopwatch.StartNew();
                long ElapsTime;
                switch (opt)
                {
                    case 0:
                        break;
                    case 1: // this run syncronously and also block the UI.
                        runDownload();
                        watch.Stop();
                        ElapsTime = watch.ElapsedMilliseconds;
                        Console.WriteLine($"Total Execution Time: {ElapsTime} in msec.");
                        break;
                    case 2: // Asyncronous Execution algorithms. this will not block the UI, but run in almost 
                            // at same time as case 1.
                        await runDownloadAsync();
                        watch.Stop();
                        ElapsTime = watch.ElapsedMilliseconds;
                        Console.WriteLine($"Total Execution Time: {ElapsTime} in msec.");
                        break;
                    case 3: // Asyncronous Execution algorithms. this will not block the UI,
                            // and run parallel too. this will execute faster the above case.
                        await runDownloadParallelAsync();
                        watch.Stop();
                        ElapsTime = watch.ElapsedMilliseconds;
                        Console.WriteLine($"Total Execution Time: {ElapsTime} in msec.");
                        break;
                    default:
                        Console.Write("Please enter correct option");
                        break;
                }
            } while (opt != 0);
        }
        public void run()
        {
            RunAsync().Wait();
        }
    }
}