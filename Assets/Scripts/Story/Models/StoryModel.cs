using System;
using System.Collections.Generic;
using System.Linq;
using Commons;
using Story.Models.States;
using UnityEngine;
using User.Models;

namespace Story.Models
{
    [Serializable]
    public class StoryModel
    {
        private StateClass _currentStateClass;
        public StateClass CurrentStateClass
        {
            get => _currentStateClass;
            set
            {
                if (_currentStateClass != value)
                {
                    _currentStateClass?.OnExit();
                    Debug.Log($"Entering {value.State}");
                    value.OnEnter();
                }
                _currentStateClass = value;
            }
        }
        
        public Endings Ending { get; set; } = Endings.None;
        
        public void Init()
        {
            CurrentStateClass = new StartStateClass();
        }

        public void LoadFromState()
        {
            _currentStateClass?.LoadFromState();
        } 
        
        public int GetExtremeAlignment(bool maximumAlignment)
        {
            List<Alignment> alignments = Enum.GetValues(typeof(Alignment)).OfType<Alignment>().ToList();
            List<int> al = alignments.Select(x => (int)x).ToList();
            
            return maximumAlignment ? Tools.GetMaxSumOfList(al) : Tools.GetMinSumOfList(al);
        }

        public void SetEnding(Endings ending)
        {
            if (Ending != Endings.None)
            {
                Debug.LogError("Ending has already been set. Cannot change ending.");
                return;
            }
            
            Ending = ending;
        }
    }
}