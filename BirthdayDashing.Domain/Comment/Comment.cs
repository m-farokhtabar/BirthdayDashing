using BirthdayDashing.Domain.SeedWork;
using Common.Exception;
using System;
using static Common.Exception.Messages;

namespace BirthdayDashing.Domain.Comment
{
    public class Comment : Entity
    {
        /// <summary>
        /// just for Mapping
        /// </summary>
        private Comment()
        {

        }

        public Comment(Guid? userId, Guid dashingId, Guid? parentId, string content, CommentType type, string mediaUrl, Guid createdById)
        {
            if (type == CommentType.Comment && userId == null)
                throw new ManualException(DATA_IS_NOT_FOUND.Replace("{0}", nameof(userId)), ExceptionType.NotFound, nameof(userId));

            UserId = userId;
            DashingId = dashingId;
            ParentId = parentId;
            Content = content;
            Type = type;
            MediaUrl = mediaUrl;
            LastEditById = null;
            LastEditDate = null;
            CreatedById = createdById;
            CreatedDate = DateTime.Now;
            Active = true;
        }
        public void Update(string content, string mediaUrl, Guid lastEditById)
        {
            Content = content;            
            MediaUrl = mediaUrl;
            LastEditById = lastEditById;
            LastEditDate = DateTime.Now;
        }
        public bool ToggleActive(Guid lastEditById)
        {
            Active = !Active;
            LastEditById = lastEditById;
            LastEditDate = DateTime.Now;
            return Active;
        }
        public Guid? UserId { get; private set; }
        public Guid DashingId { get; private set; }
        public Guid? ParentId { get; private set; }
        public string Content { get; private set; }
        public CommentType Type { get; private set; }
        public string MediaUrl { get; private set; }
        public bool Active { get; private set; }
        public Guid? LastEditById { get; private set; }
        public DateTime? LastEditDate { get; private set; }
        public Guid CreatedById { get; private set; }
        public DateTime CreatedDate { get; private set; }
        public byte[] RowVersion { get; private set; }
    }
    public enum CommentType
    {
        Comment,
        DonationComment
    }
}
