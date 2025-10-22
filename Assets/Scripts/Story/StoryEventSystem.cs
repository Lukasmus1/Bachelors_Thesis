using System;

namespace Story
{
    public class StoryEventSystem
    {
        public static event Action<string> OnEventRaised;

        public static void Raise(string eventName)
        {
            OnEventRaised?.Invoke(eventName);
        }
    }
}