using System.Runtime.Serialization;

namespace FileToEmailLinker.Models.Exceptions
{
    [Serializable]
    internal class MailingPlanNotFoundException : Exception
    {
        private int schedulationId;

        public MailingPlanNotFoundException()
        {
        }

        public MailingPlanNotFoundException(int schedulationId): this("Non è stato trovato il MailingPlan corrispondente alla schedulazione: " + schedulationId.ToString())
        {
            this.schedulationId = schedulationId;
        }

        public MailingPlanNotFoundException(string? message) : base(message)
        {
        }

        public MailingPlanNotFoundException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected MailingPlanNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}