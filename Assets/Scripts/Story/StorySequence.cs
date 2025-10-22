using System.Collections.Generic;
using UnityEngine;

namespace Story
{
    [CreateAssetMenu(fileName = "StorySequence", menuName = "Progression/Story Sequence", order = 0)]
    public class StorySequence : ScriptableObject
    {
        [Tooltip("Ordered list of objectives that define the story progression.")]
        public List<StoryStep> steps = new();
    }
}