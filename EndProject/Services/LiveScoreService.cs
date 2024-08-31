using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

public class LiveScoreService
{
    private readonly HttpClient _httpClient;

    public LiveScoreService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        // Bypass SSL validation (Development only!)
        _httpClient.DefaultRequestHeaders.Add("RxfIRJfRZEks1X3Y", "CxIeJ2egheRAQa8HInqz7Gr6kMzlJCkm");
        HttpClientHandler handler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (HttpRequestMessage, cert, cetChain, policyErrors) => true
        };
        _httpClient = new HttpClient(handler);
    }

    public async Task<JObject> GetLiveScoresAsync()
    {
        var response = await _httpClient.GetAsync("https://livescore-api.com/api-client/matches/live.json?&key=RxfIRJfRZEks1X3Y&secret=CxIeJ2egheRAQa8HInqz7Gr6kMzlJCkm");
        response.EnsureSuccessStatusCode();
        string content = await response.Content.ReadAsStringAsync();
        return JObject.Parse(content);
    }
}
