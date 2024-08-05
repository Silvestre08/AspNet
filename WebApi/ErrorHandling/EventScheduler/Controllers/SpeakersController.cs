using EventScheduler.Controllers.Base;
using EventScheduler.Data;
using EventScheduler.Model.Auth;
using EventScheduler.Services.Model.Event;
using EventScheduler.Services.Model.Participant;
using EventScheduler.Services.Model.Speaker;
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
    public class SpeakersController : BaseController
    {
        private readonly IEventDataService _eventDataService;
        public SpeakersController(IEventDataService eventDataService)
        {
            _eventDataService = eventDataService;
        }

      
        [HttpPost]
        public IActionResult AddSpeaker([FromBody] NewSpeakerModel data)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var newEventId=_eventDataService.AddSpeaker(CurrentUserId, data);
            return Ok(newEventId);
        }
        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete([FromRoute] Guid id)
        {
            _eventDataService.DeleteSpeaker(CurrentUserId,id);
            return Ok(id);
        }
    }
}
