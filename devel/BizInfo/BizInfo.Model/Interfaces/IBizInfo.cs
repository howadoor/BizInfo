using System;
using System.Collections.Generic;
using BizInfo.Model.Entities;

namespace BizInfo.Model.Interfaces
{
    public interface IBizInfo
    {
        Int64? WebSourceId {get;}
        string Summary { get; set; }
        string Text { get; set; }
        Guid ContentHash { get; set; }
        DateTime CreationTime { get; set; }
        int? NativeCategoryId { get; set; }
        IEnumerable<string> PhotoUrlsList { get; set; }
        StructuredContent StructuredContentDocument { get; set; }
        float IsCompanyScore { get; set; }

        Int32? SourceTagId { get; set; }
        Int32? VerbKindTagId { get; set; }
        Int32? VerbTagId { get; set; }
        Int32? DomainTagId { get; set; }
    }
}