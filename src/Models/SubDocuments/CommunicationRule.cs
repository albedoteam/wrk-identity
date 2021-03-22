using System.Collections.Generic;

namespace Identity.Business.Models.SubDocuments
{
    public class CommunicationRule
    {
        public string TemplateId { get; set; }
        public Dictionary<string, string> DefaultContentParameters { get; set; }
    }
}