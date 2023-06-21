using SocialMedia.Core.Messages;

namespace SocialMedia.Core.Events
{
    public abstract class BaseEvent : Message
    {
        protected BaseEvent(string type)
        {
            Type = type;
        }
        public int Version { get;  set; }
        public string Type { get; set; }
    }
}
