using BizInfo.Model.Interfaces;

namespace BizInfo.Model.Entities
{
    public enum ProcessingResult
    {
        OK = 0,
        Failed = -1
    }

    public partial class WebSource : IWebSource
    {
        public ProcessingResult ResultStatus
        {
            get { return (ProcessingResult) this.ProcessingResult; }
            set { this.ProcessingResult = (int) value; }
        }

    }
}