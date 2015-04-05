using System;
using BizInfo.Harvesting.Services.Core;
using Perenis.Core.Interfaces;

namespace BizInfo.App.Services.Logging
{
    /// <summary>
    /// Logs changes in <see cref="Daemon.Status"/>.
    /// </summary>
    public class DaemonStatusLogger : IAttachable<Daemon>
    {
        public string DaemonName { get; set; }
        
        public void Attach(Daemon attachTarget)
        {
            attachTarget.StatusChanged += OnDaemonStatusChanged;
        }

        public void Detach(Daemon attachTarget)
        {
            attachTarget.StatusChanged -= OnDaemonStatusChanged;
        }

        private void OnDaemonStatusChanged(object sender, DaemonStatusChangedEventArgs e)
        {
            this.LogInfo(string.Format("Daemon {0} status changed {1}=>{2}.", DaemonName, e.OldStatus, e.NewStatus));
        }
    }
}