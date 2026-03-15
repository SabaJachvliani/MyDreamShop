namespace Application.Common.Exeptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message = "Resource was not found.")
            : base(message)
        {
        }
    }
}
