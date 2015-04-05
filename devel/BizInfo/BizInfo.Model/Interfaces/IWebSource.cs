using System;

namespace BizInfo.Model.Interfaces
{
    public interface IWebSource
    {
        Int64 Id { get; }
        string Url { get; }
        DateTime Scouted { get; }
        DateTime? Processed { get; set; }
        int ProcessingResult { get; set; }
    }
}