using System;
using System.Collections.Generic;
using System.Linq;

namespace Story.Models.Actions
{
    /// <summary>
    /// Some actions span across multiple states and need to be persistent throughout the story.
    /// This class holds the persistent actions and their states.
    /// </summary>
    [Serializable]
    public class ListOfActionsPersistent
    {
        public List<Pair> actions = new();

        public bool GetAction(ActionType key)
        {
            return actions.Find(x => x.key == key).value;
        }

        public void SetAction(ActionType key, bool value)
        {
            if (actions.Any(x => x.key == key))
            {
                actions.Find(x => x.key == key).value = value;   
            }
            else
            {
                actions.Add(new Pair(key, value));
            }
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