using System;
using Apps.ChatTerminal.Commons;
using Story.Models.SideActions;
using User.Commons;
using User.Models;

namespace Story.Models.States
{
    [Serializable]
    public class CuratorFirstStateClass : StateClass
    {
        public override int State => (int)StatesEnum.CuratorFirst;
        public override int NextState { get; set; } = (int)StatesEnum.NewFiles;

        private bool _shouldUnsubscribe = false;
        private bool _curatorLoaded = false;
        
        public override void OnEnter()
        {
            LoadFromState();
        }

        public override void OnExit()
        {
            ChatTerminalMvc.Instance.MessageSystemController.messageTyped -= TransitionCheck;
        }

        public override void LoadFromState()
        {
            if (!UserMvc.Instance.UserController.GetPersistentData(UserDataType.DeletedVirusFile))
            {
                KpDoNotDeleteFiles.OnKpDoNotDeleteFiles += OnEnter;
                _shouldUnsubscribe = true;
                return;
            }

            if (_shouldUnsubscribe)
            {
                KpDoNotDeleteFiles.OnKpDoNotDeleteFiles -= OnEnter;
            }

            if (!_curatorLoaded)
            {
                ChatTerminalMvc.Instance.ChatTerminalController.LoadNewProfile("curator");
                _curatorLoaded = true;
            }
            
            ChatTerminalMvc.Instance.MessageSystemController.messageTyped += TransitionCheck;
        }

        private void TransitionCheck(string messageID)
        {
            if (messageID != "curatorFirstFinish")
            {
                return;
            }
            
            ChatTerminalMvc.Instance.ChatTerminalController.UnloadProfile("curator");
            
            ChangeToNextState();
        }
    }
}