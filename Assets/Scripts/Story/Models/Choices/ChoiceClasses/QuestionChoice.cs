using System.Collections.Generic;
using Apps.ChatTerminal.Commons;
using User.Commons;
using User.Models;

namespace Story.Models.Choices.ChoiceClasses
{
    public class QuestionChoice : IChoice
    {
        public List<ChoiceActionClass> Actions { get; set; } = new()
        {
            new ChoiceActionClass(
            0,
            () =>
            {
                UserMvc.Instance.UserController.IncreaseCopsAlignment(Alignment.QuestionAI);
                
                ChatTerminalMvc.Instance.MessageSystemController.QueueSecondaryMessageFromChoice("curator", "curatorQuestionAI");
                ChatTerminalMvc.Instance.MessageSystemController.ToggleMessagePause(false);
                
                
            }),
            new ChoiceActionClass(
            1,
            () =>
            {
                ChatTerminalMvc.Instance.MessageSystemController.QueueSecondaryMessageFromChoice("curator", "curatorQuestionNeutral");
                ChatTerminalMvc.Instance.MessageSystemController.ToggleMessagePause(false);
                
                
            }),
            new ChoiceActionClass(
            2,
            () =>
            {
                UserMvc.Instance.UserController.IncreaseCopsAlignment(Alignment.QuestionAIHate);
                
                ChatTerminalMvc.Instance.MessageSystemController.QueueSecondaryMessageFromChoice("curator", "curatorQuestionHateAI");
                ChatTerminalMvc.Instance.MessageSystemController.ToggleMessagePause(false);
                
                
            })
        };
        
    }
}