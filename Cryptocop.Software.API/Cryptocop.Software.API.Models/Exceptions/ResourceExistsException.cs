using System;


namespace Cryptocop.Software.API.Models.Exceptions
{
    public class ResourceExistsException : Exception
    {
         public ResourceExistsException() : base("Resource already exists") {}
        public ResourceExistsException(string message) : base(message) {}
        public ResourceExistsException(string message, Exception inner) : base(message, inner) {}
    }
}