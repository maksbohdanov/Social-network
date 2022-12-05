using DataAccessLayer.Entities;
using System.Diagnostics.CodeAnalysis;

namespace SocialNetwork.Tests.Helpers
{
    internal class UserEqualityComparer : IEqualityComparer<User>
    {
        public bool Equals([AllowNull] User x, [AllowNull] User y)
        {
            if (x == null && y == null)
                return true;
            if (x == null || y == null)
                return false;

            return x.Id == y.Id &&
                x.FirstName == y.FirstName &&
                x.LastName == y.LastName &&
                x.City == y.City &&
                x.BirthDate == y.BirthDate &&
                x.Email == y.Email;
        }

        public int GetHashCode([DisallowNull] User obj)
        {
            return obj.GetHashCode();
        }
    }

    internal class MessageEqualityComparer : IEqualityComparer<Message>
    {
        public bool Equals([AllowNull] Message x, [AllowNull] Message y)
        {
            if (x == null && y == null)
                return true;
            if (x == null || y == null)
                return false;

            return x.Id == y.Id &&
                x.Text == y.Text &&
                x.AuthorId == y.AuthorId &&
                x.ChatId == y.ChatId ;
        }

        public int GetHashCode([DisallowNull] Message obj)
        {
            return obj.GetHashCode();
        }
    }

    internal class ChatEqualityComparer : IEqualityComparer<Chat>
    {
        public bool Equals([AllowNull] Chat x, [AllowNull] Chat y)
        {
            if (x == null && y == null)
                return true;
            if (x == null || y == null)
                return false;

            return x.Id == y.Id &&
                x.Users.Count == y.Users.Count &&
                x.Messages.Count == y.Messages.Count;
        }

        public int GetHashCode([DisallowNull] Chat obj)
        {
            return obj.GetHashCode();
        }
    }

}
