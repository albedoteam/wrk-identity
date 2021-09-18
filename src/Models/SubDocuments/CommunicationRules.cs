namespace Identity.Business.Models.SubDocuments
{
    public class CommunicationRules
    {
        public CommunicationRule OnUserCreated { get; set; }
        public CommunicationRule OnPasswordChangeRequested { get; set; }
        public CommunicationRule OnPasswordChanged { get; set; }
        public CommunicationRule OnUserActivated { get; set; }
        public CommunicationRule OnUserDeactivated { get; set; }
    }
}