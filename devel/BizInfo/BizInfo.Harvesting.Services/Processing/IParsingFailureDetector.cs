using System;
using BizInfo.Model.Interfaces;
using HtmlAgilityPack;

namespace BizInfo.Harvesting.Services.Processing
{
    public interface IParsingFailureDetector
    {
        ParsingFailureReason DetectParsingFailure(IWebSource webSource, IBizInfoStorage storage, HtmlDocument htmlDocument, Exception exception);
    }
}