using System;
using Apps.VirusFinder.Commons;
using Desktop.Commons;

namespace Story.Models.States
{
    [Serializable]
    public class VirusFinderStateClass : StateClass
    {
        public override int State { get; } = (int)StatesEnum.VirusFinder;
        public override int NextState { get; set; } = (int)StatesEnum.AfterVirus;

        public override void OnEnter()
        {
            DesktopMvc.Instance.DesktopGeneratorController.ToggleIcon("Virus Finder", true);
            
            //Story-wise this is called a bit late, but for practical purposes it's better to do it here
            VirusFinderMvc.Instance.VirusFinderController.CreateRandomViruses(5);
            
            LoadFromState();
        }

        public override void OnExit()
        {
            VirusFinderMvc.Instance.VirusFinderController.onVirusesCountChanged -= TransitionCheck;
        }

        public override void LoadFromState()
        {
            VirusFinderMvc.Instance.VirusFinderController.onVirusesCountChanged += TransitionCheck;
        }
        
        private void TransitionCheck(int virusCount)
        {
            if (virusCount == 0)
            {
                VirusFinderMvc.Instance.VirusFinderController.ResetVirusCount();
                ChangeToNextState();
            }
        }
    }
}