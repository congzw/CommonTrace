using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CommonTrace.Common
{
    [TestClass]
    public class CheckIfNotOkAndExpiredSpec
    {
        [TestMethod]
        public void CheckIfNecessary_StatusOk_Should_NotInvoke()
        {
            var checkIf = Create();
            var now = MockNow();
            checkIf.StatusOk = true;
            var mockCheckApi = MockCheckApi.Create(true);
            checkIf.CheckIfNecessary(now, mockCheckApi.CheckIsStatusOk);
            mockCheckApi.IsInvoked.ShouldFalse();
        }


        [TestMethod]
        public void CheckIfNecessary_StatusNotOk_NotExpired_Should_NotInvoke()
        {
            var checkIf = Create();
            var now = MockNow();
            checkIf.StatusOk = false;
            checkIf.ExpiredIn.LastCheckAt = now.AddSeconds(-2);
            var mockCheckApi = MockCheckApi.Create(true);
            checkIf.CheckIfNecessary(now, mockCheckApi.CheckIsStatusOk);
            mockCheckApi.IsInvoked.ShouldFalse();
        }
        
        [TestMethod]
        public void CheckIfNecessary_StatusNotOk_Expired_Should_Invoke()
        {
            var checkIf = Create();
            var now = MockNow();
            checkIf.StatusOk = false;
            checkIf.ExpiredIn.LastCheckAt = now.AddSeconds(-3);
            var mockCheckApi = MockCheckApi.Create(true);
            checkIf.CheckIfNecessary(now, mockCheckApi.CheckIsStatusOk);
            mockCheckApi.IsInvoked.ShouldTrue();
        }


        private Func<DateTime> MockNow { get; set; } = () => new DateTime(2019, 1, 1);
        private CheckIfNotOkAndExpired Create()
        {
            return CheckIfNotOkAndExpired.Create(TimeSpan.FromSeconds(3));
        }
    }

    public class MockCheckApi
    {
        public bool IsInvoked { get; set; }

        public bool MockIsStatusOk { get; set; }

        public bool CheckIsStatusOk()
        {
            IsInvoked = true;
            return MockIsStatusOk;
        }

        public static MockCheckApi Create(bool resultOk)
        {
            return new MockCheckApi()
            {
                MockIsStatusOk = resultOk
            };
        }
    }
}
