using System.ComponentModel.DataAnnotations;

namespace EndProject.Models
{
    public class CreateNewsViewModel
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        public IFormFile? Image { get; set; }
    }
}
