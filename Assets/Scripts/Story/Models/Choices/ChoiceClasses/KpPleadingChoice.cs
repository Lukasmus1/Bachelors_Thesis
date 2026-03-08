using System.Collections.Generic;
using Apps.ChatTerminal.Commons;
using UnityEngine;
using User.Commons;
using User.Models;

namespace Story.Models.Choices.ChoiceClasses
{
    public class KpPleadingChoice : IChoice
    {
        public List<ChoiceActionClass> Actions { get; set; } = new()
        {
            new ChoiceActionClass(
                0,
                () =>
                {
                    ChatTerminalMvc.Instance.ChatTerminalController.QueueSecondaryMessage("kp", "kpPleadingHelp", true);

                    ChatTerminalMvc.Instance.MessageSystemController.ToggleMessagePause(false);
                }),
            new ChoiceActionClass(
                1,
                () =>
                {
                    ChatTerminalMvc.Instance.ChatTerminalController.QueueSecondaryMessage("kp", "kpPleadingDelete", true);

                    ChatTerminalMvc.Instance.MessageSystemController.ToggleMessagePause(false);
                })
        };
    }
}