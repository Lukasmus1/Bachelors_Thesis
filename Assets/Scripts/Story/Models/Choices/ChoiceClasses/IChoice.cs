using System;
using System.Collections.Generic;

namespace Story.Models.Choices.ChoiceClasses
{
    public interface IChoice
    {
        public List<ChoiceActionClass> Actions { get; set; }
    }
}