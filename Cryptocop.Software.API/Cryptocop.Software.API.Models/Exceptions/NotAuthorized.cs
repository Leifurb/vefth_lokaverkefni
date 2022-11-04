using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cryptocop.Software.API.Models.Exceptions
{
    public class NotAuthorized : Exception
    {
        public NotAuthorized() : base("User is not authorized") {}
        public NotAuthorized(string message) : base(message) {}
        public NotAuthorized(string message, Exception inner) : base(message, inner) {}
    }
}