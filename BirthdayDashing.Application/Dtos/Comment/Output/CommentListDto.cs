using System;
using System.Collections.Generic;

namespace BirthdayDashing.Application.Dtos.Comment.Output
{
    public class CommentListDto
    {
        public Guid Id { get; set; }
        public Guid? UserId { get; set; }
        public Guid? ParentId { get; set; }
        public string Content { get; set; }
        public string MediaUrl { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UserFullName { get; set; }
        public string UserImageUrl { get; set; }
        public List<CommentListDto> Children { get; set; }
    }
}
