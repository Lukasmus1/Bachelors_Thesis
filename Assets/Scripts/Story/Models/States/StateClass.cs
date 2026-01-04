using System;
using Story.Commons;
using UnityEngine;

namespace Story.Models.States
{
    [Serializable]
    public abstract class StateClass
    {
        public abstract int State { get; }
        public abstract int NextState { get; }
        public abstract void OnEnter();
        public abstract void OnExit();
        public abstract void LoadFromState();

        protected void ChangeState()
        {
            StoryMvc.Instance.StoryController.CurrentStateClass = StateFactory.GetState(NextState);
        }
    }
}