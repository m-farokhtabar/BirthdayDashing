using BirthdayDashing.Application.Dtos.Comment.Input;
using System;

namespace BirthdayDashing.Application.Dtos.Comment.Output
{
    public class CommentDto
    {
        public Guid Id { get; set; }
        public Guid? UserId { get; set; }
        public Guid DashingId { get; set; }
        public Guid? ParentId { get; set; }
        public string Content { get; set; }
        public CommentTypeDto Type { get; set; }
        public string MediaUrl { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
