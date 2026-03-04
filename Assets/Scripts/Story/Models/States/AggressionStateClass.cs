using System;
using Apps.ChatTerminal.Commons;
using Commons;
using Desktop.Commons;
using FourthWall.Commons;

namespace Story.Models.States
{
    [Serializable]
    public class AggressionStateClass : StateClass
    {
        public override int State { get; } = (int)StatesEnum.Aggression;
        public override int NextState { get; set; } = (int)StatesEnum.Default;

        public override void OnEnter()
        {
            ChatTerminalMvc.Instance.ChatTerminalController.IncreaseChatProfileMessageIndex("curator");
            
            LoadFromState();
        }

        public override void OnExit()
        {
            throw new Exception("DEFAULT STATE SHOULD NOT BE USED");
        }

        public override void LoadFromState()
        {
            ChatTerminalMvc.Instance.MessageSystemController.messageTyped += NewFileCheck;
        }

        private void NewFileCheck(string messageID)
        {
            if (messageID != "curatorKPAggression")
            {
                return;
            }
            
            var timer = new AsyncTimer();

            _ = timer.StartTimer(1, () =>
            {
                DesktopMvc.Instance.DesktopGeneratorController.CloseAllApps();

                string files = ChatTerminalMvc.Instance.ChatTerminalController.GetSecondaryMessageGroupConcat("kp", "kpLastWarning").Remove('\n'); 
                FourthWallMvc.Instance.FileGenerationController.CreateCreepyFileSequence(files, Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\-", 1);
                
                timer.Dispose();
            });
        }
    }
}