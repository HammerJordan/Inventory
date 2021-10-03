using System;

namespace Infrastructure.Exceptions
{
    public class NotFoundInDbException : Exception
    {
        public NotFoundInDbException(string message) : base(message)
        {
        }
        
        
    }
}