using System;
using Apps.VirusFinder.Commons;

namespace Story.Models.States
{
    [Serializable]
    public class VirusFinderStateClass : StateClass
    {
        public override int State { get; } = (int)StatesEnum.VirusFinder;
        public override int NextState { get; } = (int)StatesEnum.Default;

        public override void OnEnter()
        {
            VirusFinderMvc.Instance.VirusFinderController.EnableApp(true);
            
            //Story-wise this is called a bit late, but for practical purposes it's better to do it here
            VirusFinderMvc.Instance.VirusFinderController.CreateRandomViruses(5);
            
            LoadFromState();
        }

        public override void OnExit()
        {
            throw new NotImplementedException();
        }

        public override void LoadFromState()
        {
            throw new NotImplementedException();
        }
    }
}