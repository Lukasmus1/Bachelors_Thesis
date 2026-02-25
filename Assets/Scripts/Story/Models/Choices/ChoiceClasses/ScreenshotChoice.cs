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
                    UserMvc.Instance.UserController.SetPersistentData(UserDataType.FirstChoiceSideWithCops, true);
                    ChatTerminalMvc.Instance.MessageSystemController.QueueSecondaryMessageFromChoice("headOfDpt", "dptScreenshotChoiceTruth");
                    
                    ChatTerminalMvc.Instance.MessageSystemController.ToggleMessagePause(false);
                    
                    ChatTerminalMvc.Instance.ChatTerminalController.QueueSecondaryMessage("kp", "kpFirstChoiceTruth", false);
                }),
            new ChoiceActionClass(
                1,
                () =>
                {
                    UserMvc.Instance.UserController.userModel.CopsAlignment += (int)Alignment.AI;
                    UserMvc.Instance.UserController.SetPersistentData(UserDataType.FirstChoiceSideWithCops, false);
                    ChatTerminalMvc.Instance.MessageSystemController.QueueSecondaryMessageFromChoice("headOfDpt", "dptScreenshotChoiceLie");
                    
                    ChatTerminalMvc.Instance.MessageSystemController.ToggleMessagePause(false);
                    
                    ChatTerminalMvc.Instance.ChatTerminalController.QueueSecondaryMessage("kp", "kpFirstChoiceLie", false);
                })
        };
    }
}