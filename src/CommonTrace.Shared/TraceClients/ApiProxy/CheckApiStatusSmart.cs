using System;
using CommonTrace.Common;

namespace CommonTrace.TraceClients.ApiProxy
{
    public class CheckStatusSmart
    {
        public CheckStatusSmart()
        {
            CheckInterval = TimeSpan.FromSeconds(3);
            GetDateNow = DateHelper.Instance.GetDateNow;
        }

        public bool CheckIfNecessary(Func<bool> checkIsOkFunc)
        {
            //check only necessary
            if (StatusOk)
            {
                return true;
            }

            var now = GetDateNow();
            if (_lastCheckApiStatusDate == null || now - _lastCheckApiStatusDate <= CheckInterval)
            {
                return StatusOk;
            }

            var isOk = checkIsOkFunc();
            SetStatusOk(isOk, now);
            return StatusOk;
        }

        public bool StatusOk { get; set; }

        public TimeSpan CheckInterval { get; set; }

        public Func<DateTime> GetDateNow { get; set; }

        public void SetStatusOk(bool apiStatusOk, DateTime checkDate)
        {
            StatusOk = apiStatusOk;
            _lastCheckApiStatusDate = checkDate;
        }

        private DateTime? _lastCheckApiStatusDate = null;
    }
}