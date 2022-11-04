using System;

namespace Cryptocop.Software.API.Models.Exceptions
{
    public class IdentityException : Exception
    {
        public IdentityException() : base("The identity of user was not found") {}
        public IdentityException(string message) : base(message) {}
        public IdentityException(string message, Exception inner) : base(message, inner) {}
    }
}