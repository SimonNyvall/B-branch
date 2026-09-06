using System.Text.Json.Serialization;

namespace Bbranch.Shared.HttpDtos;

public class GithubPullRequestDto
{
    [JsonPropertyName("url")]
    public string HtmlUrl { get; set; } = string.Empty;
}
