using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CommonTrace.Common
{
    [TestClass]
    public class ExpiredInSpec
    {
        [TestMethod]
        public void ShouldExpired_FirstAsk_Should_True()
        {
            var expiredSmart = Create();
            var now = MockNow();
            expiredSmart.ShouldExpired(now).ShouldTrue();
            expiredSmart.ShouldExpired(now).ShouldFalse();
        }

        [TestMethod]
        public void ShouldExpired_InSpan_Should_False()
        {
            var expiredSmart = Create();
            var now = MockNow();
            expiredSmart.LastCheckAt = now.AddSeconds(-2);
            expiredSmart.ShouldExpired(now).ShouldFalse();
        }

        [TestMethod]
        public void ShouldExpired_EqualSpan_Should_True()
        {
            var expiredSmart = Create();
            var now = MockNow();
            expiredSmart.LastCheckAt = now.AddSeconds(-3);
            expiredSmart.ShouldExpired(now).ShouldTrue();
            expiredSmart.ShouldExpired(now).ShouldFalse();
        }

        [TestMethod]
        public void ShouldExpired_AfterSpan_Should_True()
        {
            var expiredSmart = Create();
            var now = MockNow();
            expiredSmart.LastCheckAt = now.AddSeconds(-4);
            expiredSmart.ShouldExpired(now).ShouldTrue();
            expiredSmart.ShouldExpired(now).ShouldFalse();
        }

        private Func<DateTime> MockNow { get; set; } = () => new DateTime(2019, 1, 1);
        private ExpiredIn Create()
        {
            return new ExpiredIn() { ExpiredInterval = TimeSpan.FromSeconds(3)};
        }
    }
}
