using Apps.ChatTerminal.Commons;
using Commons;
using Saving.Commons;

namespace Story.Models.SideActions
{
    public static class KpDoNotDeleteFiles
    {
        public static void MessagePlayer()
        {
            ChatTerminalMvc.Instance.ChatTerminalController.LoadNewProfile("kp");
            ChatTerminalMvc.Instance.ChatTerminalController.SetChatProfileMessageIndex("kp", 0);

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
                SavingMvc.Instance.SavingController.QuitAndSaveGame();
            });
            
            ChatTerminalMvc.Instance.MessageSystemController.messageTyped -= MessageTyped;
        }
    }
}