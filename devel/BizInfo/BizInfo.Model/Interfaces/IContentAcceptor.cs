using System;

namespace BizInfo.Model.Interfaces
{
    /// <summary>
    /// Accepts content requested by <see cref="IUrlDownloadRequest"/>
    /// </summary>
    public interface IContentAcceptor
    {
        /// <summary>
        /// Accept content as an array of bytes
        /// </summary>
        /// <param name="content"></param>
        /// <param name="loadTime"></param>
        void Accept(byte[] content, DateTime loadTime);
        
        /// <summary>
        /// Accept content as an array of bytes with length
        /// </summary>
        /// <param name="content"></param>
        /// <param name="contentLength"></param>
        /// <param name="loadTime"></param>
        void Accept(byte[] content, long contentLength, DateTime loadTime);
    }
}