namespace API.Exceptions
{
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException()
        {
        }

        public UserNotFoundException(string message) : base(message)
        {
        }

        public UserNotFoundException(string message, Exception inner) : base(message, inner)
        {
        }
    }

    public class InvalidUserPasswordException : Exception
    {
        public InvalidUserPasswordException()
        {
        }

        public InvalidUserPasswordException(string message) : base(message)
        {
        }

        public InvalidUserPasswordException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}