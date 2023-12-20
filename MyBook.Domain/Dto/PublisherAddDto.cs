using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBook.Domain.Dto
{
    public class PublisherAddDto
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string ContactInformation { get; set; }
        public string Website { get; set; }
        public DateTime FoundingYear { get; set; }
        public string CEO { get; set; }
        public string Country { get; set; }
        public decimal Revenue { get; set; }
    }
}
