using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBook.Domain.Dto
{
    public class BookAuthorDto
    {
        public string BookName { get; set; }
        public List<string> BookAuthor { get; set; }
    }
}
