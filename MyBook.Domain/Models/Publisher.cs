using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBook.Domain.Models
{
    public class Publisher
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string ContactInformation { get; set; }
        public string Website { get; set; }
        public DateTime FoundingYear { get; set; }
        public string CEO { get; set; }
        public string Country { get; set; }
        public decimal Revenue { get; set; }

        // Navigation property: List of books published by this publisher
        //public Book Book { get; set; }
        //public List<Book> Books { get; set; }
    }
}
