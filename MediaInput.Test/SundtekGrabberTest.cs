using MediaInput;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MediaInput.Test
{
    public class SundtekGrabberTest
    {
        private IGrabber _grabber;

        [SetUp]
        public void Setup()
        {
            _grabber = SundtekGrabber.GetSingleton();
        }

        [Test]
        public void GetAvailableCategoriesCorrect()
        {
            var categories = _grabber.GetAvailableCategories();
            var enumerable = categories as string[] ?? categories.ToArray();
            Assert.AreEqual(2, enumerable.Length);
            Assert.IsTrue(enumerable.Contains("television"));
            Assert.IsTrue(enumerable.Contains("radio"));
        }

        [Test]
        public void GetAvailableContent()
        {
            var fullContent = _grabber.GetAvailableContentInformation();
            
            Assert.AreEqual(2, fullContent.Count);
            var radioContent = fullContent["radio"];
            var tvContent = fullContent["television"];
            Assert.AreEqual(78, radioContent.Count());
            Assert.AreEqual(96, tvContent.Count());
        }

        [Test]
        public void GetAvailableContentWithCategoriesCorrect()
        {
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
            //Test a radio and a tv channel
            Assert.AreEqual(new Tuple<Uri, bool>(new Uri("http://localhost:22000/stream/MDR KLASSIK"), true),
                _grabber.GetMediaStream("00082C5B411111A07A362AEEDE0F546319FFFF49"));
            Assert.AreEqual(new Tuple<Uri, bool>(new Uri("http://localhost:22000/stream/HSE24 EXTRA"), true),
                _grabber.GetMediaStream("02F8E9F27329EC332E35FC532467AB4D8585866E"));
        }
    }
}