using BirthdayDashing.Domain;
using Common.Feedback;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Net.Mime;

namespace BirthdayDashing.API.Controllers
{
    [Authorize(Roles = Role.AdminOrUser)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Feedback<bool>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Feedback<bool>), StatusCodes.Status500InternalServerError)]
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        public override OkObjectResult Ok([ActionResultObjectValue] object value)
        {
            return base.Ok(new Feedback<object>(FeedbackType.DoneSuccessfully, MessageType.Info, value));
        }
    }
}
