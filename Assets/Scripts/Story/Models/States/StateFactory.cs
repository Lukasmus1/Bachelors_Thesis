using System;

namespace Story.Models.States
{
    public static class StateFactory
    {
        public static IState GetState(int stateEnum)
        {
            var state = (StatesEnum)stateEnum;
            
            return state switch
            {
                StatesEnum.Start => new StartState(),
                StatesEnum.MouseQuest => new MouseQuestState(),
                
                StatesEnum.Default => new DefaultState(),
                _ => throw new ArgumentOutOfRangeException(nameof(stateEnum), stateEnum, null)
            };
        }
    }
}