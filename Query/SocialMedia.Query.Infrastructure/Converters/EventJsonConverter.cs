using SocialMedia.Core.Events;
using System.Text.Json;
using System.Text.Json.Serialization;
using SocialMedia.Common.Events;

namespace SocialMedia.Query.Infrastructure.Converters
{
    public class EventJsonConverter : JsonConverter<BaseEvent>
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert.IsAssignableFrom(typeof(BaseEvent));
        }
        public override BaseEvent Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (!JsonDocument.TryParseValue(ref reader, out var doc))
            {
                throw new JsonException($"failed to parse {nameof(JsonDocument)}");
            }

            if (!doc.RootElement.TryGetProperty("Type", out var type))
            {
                throw new JsonException($"Could not detect the type discriminator property");
            }

            var eventType = type.GetString();
            var json = doc.RootElement.GetRawText();
            return eventType switch
            {
                nameof(PostCreatedEvent) => JsonSerializer.Deserialize<PostCreatedEvent>(json, options),
                nameof(MessageUpdatedEvent) => JsonSerializer.Deserialize<MessageUpdatedEvent>(json, options),
                nameof(PostLikedEvent) => JsonSerializer.Deserialize<PostLikedEvent>(json, options),
                nameof(PostRemovedEvent) => JsonSerializer.Deserialize<PostRemovedEvent>(json, options),
                nameof(CommendAddedEvent) => JsonSerializer.Deserialize<CommendAddedEvent>(json, options),
                nameof(CommentRemoveEvent) => JsonSerializer.Deserialize<CommentRemoveEvent>(json, options),
                nameof(CommentUpdatedEvent) => JsonSerializer.Deserialize<CommentUpdatedEvent>(json, options),
                _ => throw new JsonException($"Unknown event type: {eventType}")
            };
        }

        public override void Write(Utf8JsonWriter writer, BaseEvent value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
    
}
