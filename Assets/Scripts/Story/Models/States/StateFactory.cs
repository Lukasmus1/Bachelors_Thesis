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
                StatesEnum.AutostereogramState => new AutostereogramState(),
                StatesEnum.MysteriousFile => new MysteriousFileState(),
                StatesEnum.AfterCrash => new AfterCrashStateClass(),
                StatesEnum.VirusFinderMessage => new VirusFinderMessageStateClass(),
                StatesEnum.VirusFinder => new VirusFinderStateClass(),
                StatesEnum.AfterVirus => new AfterVirusCleanupStateClass(),
                StatesEnum.ThomasFinal => new ThomasFinalCleanupStateClass(),
                StatesEnum.AfterThomasFinal => new AfterThomasFinalCleanupStateClass(),
                StatesEnum.CuratorFirst => new CuratorFirstStateClass(),
                _ => throw new ArgumentOutOfRangeException(nameof(stateEnum), stateEnum, null)
            };
        }
    }
}