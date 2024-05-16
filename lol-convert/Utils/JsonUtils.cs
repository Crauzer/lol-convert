using System.Text.Json;

namespace lol_convert.Utils;

internal static class JsonUtils
{
    public static readonly JsonSerializerOptions DefaultOptions = new()
    {
        WriteIndented = true,
        IncludeFields = true,
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
    };
}
