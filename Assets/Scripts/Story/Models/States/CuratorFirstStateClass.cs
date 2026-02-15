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
        public override int NextState => (int)StatesEnum.NewFiles;

        private bool _shouldUnsubscribe = false;
        
        public override void OnEnter()
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
            
            ChatTerminalMvc.Instance.ChatTerminalController.LoadNewProfile("curator");
            
            LoadFromState();
        }

        public override void OnExit()
        {
            ChatTerminalMvc.Instance.MessageSystemController.messageTyped -= TransitionCheck;
        }

        public override void LoadFromState()
        {
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