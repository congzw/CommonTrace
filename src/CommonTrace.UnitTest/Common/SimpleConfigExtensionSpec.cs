using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CommonTrace.Common
{
    [TestClass]
    public class SimpleConfigExtensionSpec
    {
        [TestMethod]
        public void AddOrUpdateModel_NotExist_Should_Added()
        {
            var simpleConfig = CreateSimpleConfig();
            var foo = new Foo() { Bar = "blah" };
            simpleConfig.AddOrUpdateModel(foo);
            var theValue = simpleConfig.TryGetModel<Foo>(null);
            theValue.ShouldSame(foo);
        }

        [TestMethod]
        public void AddOrUpdateModel_Exist_Should_Updated()
        {
            var simpleConfig = CreateSimpleConfig();
            var foo = new Foo() { Bar = "blah" };
            simpleConfig.AddOrUpdateModel(foo);
            var foo2 = new Foo() { Bar = "blah2" };
            simpleConfig.AddOrUpdateModel(foo2);
            var theValue = simpleConfig.TryGetModel<Foo>(null);
            theValue.ShouldSame(foo2);
        }

        [TestMethod]
        public void TryGetModel_NotExist_Should_Return_Default()
        {
            var simpleConfig = CreateSimpleConfig();
            var theValue = simpleConfig.TryGetModel<Foo>(null);
            theValue.ShouldNull();
        }

        [TestMethod]
        public void TryGetModel_Exist_Should_Return_It()
        {
            var simpleConfig = CreateSimpleConfig();
            var foo = new Foo() { Bar = "blah" };
            simpleConfig.AddOrUpdateModel(foo);
            var theValue = simpleConfig.TryGetModel<Foo>(null);
            theValue.ShouldSame(foo);
        }
        private ISimpleConfig CreateSimpleConfig()
        {
            return new SimpleConfig();
        }
    }
}
