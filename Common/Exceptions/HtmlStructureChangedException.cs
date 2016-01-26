using System;

namespace CloudDaemon.Common.Exceptions
{
    public class HtmlStructureChangedException : Exception
    {

        public const string BaseMessage = "HTML struture is no longer valid for {0}\n{1}";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url">Url that failed to return the expected structure</param>
        /// <param name="expectedStructure">Indications about the expected structure of the document</param>
        public HtmlStructureChangedException(string url, string expectedStructure)
            : base (String.Format(BaseMessage, url, expectedStructure))
        {
        }
    }
}
