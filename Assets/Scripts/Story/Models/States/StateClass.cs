using System;
using Story.Commons;

namespace Story.Models.States
{
    [Serializable]
    public abstract class StateClass
    {
        public abstract int State { get; }
        public abstract int NextState { get; set; }
        public abstract void OnEnter();
        public abstract void OnExit();
        public abstract void LoadFromState();

        protected void ChangeToNextState()
        {
            StoryMvc.Instance.StoryController.CurrentStateClass = StateFactory.GetState(NextState);
        }
    }
}