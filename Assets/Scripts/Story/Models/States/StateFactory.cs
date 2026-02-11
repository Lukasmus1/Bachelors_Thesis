using System;

namespace Story.Models.States
{
    public static class StateFactory
    {
        /// <summary>
        /// Gets the state class corresponding to the given state enum value.
        /// </summary>
        /// <param name="stateEnum">Enum value of the state</param>
        /// <returns>Class associated with the enum</returns>
        /// <exception cref="ArgumentOutOfRangeException">Gets thrown if there is an enum that has no class</exception>
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
                StatesEnum.Detective => new NewFilesStateClass(),
                _ => throw new ArgumentOutOfRangeException(nameof(stateEnum), stateEnum, null)
            };
        }
    }
}