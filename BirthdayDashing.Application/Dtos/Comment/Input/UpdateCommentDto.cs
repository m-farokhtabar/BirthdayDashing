using System.ComponentModel.DataAnnotations;

namespace BirthdayDashing.Application.Dtos.Comment.Input
{
    public class UpdateCommentDto
    {
        [StringLength(1024)]
        public string Content { get; set; }
        [StringLength(1024)]
        public string MediaUrl { get; set; }
    }
}
