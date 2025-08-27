using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using TourBooking.Application.Interfaces.Services;
using TourBooking.Domain.Entities;
using Newtonsoft.Json;
using TourBooking.Application.DTOs;
using Microsoft.Extensions.Configuration;
using System.Net;
using TourBooking.Application.Expactions;


public sealed class NetgsmSmsService : INetgsmSmsService
{
    private static readonly Uri BaseUri = new("https://api.netgsm.com.tr");
    private const string SendPath = "/sms/rest/v2/send";

    private readonly HttpClient _http;
    private readonly NetgsmOptions _opt;

    public NetgsmSmsService(HttpClient http, IOptions<NetgsmOptions> opt)
    {
        _http = http;
        _opt = opt.Value;

        _http.BaseAddress ??= BaseUri;
        _http.Timeout = TimeSpan.FromSeconds(2);

        // Authorization: Basic base64(username:password)
        var plain = $"{_opt.Username}:{_opt.Password}";
        var base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(plain));
        var credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_opt.Username}:{_opt.Password}"));
        _http.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Basic", base64);
        _http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public async Task SendAsync(string phoneNumber, string message, CancellationToken ct = default)
    {
        var payload = new
        {
            msgheader = _opt.MsgHeader,
            encoding = "TR",
            messages = new[] { new { msg = message, no = phoneNumber } }
        };

        using var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
        using var resp = await _http.PostAsync(SendPath, content, ct);
        var body = await resp.Content.ReadAsStringAsync(ct);

        // Hata fırlat ki üst katman loglayıp handle edebilsin
        resp.EnsureSuccessStatusCode();
        // İstersen body'yi loglayabilirsin
        // _logger?.LogInformation("Netgsm OK: {Body}", body);
    }

    public async Task SendBatchAsync(IEnumerable<SmsMessageDto> items, CancellationToken ct = default)
    {
        var messages = items.Select(i => new { msg = i.message, no = i.phone }).ToArray();


        var data = new
        {
            msgheader = _opt.MsgHeader,
            messages = messages,
            encoding = "TR",
            iysfilter = "",
            partnercode = ""
        };

        string url = "https://api.netgsm.com.tr/sms/rest/v2/send";
        string username = _opt.Username;
        string password = _opt.Password;

        using (HttpClient client = new HttpClient())
        {
            var credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{password}"));
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", credentials);
            // client.DefaultRequestHeaders.Add("Content-Type", "application/json");
            client.DefaultRequestHeaders.Accept
                            .Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.Timeout = TimeSpan.FromSeconds(2);

            var jsonContent = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");

            try
            {
                HttpResponseMessage response = await client.PostAsync(url, jsonContent);
                if (response != null && response.StatusCode != HttpStatusCode.OK)
                {
                    throw new BusinessRuleValidationException(await response.Content.ReadAsStringAsync());
                }
                else
                {
                    string responseString = await response.Content.ReadAsStringAsync();
                    // job id yakala ve DB insert
                    Console.WriteLine("bilgiler" + _opt.Username + _opt.MsgHeader + _opt.Password);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
