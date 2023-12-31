﻿using BirthdayDashing.Domain.Roles;
using Common.Feedback;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System;
using System.Linq;
using System.Net.Mime;
using System.Security.Claims;
using static Common.Exception.Messages;

namespace BirthdayDashing.API.Controllers
{
    [Authorize(Roles = Role.AdminOrUser)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]    
    [ProducesResponseType(typeof(Feedback<bool>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Feedback<bool>), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(Feedback<bool>), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(Feedback<bool>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(Feedback<bool>), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(Feedback<bool>), StatusCodes.Status409Conflict)]    
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected Guid AuthenticatedUserId => Guid.Parse(User.Identity.Name);
        protected string[] AuthenticatedUserRoles => User.Claims.Where(x => x.Type == ClaimTypes.Role)?.Select(x => x.Value)?.ToArray();
        public override OkObjectResult Ok([ActionResultObjectValue] object value)
        {            
            return base.Ok(new Feedback<object>(value, MessageType.Success, THE_OPERATION_WAS_DONE_SUCCESSFULLY, ""));
        }
    }
}
