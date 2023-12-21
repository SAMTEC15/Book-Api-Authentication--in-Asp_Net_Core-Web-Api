using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBook.Domain.Dto
{
    public class AuthorWithBooksDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<string> BookTitles { get; set; }
    }
}
