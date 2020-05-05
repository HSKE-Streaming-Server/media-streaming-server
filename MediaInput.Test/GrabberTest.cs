using System;
using System.Collections.Generic;
using System.Text;
using MediaInput;
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
            _grabber = Grabber.GetSingleton();
        }

        [Test]
        public void GetAvailableCategoriesCorrect()
        {
            var categories = _grabber.GetAvailableCategories();
            var enumerable = categories as string[] ?? categories.ToArray();
            //Exactly three distinc categories
            Assert.AreEqual(3, enumerable.Length);
            //All three distinct categories present
            Assert.IsTrue(enumerable.Contains("ard"));
            Assert.IsTrue(enumerable.Contains("zdf"));
            Assert.IsTrue(enumerable.Contains("arte"));
        }

        [Test]
        public void GetAvailableContent()
        {
            var fullContent = _grabber.GetAvailableContentInformation();

            //Content for all three categories present
            Assert.AreEqual(3, fullContent.Count);

            //Check content count per category
            var ardContent = fullContent["ard"];
            var zdfContent = fullContent["zdf"];
            var arteContent = fullContent["arte"];
            Assert.AreEqual(2, ardContent.Count());
            Assert.AreEqual(2, zdfContent.Count());
            Assert.AreEqual(1, arteContent.Count());
        }

        [Test]
        public void GetAvailableContentWithCategoriesCorrect()
        {
            //Check content count per category
            var zdfContent = _grabber.GetAvailableContentInformation("zdf");
            var ardContent = _grabber.GetAvailableContentInformation("ard");
            var arteContent = _grabber.GetAvailableContentInformation("arte");
            Assert.AreEqual(2, zdfContent.Count());
            Assert.AreEqual(2, ardContent.Count());
            Assert.AreEqual(1, arteContent.Count());
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
        }
    }
}