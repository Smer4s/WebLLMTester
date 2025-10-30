
using System.Text.Json;
using System.Text;

namespace WebLLMTester
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            var summaries = new[]
            {
                "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
            };

            app.MapGet("/weatherforecast", (HttpContext httpContext) =>
            {
                var forecast = Enumerable.Range(1, 5).Select(index =>
                    new WeatherForecast
                    {
                        Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                        TemperatureC = Random.Shared.Next(-20, 55),
                        Summary = summaries[Random.Shared.Next(summaries.Length)]
                    })
                    .ToArray();
                return forecast;
            })
            .WithName("GetWeatherForecast")
            .WithOpenApi();

            app.MapPost("/ask", async (PromptRequest request, HttpContext context) =>
            {
                using var client = new HttpClient();

                var ollamaUrl = "http://localhost:11434/api/generate";

                var payload = new
                {
                    model = "phi3:mini", // или другая модель
                    prompt = request.Prompt,
                    stream = true
                };

                using var response = await client.PostAsJsonAsync(ollamaUrl, payload, context.RequestAborted);

                response.EnsureSuccessStatusCode();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                var sb = new StringBuilder();

                // Читаем стрим построчно
                using var stream = await response.Content.ReadAsStreamAsync();
                using var reader = new StreamReader(stream);

                while (!reader.EndOfStream)
                {
                    var line = await reader.ReadLineAsync();
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    try
                    {
                        var json = JsonSerializer.Deserialize<OllamaChunk>(line, options);
                        if (json?.Response != null)
                        {
                            sb.Append(json.Response);
                        }
                    }
                    catch
                    {
                        // Игнорируем кривые куски
                    }
                }

                var result = new { text = sb.ToString() };
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(result);
            })
                .WithName("AskLLM")
            .WithOpenApi();

            app.MapPost("/generate-roadmap", async (RoadmapRequest request, HttpContext context) =>
            {
                using var client = new HttpClient();
                var ollamaUrl = "http://localhost:11434/api/generate";

                // Жёсткий промпт
                var prompt = $@"
                Составь roadmap для цели: {request.GoalTitle}.
                Описание: {request.GoalDescription}.

                Формат ответа: JSON-массив из 30 объектов.
                Каждый объект имеет поля:
                - ""title"": краткое название задачи (без номера шага).
                - ""description"": конкретные инструкции, что именно нужно сделать, и указание, какой отчёт нужно предоставить для подтверждения выполнения.

                Все задачи должны быть написаны на русском языке.

                ⚡ Очень важно:
                - Задачи должны быть абсолютно конкретными и измеримыми.
                - Для спорта всегда указывай числа: например, ""пройти 10 000 шагов"", ""сделать 3 подхода по 12 приседаний"", ""съесть рацион на 2000 ккал"".
                - Для изучения языков и других навыков — пошагово и конкретно: ""изучить спряжение глаголов в настоящем времени"", ""выучить 20 новых слов по теме 'Еда'"", ""прочитать одну статью и выписать 10 новых выражений"".
                - Не добавляй абстрактных задач вроде ""ознакомиться"", ""создать план"", ""обсудить"". Каждая задача должна быть проверяемой и требовать отчёта.
                - В description всегда указывай форму отчёта: фото, скриншот, текстовый файл, аудио или видео.

                Пример:
                [
                  {{
                    ""title"": ""Ходьба"",
                    ""description"": ""Пройди 10 000 шагов за день. Отправь отчёт в виде скриншота с фитнес-трекера.""
                  }},
                  {{
                    ""title"": ""Силовая тренировка"",
                    ""description"": ""Сделай 3 подхода по 12 приседаний, 3 подхода по 10 отжиманий и 3 подхода по 30 секунд планки. Отправь отчёт в виде фото или видео тренировки.""
                  }},
                  {{
                    ""title"": ""Изучение грамматики"",
                    ""description"": ""Выучи спряжение глаголов в настоящем времени в английском языке. Отправь отчёт в виде фото конспекта или скриншота выполненных упражнений.""
                  }}
                ]

                Не добавляй пояснений, комментариев или текста вне JSON.
                ";


                var payload = new
                {
                    model = "llama3:8b",
                    prompt,
                    stream = true
                };

                using var response = await client.PostAsJsonAsync(ollamaUrl, payload, context.RequestAborted);
                response.EnsureSuccessStatusCode();

                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var sb = new StringBuilder();

                using var stream = await response.Content.ReadAsStreamAsync();
                using var reader = new StreamReader(stream);

                while (!reader.EndOfStream)
                {
                    var line = await reader.ReadLineAsync();
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    try
                    {
                        var json = JsonSerializer.Deserialize<OllamaChunk>(line, options);
                        if (json?.Response != null) sb.Append(json.Response);
                    }
                    catch { }
                }

                List<RoadmapTask> tasks;
                var taskCount = 0;
                try
                {
                    var raw = sb.ToString();
                    raw = raw.Trim();
                    if (raw.StartsWith("```"))
                    {
                        // убрать markdown-обёртку, если модель её добавила
                        raw = raw.Trim('`').Trim();
                    }

                    Console.WriteLine("RAW RESPONSE: " + raw);

                    // 1. Прямая попытка
                    var parsed = JsonSerializer.Deserialize<List<TempTask>>(raw, options);

                    // 2. Если модель вернула JSON внутри строки
                    if (parsed == null)
                    {
                        var inner = JsonSerializer.Deserialize<string>(raw, options);
                        if (!string.IsNullOrWhiteSpace(inner))
                            parsed = JsonSerializer.Deserialize<List<TempTask>>(inner, options);
                    }

                    tasks = parsed?.Select((p, i) =>
                        new RoadmapTask(taskCount++, p.Title, p.Description, false)
                    ).ToList() ?? new List<RoadmapTask>();
                }
                catch
                {
                    tasks = new List<RoadmapTask>();
                }

                tasks = tasks.Where(t => !string.IsNullOrWhiteSpace(t.Title)
                                      && !t.Title.Contains("пусто", StringComparison.OrdinalIgnoreCase))
                             .ToList();

                var result = new { roadmap = tasks };
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(result);
            })
.WithName("GenerateRoadmap")
.WithOpenApi();

            app.Run();
        }
    }
}


record PromptRequest(string Prompt);


// Модели
record RoadmapRequest(string GoalTitle, string GoalDescription);

class TempTask
{
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
}

record RoadmapTask(int Id, string Title, string Description, bool IsCompleted = false);

class OllamaChunk
{
    public string? Response { get; set; }
    public bool Done { get; set; }
}