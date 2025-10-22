using Apps.ChatTerminal.Controllers;

namespace Apps.ChatTerminal.Commons
{
    public class ChatTerminalMvc
    {
        //Singleton instance
        private static ChatTerminalMvc _instance;
        public static ChatTerminalMvc Instance
        {
            get
            {
                _instance ??= new ChatTerminalMvc();
                return _instance;
            }
        }

        public ChatTerminalController ChatTerminalController { get; set; }
        public MessageSystemController MessageSystemController { get; set; }
        
        private ChatTerminalMvc()
        {
            ChatTerminalController = new ChatTerminalController();
            MessageSystemController = new MessageSystemController();
        }
    }
}