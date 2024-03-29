using System;

namespace Cryptocop.Software.API.Models.Exceptions
{
    public class ModelFormatException : Exception
    {
        public ModelFormatException() : base("Model is not preperly formatted.") {}
        public ModelFormatException(string message) : base(message) {}
        public ModelFormatException(string message, Exception inner) : base(message, inner) {}
    }
}