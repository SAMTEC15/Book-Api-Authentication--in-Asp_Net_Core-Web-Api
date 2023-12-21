using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBook.Domain
{
    public class Response
    {
        public bool Succeeded { get; set; }
        public string Message { get; set; }      
        public int StatusCode { get; set; }
        public override string ToString() => JsonConvert.SerializeObject(this);
       
    }
}
