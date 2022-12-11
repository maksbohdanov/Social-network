namespace BuisnessLogicLayer.Exceptions
{
    public class SocialNetworkException: Exception
    {
        public SocialNetworkException() 
        {
        }

        public SocialNetworkException(string message) : base(message)
        {
        }

        public SocialNetworkException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
