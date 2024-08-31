using System.ComponentModel.DataAnnotations;

namespace EndProject.Models
{
    public class EditNewsViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        public IFormFile? Image { get; set; }

        public string? ImagePath { get; set; }
    }
}
