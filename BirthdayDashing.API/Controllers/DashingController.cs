using BirthdayDashing.Application.Dashings;
using BirthdayDashing.Application.Dtos.Dashings.Input;
using BirthdayDashing.Application.Dtos.Dashings.Output;
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
    public class DashingController : BaseController
    {
        private readonly IDashingWriteService WriteService;
        private readonly IDashingReadService ReadService;

        public DashingController(IDashingWriteService writeService, IDashingReadService readService)
        {
            WriteService = writeService;
            ReadService = readService;
        }

        [HttpPost]
        public async Task<ActionResult<Feedback<bool>>> Add([FromBody] AddDashingDto dashing)
        {
            await WriteService.AddAsync(dashing);
            return Ok(true);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<Feedback<bool>>> Update(Guid id, [FromBody] UpdateDashingDto dashing)
        {
            await WriteService.UpdateAsync(id, dashing);
            return Ok(true);
        }
        [HttpPut("ToggleActive/{id}")]
        public async Task<ActionResult<Feedback<bool>>> ToggleActive(Guid id)
        {            
            return Ok(await WriteService.ToggleActiveAsync(id));
        }
        [HttpPut("ToggleDeleted/{id}")]
        public async Task<ActionResult<Feedback<bool>>> ToggleDeleted(Guid id)
        {
            return Ok(await WriteService.ToggleDeletedAsync(id));
        }
        [HttpGet("{Id}")]
        public async Task<ActionResult<Feedback<DashingDto>>> Get(Guid id)
        {
            var Value = await ReadService.GetAsync(id);
            return Value != null ? Ok(Value) : throw new ManualException(DATA_IS_NOT_FOUND.Replace("{0}", "Dashing"), ExceptionType.NotFound, "Id");
        }
        /// <summary>
        /// Get all dashings of the user
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<Feedback<IEnumerable<DashingDto>>>> Get()
        {
            var Value = await ReadService.GetByUserIdAsync(AuthenticatedUserId);
            return Value != null ? Ok(Value) : throw new ManualException(DATA_IS_NOT_FOUND.Replace("{0}", "Dashing"), ExceptionType.NotFound, "Id");
        }
    }
}
