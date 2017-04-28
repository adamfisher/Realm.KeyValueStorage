using System;
using FakeItEasy;
using NUnit.Framework;
using Realms.KeyValueStorage;

namespace Realm.KeyValueStorage.Tests
{
    using Realms;

    [TestFixture]
    public class KeyValueRealm_Tests
    {
        private Realm Realm;
        private KeyValueRealm KeyValueRealm;

        [SetUp]
        public void Setup()
        {
            Realm = A.Fake<Realm>();
            KeyValueRealm = new KeyValueRealm(Realm);
        }

        [TestCase("Non Existent Key", null, ExpectedResult = null)]
        [TestCase("Username", "KewlSmith", ExpectedResult = "KewlSmith")]
        [TestCase("Password", 1234, ExpectedResult = 1234)]
        [TestCase("Married", true, ExpectedResult = true)]
        public object GetKey(string key, object expectedValue)
        {
            var aCallToRealmRead = A.CallTo(() => Realm.Find<KeyValueItem>(A<string>.That.Matches(s => s.Equals(key))));
            aCallToRealmRead.Returns(new KeyValueItem(key, expectedValue));
            var value = KeyValueRealm.Get(key);
            aCallToRealmRead.MustHaveHappened();
            return value;
        }

        [Test]
        public void SetKey()
        {
            var aCallToRealmWrite = A.CallTo(() => Realm.Write(A<Action>._));

            KeyValueRealm.Set("Username", "KewlSmith");
            KeyValueRealm.Set("Password", 1234);
            KeyValueRealm.Set("Married", true);

            aCallToRealmWrite.MustHaveHappened(Repeated.Exactly.Times(3));
        }

        [Test]
        public void Remove()
        {
            var aCallToRealmRemove = A.CallTo(() => Realm.Remove(A<KeyValueItem>._));
            KeyValueRealm.Remove(new KeyValueItem("Username", "KewlSmith"));
            aCallToRealmRemove.MustHaveHappened();
        }
    }
}
