using System;
using Story.Models.States;

namespace Story.Models
{
    [Serializable]
    public class StoryModel
    {
        private StateClass currentStateClass;
        public StateClass CurrentStateClass
        {
            get => currentStateClass;
            set
            {
                if (currentStateClass != value)
                {
                    currentStateClass?.OnExit();
                    value.OnEnter();
                }
                currentStateClass = value;
            }
        }
        
        public void Init()
        {
            CurrentStateClass = new StartStateClass();
        }

        public void LoadFromState()
        {
            currentStateClass?.LoadFromState();
        }
    }
}