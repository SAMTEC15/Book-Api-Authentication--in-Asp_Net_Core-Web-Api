using MyBook.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBook.Domain.Dto
{
    public class AuthorAddDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        //public List<BookAuthor> BookAuthors { get; set; }
    }
}
