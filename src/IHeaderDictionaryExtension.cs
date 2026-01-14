using System.Buffers;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Soenneker.Extensions.Dictionaries.IHeader;

/// <summary>
/// A collection of helpful IHeaderDictionary extension methods
/// </summary>
// ReSharper disable once InconsistentNaming
public static class IHeaderDictionaryExtension
{
    /// <summary>
    /// Converts headers to a compact JSON string without LINQ allocations.
    /// </summary>
    public static string ToJsonString(this IHeaderDictionary headers)
    {
        // If headers are small, 512 is usually enough;
        var buffer = new ArrayBufferWriter<byte>(512);

        var writerOptions = new JsonWriterOptions
        {
            // Safe here because we control the structure and property names come from the server header collection.
            // (If you ever pass arbitrary untrusted property names, keep validation on.)
            SkipValidation = true
        };

        using var writer = new Utf8JsonWriter(buffer, writerOptions);

        writer.WriteStartObject();

        foreach ((string key, StringValues values) in headers)
        {
            writer.WritePropertyName(key);

            int count = values.Count;

            if (count <= 1)
            {
                // Avoid StringValues.ToString() to prevent join allocations / formatting paths.
                // If count == 0, write empty string (matches StringValues.ToString() behavior)
                writer.WriteStringValue(count == 1 ? values[0] : string.Empty);
                continue;
            }

            writer.WriteStartArray();
            for (int i = 0; i < count; i++)
                writer.WriteStringValue(values[i]);
            writer.WriteEndArray();
        }

        writer.WriteEndObject();
        writer.Flush();

        // Final string allocation is unavoidable.
        return Encoding.UTF8.GetString(buffer.WrittenSpan);
    }
}