/*using System;
using NUnit.Framework;
using System.Linq;

namespace MediaInput.Test
{
    class GrabberTest
    {
        private IGrabber _grabber;

        [SetUp]
        public void Setup()
        {
            throw new NotImplementedException();
        }

        [Test]
        public void GetAvailableCategoriesCorrect()
        {
            var categories = _grabber.GetAvailableCategories();
            var enumerable = categories as string[] ?? categories.ToArray();
            //Exactly 5 distinct categories
            Assert.AreEqual(5, enumerable.Length);
            //video/stream
            Assert.IsTrue(enumerable.Contains("ard"));
            Assert.IsTrue(enumerable.Contains("zdf"));
            Assert.IsTrue(enumerable.Contains("arte"));
            //sundtek
            Assert.IsTrue(enumerable.Contains("television"));
            Assert.IsTrue(enumerable.Contains("radio"));
        }

        [Test]
        public void GetAvailableContent()
        {
            var fullContent = _grabber.GetAvailableContentInformation();

            //Content for all 5 categories present
            Assert.AreEqual(5, fullContent.Count);

            //Check content count per category
            //video/stream
            var ardContent = fullContent["ard"];
            var zdfContent = fullContent["zdf"];
            var arteContent = fullContent["arte"];
            Assert.AreEqual(2, ardContent.Count());
            Assert.AreEqual(2, zdfContent.Count());
            Assert.AreEqual(1, arteContent.Count());
            //sundtek
            var radioContent = fullContent["radio"];
            var tvContent = fullContent["television"];
            Assert.AreEqual(78, radioContent.Count());
            Assert.AreEqual(96, tvContent.Count());
        }

        [Test]
        public void GetAvailableContentWithCategoriesCorrect()
        {
            //Check content count per category
            //stream/video
            var zdfContent = _grabber.GetAvailableContentInformation("zdf");
            var ardContent = _grabber.GetAvailableContentInformation("ard");
            var arteContent = _grabber.GetAvailableContentInformation("arte");
            Assert.AreEqual(2, zdfContent.Count());
            Assert.AreEqual(2, ardContent.Count());
            Assert.AreEqual(1, arteContent.Count());
            //sundtek
            var radioContent = _grabber.GetAvailableContentInformation("radio");
            var tvContent = _grabber.GetAvailableContentInformation("television");
            Assert.AreEqual(78, radioContent.Count());
            Assert.AreEqual(96, tvContent.Count());
        }

        [Test]
        public void GetMediaStreamCorrect()
        {
            //Test that exception for invalid id is thrown correctly
            Assert.Throws<Exception>(delegate { _grabber.GetMediaStream("ThisContentIdIsInvalid"); },
                "Content with specified contentId does not exist in database");
            Assert.Throws<Exception>(delegate { _grabber.GetMediaStream(""); },
                "Content with specified contentId does not exist in database");
            Assert.Throws<Exception>(delegate { _grabber.GetMediaStream(null); },
                "Content with specified contentId does not exist in database");
            //Test Get Video-Uri from ID
            Assert.AreEqual(new Tuple<Uri, bool>(new Uri("https://zdfvodnone-vh.akamaihd.net/i/meta-files/zdf/smil/m3u8/300/20/03/200328_1345_sendung_sof/3/200328_1345_sendung_sof.smil/master.m3u8"), false),
                _grabber.GetMediaStream("4ee8bcf8006a012bc48a8c6f33c9fad60ce08191"));
            Assert.AreEqual(new Tuple<Uri, bool>(new Uri("http://localhost:22000/stream/MDR KLASSIK"), true),
                _grabber.GetMediaStream("00082C5B411111A07A362AEEDE0F546319FFFF49"));
            Assert.AreEqual(new Tuple<Uri, bool>(new Uri("http://localhost:22000/stream/HSE24 EXTRA"), true),
                _grabber.GetMediaStream("02F8E9F27329EC332E35FC532467AB4D8585866E"));
        }
    }
}*/