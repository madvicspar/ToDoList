using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ToDoList.Models
{
    public class ToDoItem
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [Required]
        public string Title { get; set; }
        public bool IsComplete { get; set; }
        [ForeignKey("CategoryId")]
        public long CategoryId { get; set; }
        public virtual ItemCategory Category { get; set; }
        //[ForeignKey("User")]
        //public string UserId { get; set; }
        //public virtual IdentityUser User { get; set; }
    }
}
