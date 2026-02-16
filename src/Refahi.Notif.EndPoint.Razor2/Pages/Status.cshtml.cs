using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Refahi.Notif.EndPoint.Razor.Pages
{
    public class StatusModel : PageModel
    {
        public bool IsOk;
        public bool IsKaveNegarOk;

        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        public StatusModel(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task OnGetAsync()
        {
            var baseUrl = _configuration["ApiBaseUrl"];
            _httpClient.BaseAddress = new Uri(baseUrl);

            var response = await _httpClient.GetAsync("healthcheck");
            IsOk = response.IsSuccessStatusCode;

            var kaveResponse = await _httpClient.GetAsync("healthcheck/kavenegar");
            IsKaveNegarOk = kaveResponse.IsSuccessStatusCode;


        }
    }
}
