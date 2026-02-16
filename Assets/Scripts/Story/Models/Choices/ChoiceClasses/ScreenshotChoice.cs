using System;
using System.Collections.Generic;
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
                    Debug.Log("Cops alignment increased by " + (int)Alignment.Cops);
                }),
            new ChoiceActionClass(
                1,
                () =>
                {
                    UserMvc.Instance.UserController.userModel.CopsAlignment += (int)Alignment.AI;
                    Debug.Log("Cops alignment increased by " + (int)Alignment.AI);
                })
        };
    }
}