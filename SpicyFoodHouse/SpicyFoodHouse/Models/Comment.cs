using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace SpicyFoodHouse.Models
{
    public class Comment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        [DisplayName("Your Comment")]
        [StringLength(1500, MinimumLength = 5, ErrorMessage = "minimum len 5 and maximum len 1500 char")]
        public string CommentText { get; set; }

        
        public DateTime CommentTime  { get; set; }
    }
}
