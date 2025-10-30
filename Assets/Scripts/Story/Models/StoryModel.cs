using System;
using Story.Models.States;

namespace Story.Models
{
    [Serializable]
    public class StoryModel
    {
        private IState _currentState;
        public IState CurrentState
        {
            get => _currentState;
            set
            {
                if (_currentState != value)
                {
                    _currentState?.OnExit();
                    value.OnEnter();
                    
                }
                _currentState = value;
            }
        }
        
        public void Init()
        {
            CurrentState = new StartState();
        }
    }
}