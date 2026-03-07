using System.Collections.Generic;
using Apps.ChatTerminal.Commons;
using User.Commons;
using User.Models;

namespace Story.Models.Choices.ChoiceClasses
{
    public class HelpChoice : IChoice
    {
        public List<ChoiceActionClass> Actions { get; set; } = new()
        {
            new ChoiceActionClass(
                0,
                () =>
                {
                    ChatTerminalMvc.Instance.MessageSystemController.QueueSecondaryMessageFromChoice("headOfDpt", "dptChoiceHelp");
                    ChatTerminalMvc.Instance.MessageSystemController.ToggleMessagePause(false);
                    
                    UserMvc.Instance.UserController.SetPersistentData(UserDataType.LastHelpChoiceHelp, true);
                }),
            new ChoiceActionClass(
                1,
                () =>
                {
                    ChatTerminalMvc.Instance.MessageSystemController.QueueSecondaryMessageFromChoice("headOfDpt", "dptChoiceIgnore");
                    ChatTerminalMvc.Instance.MessageSystemController.ToggleMessagePause(false);
                    
                    UserMvc.Instance.UserController.SetPersistentData(UserDataType.LastHelpChoiceHelp, false);
                })
        };
    }
}