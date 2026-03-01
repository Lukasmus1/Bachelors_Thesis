using System;
using System.Collections.Generic;

namespace Story.Models.Actions
{
    /// <summary>
    /// Some actions span across multiple states and need to be persistent throughout the story.
    /// This class holds the persistent actions and their states.
    /// </summary>
    [Serializable]
    public class ListOfActionsPersistent
    {
        public List<Pair> actions = new()
        {
            (new Pair(ActionType.HiddenVirus, false))
        };

        public bool GetAction(ActionType key)
        {
            return actions.Find(x => x.key == key).value;
        }

        public void SetAction(ActionType key, bool value)
        {
            actions.Find(x => x.key == key).value = value;
        }
    }
    
    [Serializable]
    public class Pair
    {
        public ActionType key;
        public bool value;

        public Pair(ActionType k, bool v)
        {
            key = k;
            value = v;
        }
    }

    [Serializable]
    public enum ActionType
    {
        HiddenVirus,
        ImportantFile
    }
}