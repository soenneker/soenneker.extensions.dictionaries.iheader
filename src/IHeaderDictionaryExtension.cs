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
        var buffer = new ArrayBufferWriter<byte>(1024);
        using var writer = new Utf8JsonWriter(buffer);

        writer.WriteStartObject();

        foreach ((string key, StringValues values) in headers)
        {
            writer.WritePropertyName(key);

            if (values.Count <= 1)
            {
                writer.WriteStringValue(values.ToString());
            }
            else
            {
                writer.WriteStartArray();

                for (var i = 0; i < values.Count; i++)
                {
                    writer.WriteStringValue(values[i]);
                }

                writer.WriteEndArray();
            }
        }

        writer.WriteEndObject();
        writer.Flush();

        return Encoding.UTF8.GetString(buffer.WrittenSpan);
    }
}