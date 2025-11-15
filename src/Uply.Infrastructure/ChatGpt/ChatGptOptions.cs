namespace Uply.Infrastructure.ChatGpt;

public class ChatGptOptions
{
    public string ApiKey { get; set; } = null!;
    public string Model { get; set; } = "gpt-4.1-mini";
}
