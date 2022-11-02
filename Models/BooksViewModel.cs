using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2.Models
{
    public class BooksViewModel
    {
        [Key]
        public int BookID { get; set; }
        [Required]
        public string BookTitle { get; set; }
        [Required]
        public string BookAutor { get; set; }
        [Range(1,int.MaxValue,ErrorMessage ="Should be greater than or Equal 1")]
        public int BookPrice { get; set; }
    }
}
