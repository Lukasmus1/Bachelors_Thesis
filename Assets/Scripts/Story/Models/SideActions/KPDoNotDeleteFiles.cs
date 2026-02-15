using System;
using Apps.ChatTerminal.Commons;
using Commons;
using Saving.Commons;
using User.Commons;
using User.Models;

namespace Story.Models.SideActions
{
    public static class KpDoNotDeleteFiles
    {
        public static Action OnKpDoNotDeleteFiles;
            
        public static void MessagePlayer()
        {
            ChatTerminalMvc.Instance.ChatTerminalController.LoadNewProfile("kp");

            ChatTerminalMvc.Instance.MessageSystemController.messageTyped += MessageTyped;
        }

        private static void MessageTyped(string messageID)
        {
            if (messageID != "kpWarning")
            {
                return;
            }
            
            //After 2 seconds, quit and save the game. This gives the player enough time to read the message.
            var timer = new AsyncTimer();
            _ = timer.StartTimer(3, () => 
            {
                UserMvc.Instance.UserController.SetPersistentData(UserDataType.DeletedVirusFile, true);
                OnKpDoNotDeleteFiles?.Invoke();
                SavingMvc.Instance.SavingController.QuitAndSaveGame();
            });
            
            ChatTerminalMvc.Instance.MessageSystemController.messageTyped -= MessageTyped;
        }
    }
}