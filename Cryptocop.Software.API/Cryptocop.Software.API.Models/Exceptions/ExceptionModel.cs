using Newtonsoft.Json;

namespace Cryptocop.Software.API.Models.Exceptions
{
    public class ExceptionModel
    {
        public int StatusCode { get; set; }
        public string ExceptionMessage { get; set; }
        public override string ToString() => JsonConvert.SerializeObject(this);
    }
}