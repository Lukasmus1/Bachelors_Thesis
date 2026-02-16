using System.Collections.Generic;
using Apps.ChatTerminal.Commons;
using UnityEngine;
using User.Commons;
using User.Models;

namespace Story.Models.Choices.ChoiceClasses
{
    public class ScreenshotChoice : IChoice
    {
        public List<ChoiceActionClass> Actions { get; set; } = new()
        {
            new ChoiceActionClass(
                0,
                () =>
                {
                    UserMvc.Instance.UserController.userModel.CopsAlignment += (int)Alignment.Cops;
                    ChatTerminalMvc.Instance.MessageSystemController.QueueSecondaryMessageFromChoice("headOfDpt", "dptScreenshotChoiceTruth");
                    
                    ChatTerminalMvc.Instance.MessageSystemController.ToggleMessagePause(false);
                }),
            new ChoiceActionClass(
                1,
                () =>
                {
                    UserMvc.Instance.UserController.userModel.CopsAlignment += (int)Alignment.AI;
                    ChatTerminalMvc.Instance.MessageSystemController.QueueSecondaryMessageFromChoice("headOfDpt", "dptScreenshotChoiceLie");
                    
                    ChatTerminalMvc.Instance.MessageSystemController.ToggleMessagePause(false);
                })
        };
    }
}