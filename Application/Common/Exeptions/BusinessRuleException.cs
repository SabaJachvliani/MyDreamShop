namespace Application.Common.Exeptions
{
    public class BusinessRuleException : Exception
    {
        public BusinessRuleException(string message)
            : base(message)
        {
        }
    }
}
