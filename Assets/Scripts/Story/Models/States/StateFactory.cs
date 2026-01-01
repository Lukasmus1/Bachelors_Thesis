using System;

namespace Story.Models.States
{
    public static class StateFactory
    {
        public static StateClass GetState(int stateEnum)
        {
            var state = (StatesEnum)stateEnum;
            
            return state switch
            {
                StatesEnum.Start => new StartStateClass(),
                StatesEnum.MouseQuest => new MouseQuestStateClass(),
                StatesEnum.ThomasBare => new ThomasBareStateClass(),
                StatesEnum.Default => new DefaultStateClass(),
                StatesEnum.MysteriousFile => new MysteriousFileStateClass(),
                _ => throw new ArgumentOutOfRangeException(nameof(stateEnum), stateEnum, null)
            };
        }
    }
}