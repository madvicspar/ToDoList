using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ToDoList.Models
{
    public class ToDoItem
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [Required]
        [Display(Name = "Название")]
        public string Title { get; set; }
        [Display(Name = "Выполнено?")]
        public bool IsComplete { get; set; }
        [ForeignKey("CategoryId")]
        [Display(Name = "Категория")]
        public long CategoryId { get; set; }
        [Display(Name = "Категория")]
        public virtual ItemCategory? Category { get; set; }
    }
}
