using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBook.Domain.Dto
{
    public class PublisherWithBooksAndAuthorsDto
    {
        public string Name { get; set; }    
        public List<BookAuthorDto>   BookAuthors { get; set; }
    }
}
