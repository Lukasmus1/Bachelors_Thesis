using System;
using System.Collections.Generic;

namespace Apps.ChatTerminal.Models
{
    [Serializable]
    public class ChatProfilesWrapper
    {
        // ReSharper disable once InconsistentNaming
        public List<ChatProfileModel> ChatProfiles;
    }

    [Serializable]
    public class SecondaryChatProfilesWrapper
    {
        // ReSharper disable once InconsistentNaming
        public List<SecondaryChatProfileModel> ChatProfiles;
    }
}