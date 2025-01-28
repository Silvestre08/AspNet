using ImageGallery.API.Services;
using Microsoft.AspNetCore.Authorization;

namespace ImageGallery.API.Authorization
{
    public class MustOwnImageHandler : AuthorizationHandler<MustOwnImageRequirement>
    {
        private readonly IGalleryRepository _galleryRepository;
        private readonly IHttpContextAccessor _contextAccessor;

        public MustOwnImageHandler(IHttpContextAccessor httpContextAccessor, IGalleryRepository galleryRepository)
        {
            _contextAccessor = httpContextAccessor;
            _galleryRepository = galleryRepository;        
        }
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, MustOwnImageRequirement requirement)
        {
            var imageId = _contextAccessor.HttpContext?.GetRouteValue("id")?.ToString();
            if (!Guid.TryParse(imageId, out Guid idAsGuid)) 
            {
                context.Fail();
                return;
            }

            var imagerOwner = (await _galleryRepository.GetImageAsync(idAsGuid))?.OwnerId;
            
            if (context.User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value == imagerOwner) 
            {
                context.Succeed(requirement);
            }
        }
    }
}
