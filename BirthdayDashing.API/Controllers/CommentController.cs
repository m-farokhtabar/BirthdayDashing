using BirthdayComment.Application.Comments;
using BirthdayDashing.Application.Comments;
using BirthdayDashing.Application.Dtos.Comment.Input;
using BirthdayDashing.Application.Dtos.Comment.Output;
using Common.Exception;
using Common.Feedback;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;
using static Common.Exception.Messages;

namespace BirthdayDashing.API.Controllers
{
    [Consumes(MediaTypeNames.Application.Json)]
    public class CommentController : BaseController
    {
        private readonly ICommentWriteService WriteService;
        private readonly ICommentReadService ReadService;

        public CommentController(ICommentWriteService writeService, ICommentReadService readService)
        {
            WriteService = writeService;
            ReadService = readService;
        }

        [HttpPost]
        public async Task<ActionResult<Feedback<bool>>> Add([FromBody] AddCommentDto Comment)
        {
            await WriteService.AddAsync(Comment);
            return Ok(true);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<Feedback<bool>>> Update(Guid id, [FromBody] UpdateCommentDto Comment)
        {
            await WriteService.UpdateAsync(id, Comment);
            return Ok(true);
        }
        [HttpPut("ToggleActive/{id}")]
        public async Task<ActionResult<Feedback<bool>>> ToggleActive(Guid id)
        {
            return Ok(await WriteService.ToggleActiveAsync(id));
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Feedback<CommentDto>>> Get(Guid id)
        {
            var Value = await ReadService.GetAsync(id);
            return Value != null ? Ok(Value) : throw new ManualException(DATA_IS_NOT_FOUND.Replace("{0}", "Comment"), ExceptionType.NotFound, "Id");
        }
        /// <summary>
        /// Get all Comments of the Dashing
        /// </summary>
        /// <returns></returns>
        [HttpGet("All/{DashingId}/{Date?}")]
        public async Task<ActionResult<Feedback<IEnumerable<CommentListDto>>>> GetByDashingId(Guid DashingId, DateTime? Date)
        {
            var Value = await ReadService.GetByDashingIdAsync(DashingId, Date);
            return Value?.Count > 0 ? Ok(Value) : throw new ManualException(DATA_IS_NOT_FOUND.Replace("{0}", "Comment"), ExceptionType.NotFound, "Id");
        }
        /// <summary>
        /// Get Children  Of an Comment
        /// </summary>
        /// <returns></returns>
        [HttpGet("Children/{id}/{Date?}")]
        public async Task<ActionResult<Feedback<IEnumerable<CommentListDto>>>> GetChildrenComment(Guid id, DateTime? Date)
        {
            var Value = await ReadService.GetChildren(id, Date);
            return Value?.Count > 0 ? Ok(Value) : throw new ManualException(THERE_ARE_ANY_DATA_TO_SHOW.Replace("{0}", "Inner comments"), ExceptionType.NotFound, "Id");
        }
    }
}
