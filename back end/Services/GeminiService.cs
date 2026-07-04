using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using OpenAI.Chat;
using School_Project_API.DTO;
using School_Project_API.Services.Interfaces;



namespace SchoolManagementSystem.API.Services;

public class GeminiService : IGeminiService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public GeminiService(IConfiguration config, HttpClient httpClient)
    {
        _httpClient = httpClient;

  
        _apiKey = config["Gemini:ApiKey"];
    }

    public async Task<string> SendMessageAsync(string message)
    {

        var url =
$"https://generativelanguage.googleapis.com/v1/models/gemini-2.5-flash:generateContent?key={_apiKey}";


        var body = new
        {
            contents = new[]
            {
            new
            {
                parts = new[]
                {
                    new { text = message }
                }
            }
        }
        };

        var json = JsonSerializer.Serialize(body);

        var response = await _httpClient.PostAsync(
            url,
            new StringContent(json, Encoding.UTF8, "application/json")
        );

        var result = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            throw new Exception(result);

        using var doc = JsonDocument.Parse(result);

        return doc.RootElement
            .GetProperty("candidates")[0]
            .GetProperty("content")
            .GetProperty("parts")[0]
            .GetProperty("text")
            .GetString();
    }

}
