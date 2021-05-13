using System;
using System.ComponentModel.DataAnnotations;

namespace BirthdayDashing.Application.Dtos.Comment.Input
{
    public class AddCommentDto
    {
        public Guid? UserId { get; set; }
        public Guid DashingId { get; set; }
        public Guid? ParentId { get; set; }
        [StringLength(1024)]
        public string Content { get; set; }
        [Required]
        [EnumDataType(typeof(CommentTypeDto))]
        public CommentTypeDto Type { get; set; }
        [StringLength(1024)]
        public string MediaUrl { get; set; }                        
    }
}
