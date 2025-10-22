using UnityEngine;

namespace Story
{
    public class StoryProgression : MonoBehaviour
    {
        public static StoryProgression Instance;
        
        private void Awake()
        {
            if (Instance == null || Instance == this)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
