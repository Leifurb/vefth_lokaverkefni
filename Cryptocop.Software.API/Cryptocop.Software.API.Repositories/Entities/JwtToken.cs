using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cryptocop.Software.API.Repositories.Entities
{
    public class JwtToken
    {
        public int Id { get; set;}
        public bool Blacklisted { get; set;}
    }
}