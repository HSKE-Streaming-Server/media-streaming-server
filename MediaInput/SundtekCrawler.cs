using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace MediaInput
{
    public class SundtekCrawler
    {
        private const string SundtekAddress = "localhost";
        private const string SundtekPort = "22000";

        private readonly HttpClient _client;
        private Timer _crawlTimer;

        private SundtekCrawler _singleton;

        private SundtekCrawler(HttpClient client)
        {
            _client = client;
        }

        public SundtekCrawler GetSingleton(HttpClient client)
        {
            return _singleton ??= new SundtekCrawler(client);
        }
        
        /// <summary>
        /// Starts crawling task for this crawler with the specified interval.
        /// </summary>
        /// <param name="interval">The interval between crawls.</param>
        public void StartCrawling(TimeSpan interval)
        {
            _crawlTimer ??= new Timer(CrawlMethod, null, TimeSpan.Zero, TimeSpan.FromMinutes(30));
        }

        public void StopCrawling()
        {
            
        }

        private void CrawlMethod(Object stateInfo)
        {
            
            var radioUri = new Uri($"http://{SundtekAddress}:{SundtekAddress}/radio.m3u");
            var freeSdtvuri = new Uri($"http://{SundtekAddress}:{SundtekPort}/freesdtv.m3u");
            var freeHdtvuri = new Uri($"http://{SundtekAddress}:{SundtekPort}/freehdtv.m3u");
            var radioPlaylist = _client.GetStreamAsync(radioUri).Result;
            var freeSdtvPlaylist = _client.GetStreamAsync(freeSdtvuri).Result;
            var freeHdtvPlaylist = _client.GetStreamAsync(freeHdtvuri).Result;

                

            //TODO: What can we do to either make this guid permanent or use something different that is determined from the name and category of the channel?
            var radioList = M3UParser.ParsePlaylist(radioPlaylist).Select
                (item => new ContentInformation(Guid.Empty, item?.Item2, "radio", true, new Uri(item?.Item1)));
            
            var sdtvList = M3UParser.ParsePlaylist(freeSdtvPlaylist).Select
                (item => new ContentInformation(Guid.Empty, item?.Item2, "television", true, new Uri(item?.Item1)));
            
            var hdtvList = M3UParser.ParsePlaylist(freeHdtvPlaylist).Select
                (item => new ContentInformation(Guid.Empty, item?.Item2, "television", true, new Uri(item?.Item1)));
            
            var televisionList = sdtvList.Concat(hdtvList);

                
            //now write all of this bollocks into the database, but only if it doesn't exist yet
            //TODO: actual database writes instead of writing to a file?
            var radioFileWriter = new StreamWriter(File.OpenWrite("./radio.csv"));
        }
    }
}