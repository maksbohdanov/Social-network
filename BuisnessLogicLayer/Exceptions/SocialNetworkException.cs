using System.Runtime.Serialization;

namespace BuisnessLogicLayer.Exceptions
{
    [Serializable]
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

        protected SocialNetworkException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
