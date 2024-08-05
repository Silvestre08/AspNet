using EventScheduler.Controllers.Base;
using EventScheduler.Data;
using EventScheduler.Model.Auth;
using EventScheduler.Services.Model.Event;
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
    public class EventsController : BaseController
    {
        private readonly IEventDataService _eventDataService;
        public EventsController(IEventDataService eventDataService)
        {
            _eventDataService = eventDataService;
        }

        [HttpGet]
        public IActionResult GetEvents()
        {
            var events = _eventDataService.GetEvents(CurrentUserId);
            return Ok(events);
        }
        [HttpGet]
        [Route("external")]
        public IActionResult GetExternalEvents()
        {
            var events = _eventDataService.GetExcernalEvents(CurrentUserId);
            return Ok(events);
        }
        [HttpGet]
        [Route("{id}/speakers")]
        public IActionResult GetSpeakersByEvent([FromRoute] Guid id)
        {
            var speakers = _eventDataService.GetSpeakersByEvent(CurrentUserId,id);
            return Ok(speakers);
        }
        [HttpGet]
        [Route("{id}/participants")]
        public IActionResult GetParticipantsByEvent([FromRoute] Guid id)
        {
            var participants = _eventDataService.GetParticipantsByEvent(CurrentUserId, id);
            return Ok(participants);
        }
        [HttpPost]
        public IActionResult AddEvent([FromBody] NewEventModel data)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var newEventId=_eventDataService.AddEvent(CurrentUserId, data);
            return Ok(newEventId);
        }
        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete([FromRoute] Guid id)
        {
            _eventDataService.DeleteEvent(CurrentUserId,id);
            return Ok(id);
        }
    }
}
