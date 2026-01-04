using Story.Commons;
using Story.Controllers;
using UnityEngine;

namespace Story.Views
{
    public class StoryManager : MonoBehaviour
    {
        //Singleton instance
        public static StoryManager Instance { get; set; }

        private StoryController _storyController = StoryMvc.Instance.StoryController;
        
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        
        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }
    }
}
