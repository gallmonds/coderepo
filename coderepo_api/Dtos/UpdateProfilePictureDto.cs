using System.ComponentModel.DataAnnotations;

namespace coderepo_api.Dtos
{
    public class UpdateProfilePictureDto
    {
        [Required]
        public IFormFile File { get; set; } = null!;
    }
}
