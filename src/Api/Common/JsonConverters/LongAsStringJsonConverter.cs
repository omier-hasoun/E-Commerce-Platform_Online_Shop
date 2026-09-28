
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Api.Common.JsonConverters;
public sealed class LongAsStringJsonConverter : JsonConverter<long>
{
    public override long Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options)
    {
        switch (reader.TokenType)
        {
            case JsonTokenType.Number:
                if (reader.TryGetUInt64(out var longValue))
                    return (long)longValue;

                break;

            case JsonTokenType.String:
                var str = reader.GetString();

                if (long.TryParse(str, out var parsed))
                    return (long)parsed;

                break;
        }

        throw new JsonException(
            $"Unable to convert value to {nameof(Int64)}.");
    }

    public override void Write(
        Utf8JsonWriter writer,
        long value,
        JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}
