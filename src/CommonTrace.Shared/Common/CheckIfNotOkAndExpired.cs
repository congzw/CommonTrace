using System;

namespace CommonTrace.Common
{
    public class CheckIfNotOkAndExpired
    {
        public CheckIfNotOkAndExpired()
        {
            ExpiredIn.Create(TimeSpan.FromSeconds(3));
        }

        public virtual bool CheckIfNecessary(DateTime now, Func<bool> checkStatusOkFunc)
        {
            //check only necessary
            if (StatusOk)
            {
                return true;
            }

            var shouldExpired = ExpiredIn.ShouldExpired(now);
            if (!shouldExpired)
            {
                return StatusOk;
            }

            StatusOk = checkStatusOkFunc();

            return StatusOk;
        }

        public bool StatusOk { get; set; }

        public ExpiredIn ExpiredIn { get; set; }

        public static CheckIfNotOkAndExpired Create(TimeSpan? timeSpan = null)
        {
            return new CheckIfNotOkAndExpired() { ExpiredIn = ExpiredIn.Create(timeSpan ?? TimeSpan.FromSeconds(3)) };
        }
    }
}