using NUnit.Framework;
using Realms.KeyValueStorage;
using System;
using System.Collections.Generic;

namespace Realm.KeyValueStorage.Tests
{
    [TestFixture]
    public class KeyValueItem_Tests
    {
        [Test]
        public void CreatingAnObjectWithNoArgumentConstructor_PopulatesProperties()
        {
            var expiration = DateTimeOffset.Now;

            var item = new KeyValueItem
            {
                Key = "Username",
                Value = "Smith",
                ExpiresOn = expiration
            };

            Assert.AreEqual("Username", item.Key);
            Assert.AreEqual("Smith", item.Value);
            Assert.AreEqual(expiration, item.ExpiresOn);
        }

        [Test]
        public void CreatingAnObjectWithArgumentConstructor_PopulatesProperties()
        {
            var expiration = DateTimeOffset.Now;
            var item = new KeyValueItem("Username", "KewlSmith", expiration);

            Assert.AreEqual("Username", item.Key);
            Assert.AreEqual("KewlSmith", item.Value);
            Assert.AreEqual(expiration, item.ExpiresOn);
        }

        [Test]
        public void CompareTo_ReturnsCorrectRelation()
        {
            var items = new List<KeyValueItem>()
            {
                new KeyValueItem("Username", "KewlSmith"),
                new KeyValueItem("Password", "123456"),
                new KeyValueItem("Last Name", "Smith"),
                new KeyValueItem("First Name", "John"),
                new KeyValueItem("Pin Code", 1234)
            };

            items.Sort();

            Assert.AreEqual("First Name", items[0].Key);
            Assert.AreEqual("Last Name", items[1].Key);
            Assert.AreEqual("Password", items[2].Key);
            Assert.AreEqual("Pin Code", items[3].Key);
            Assert.AreEqual("Username", items[4].Key);
        }
    }
}
