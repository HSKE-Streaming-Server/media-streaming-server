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
            var enumerable = fullContent as IEnumerable<ContentInformation>[] ?? fullContent.ToArray();
            Assert.AreEqual(2, enumerable.Length);
            var radioContent = enumerable.ElementAt(0);
            var tvContent = enumerable.ElementAt(1);
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
    }
}