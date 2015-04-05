using System;

namespace BizInfo.App.Services.Tools
{
    public class OutdatedValidityKeeper : IValidityKeeper
    {
        public DateTime? LastUpdate { get; private set; }
        public TimeSpan MaxAge { get; set; }
        public Action Update { get; set; }

        #region IValidityKeeper Members

        public bool IsValid
        {
            get { return LastUpdate.HasValue && LastUpdate.Value > DateTime.Now - MaxAge; }
        }

        public void Validate()
        {
            lock (this)
            {
                if (!IsValid)
                {
                    Update();
                    LastUpdate = DateTime.Now;
                }
            }
        }

        #endregion
    }
}