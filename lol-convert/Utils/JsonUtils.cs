using System.Text.Json;
using System.Text.Json.Serialization;

namespace lol_convert.Utils;

internal static class JsonUtils
{
    public static readonly JsonSerializerOptions DefaultOptions = new()
    {
        WriteIndented = true,
        IncludeFields = true,
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };
}
