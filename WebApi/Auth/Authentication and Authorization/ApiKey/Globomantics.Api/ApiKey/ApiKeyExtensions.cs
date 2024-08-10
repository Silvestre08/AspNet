namespace Globomantics.Api.ApiKey
{
    public static class ApiKeyExtensions
    {
        public static void UseApiKeyAuthentication(this IApplicationBuilder webApplication) 
        {
            webApplication.UseMiddleware<ApiKeyMiddleware>();
        }
    }
}
