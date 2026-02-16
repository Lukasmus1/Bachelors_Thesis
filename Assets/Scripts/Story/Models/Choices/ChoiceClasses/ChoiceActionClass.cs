using System;
using UnityEngine.Events;

namespace Story.Models.Choices.ChoiceClasses
{
    public class ChoiceActionClass
    {
        public int ChoiceID { get; set; }
        public UnityAction ChoiceAction { get; set; }
        
        public ChoiceActionClass(int choiceID, UnityAction choiceAction)
        {
            ChoiceID = choiceID;
            ChoiceAction = choiceAction;
        }
    }
}