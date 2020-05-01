using MediaInput;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace MediaInput.Test
{
    class SundtekCrawlerTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test()
        {
            SundtekCrawler crawler = SundtekCrawler.GetSingleton(new System.Net.Http.HttpClient());

            crawler.StartCrawling(TimeSpan.FromMinutes(10));

            System.Threading.Thread.Sleep(600000);
        }
    }
}
