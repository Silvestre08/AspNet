using EventScheduler.Controllers.Base;
using EventScheduler.Data;
using EventScheduler.Model.Auth;
using EventScheduler.Services.Model.Event;
using EventScheduler.Services.Model.Participant;
using EventScheduler.Services.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EventScheduler.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ParticipantsController : BaseController
    {
        private readonly IEventDataService _eventDataService;
        public ParticipantsController(IEventDataService eventDataService)
        {
            _eventDataService = eventDataService;
        }

        /// <summary>
        /// Add participat to the event
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddParticipant([FromBody] NewParticipantModel data)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var newEventId=_eventDataService.AddParticipant(CurrentUserId, data);
            return Ok(newEventId);
        }
        /// <summary>
        /// Remove participat from event
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete([FromRoute] Guid id)
        {
            _eventDataService.DeleteParticipant(CurrentUserId,id);
            return Ok(id);
        }
    }
}
