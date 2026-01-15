using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Uply.Domain.Abstractions.Services;
using Uply.Domain.Constants;
using Uply.Domain.Entities;
using Uply.Domain.Enums;
using Uply.Domain.Models.Dto.RoadmapTasks;
using Uply.Web.Models._Roadmap_;

namespace Uply.Infrastructure.ChatGpt;

public class ChatGptClient : IChatGptClient
{
    private readonly HttpClient _httpClient;
    private readonly ChatGptOptions _options;
    private readonly ILogger<ChatGptClient> _logger;

    public ChatGptClient(IOptions<ChatGptOptions> options, IHttpClientFactory httpClientFactory, ILogger<ChatGptClient> logger)
    {
        _options = options.Value;
        _httpClient = httpClientFactory.CreateClient();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _options.ApiKey);
        _logger = logger;
    }

    public async Task<List<RoadmapTask>> GenerateRoadmapTasksAsync(CreateRoadmapModel model)
    {
        var taskAmount = RoadmapConstants.MaxTasksInRoadmap;
        if (model.Deadline.HasValue)
        {
            var daysRemain = (int)(model.Deadline.Value.ToDateTime(TimeOnly.MinValue) - DateTime.Today).TotalDays;
            taskAmount = daysRemain / model.Period.ToDaysCount();
        }

        var prompt = ChatGptConstants.PromptTemplate
            .Replace("{StartingPoint}", model.StartingPoint)
            .Replace("{Goal}", model.Goal)
            .Replace("{Deadline}", model.Deadline?.ToString("dd.MM.yyyy") ?? "не указан")
            .Replace("{Period}", model.Period.ToString())
            .Replace("{ManHoursPerTask}", ((int)model.ManHoursPerTask).ToString())
            .Replace("{TaskAmount}", taskAmount.ToString());

        var requestBody = new
        {
            model = _options.Model,
            messages = new[]
            {
                new { role = "user", content = prompt }
            },
            temperature = 0.7
        };

        var response = await _httpClient.PostAsJsonAsync("https://api.openai.com/v1/chat/completions", requestBody);

        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();

        using var doc = JsonDocument.Parse(json);
        var content = doc.RootElement
            .GetProperty("choices")[0]
            .GetProperty("message")
            .GetProperty("content")
            .GetString();

        var rawContent = content ?? string.Empty;

        if (rawContent.StartsWith("```"))
        {
            var firstNewline = rawContent.IndexOf('\n');
            var lastTicks = rawContent.LastIndexOf("```");
            if (firstNewline >= 0 && lastTicks > firstNewline)
            {
                rawContent = rawContent.Substring(firstNewline + 1, lastTicks - firstNewline - 1);
            }
        }

        var gptTasks = rawContent is null ? [] : JsonSerializer.Deserialize<List<RoadmapTaskGptDto>>(rawContent) ?? [];

        var roadmapTasks = gptTasks
            .Select((x, idx) => new RoadmapTask()
            {
                Description = x.Description,
                Title = x.Title,
                TaskNumber = idx + 1
            })
            .ToList();

        return roadmapTasks;
    }
}
