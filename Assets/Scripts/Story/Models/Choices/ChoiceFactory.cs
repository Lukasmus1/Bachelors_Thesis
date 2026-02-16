using System;
using System.Collections.Generic;
using Story.Models.Choices.ChoiceClasses;

namespace Story.Models.Choices
{
    public static class ChoiceFactory
    {
        public static List<ChoiceActionClass> GetChoiceClass(string choiceID)
        {
            return choiceID switch
            {
                "dptScreenshotChoice" => new ScreenshotChoice().Actions,
                _ => throw new Exception("choiceID does not have a corresponding ChoiceClass")
            };
        }
    }
}