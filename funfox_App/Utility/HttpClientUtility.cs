using System.Text;

namespace funfox_App.Utility
{
    public class HttpClientUtility
    {
        private readonly HttpClient _httpClient;
        private string _bearerToken;

        public HttpClientUtility(string baseUrl)
        {
            _httpClient = new HttpClient { BaseAddress = new Uri(baseUrl) };
        }

        public void SetBearerToken(string bearerToken)
        {
            _bearerToken = bearerToken;
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _bearerToken);
        }

        public async Task<string> GetAsync(string endpoint)
        {
            if (!string.IsNullOrEmpty(_bearerToken))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _bearerToken);
            }

            HttpResponseMessage response = await _httpClient.GetAsync(_httpClient.BaseAddress + endpoint);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> PostAsync(string endpoint, string content)
        {
            if (!string.IsNullOrEmpty(_bearerToken))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _bearerToken);
            }

            var stringContent = new StringContent(content, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync(_httpClient.BaseAddress + endpoint, stringContent);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<HttpResponseMessage> PostAsync(string endpoint, StringContent content)
        {
            return await _httpClient.PostAsync(_httpClient.BaseAddress + endpoint, content);
            //response.EnsureSuccessStatusCode();
            //return await response.Content.ReadAsStringAsync();
        }
    }
}
