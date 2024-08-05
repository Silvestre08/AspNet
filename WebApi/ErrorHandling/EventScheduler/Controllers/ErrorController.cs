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
    [AllowAnonymous]
    public class ErrorController : BaseController
    {
        private readonly IEventDataService _eventDataService;
        public ErrorController(IEventDataService eventDataService)
        {
            _eventDataService = eventDataService;
        }

        [HttpGet]
        public IActionResult EventThatWillFail()
        {
             _eventDataService.AddEvent("useridthatdoesntexist",new NewEventModel
            {
                 Description="test",
                 Name="test",
                  End=DateTime.UtcNow,
                  Start=DateTime.UtcNow,
                   
            });
            return Ok();
        }

    }
}
