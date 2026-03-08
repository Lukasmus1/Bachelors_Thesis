using System;
using System.Collections.Generic;
using System.Linq;
using Commons;
using Story.Models.States;
using User.Models;

namespace Story.Models
{
    [Serializable]
    public class StoryModel
    {
        private StateClass currentStateClass;
        public StateClass CurrentStateClass
        {
            get => currentStateClass;
            set
            {
                if (currentStateClass != value)
                {
                    currentStateClass?.OnExit();
                    value.OnEnter();
                }
                currentStateClass = value;
            }
        }
        
        public Endings Ending { get; set; } = Endings.None;
        
        public void Init()
        {
            CurrentStateClass = new StartStateClass();
        }

        public void LoadFromState()
        {
            currentStateClass?.LoadFromState();
        } 
        
        public int GetExtremeAlignment(bool maximumAlignment)
        {
            List<Alignment> alignments = Enum.GetValues(typeof(Alignment)).OfType<Alignment>().ToList();
            List<int> al = alignments.Select(x => (int)x).ToList();
            
            return maximumAlignment ? Tools.GetMaxSumOfList(al) : Tools.GetMinSumOfList(al);
        }
    }
}