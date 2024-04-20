namespace funfox_App.Utility
{
    public class BearerTokenMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly HttpClientUtility _httpClientUtility;

        public BearerTokenMiddleware(RequestDelegate next, HttpClientUtility httpClientUtility)
        {
            _next = next;
            _httpClientUtility = httpClientUtility;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Headers.ContainsKey("Authorization"))
            {
                string bearerToken = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                _httpClientUtility.SetBearerToken(bearerToken);
            }

            await _next(context);
        }
    }
}
