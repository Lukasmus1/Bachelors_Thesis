using System;

namespace Story.Models.Actions
{
    /// <summary>
    /// This class handles the execution of various actions within the story.
    /// For reasons described in <see cref="ListOfActionsPersistent"/>
    /// </summary>
    public class ActionsClass
    {
        //Singleton instance
        private static ActionsClass _instance;
        public static ActionsClass Instance
        {
            get
            {
                _instance ??= new ActionsClass();
                return _instance;
            }
        }

        public Action actionsToPerformOnLoad;
        
        private ListOfActionsPersistent _actionsPersistent = new();
        public ListOfActionsPersistent ActionsPersistent 
        {
            get => _actionsPersistent;
            set
            {
                _actionsPersistent = value;
                LoadFromState();
            }
            
        }

        /// <summary>
        /// Method to perform actions that need to be executed on load.
        /// This exists because the LoadFromState is called when we are not on the correct scene yet.
        /// </summary>
        public void PerformActionsOnLoad()
        {
            actionsToPerformOnLoad?.Invoke();
            
            actionsToPerformOnLoad = null;
        }
        
        /// <summary>
        /// Method to load actions from persistent state.
        /// </summary>
        private void LoadFromState()
        {
            foreach (Pair action in ActionsPersistent.actions)
            {
                if (!action.value)
                {
                    continue;
                }

                actionsToPerformOnLoad += () => PerformAction(action.key);
            }
        }
        
        
        /// <summary>
        /// Performs the action specified by the ActionType enum.
        /// </summary>
        /// <param name="type">ActionType</param>
        /// <exception cref="ArgumentOutOfRangeException">Gets thrown if we use unspecified ActionType</exception>
        public void PerformAction(ActionType type)
        {
            switch (type)
            {
                case ActionType.HiddenVirus:
                    HiddenVirus.PerformHiddenVirusAction();
                    break;
                
                case ActionType.ImportantFile:
                    ImportantFile.SetupFileDeletionDetection();
                    break;
                    
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
            
            ActionsPersistent.SetAction(type, true);
        }
        
        /// <summary>
        /// Performs cleanup for the action specified by the ActionType enum.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Gets thrown if we use unspecified ActionType</exception>
        public void CleanupAction()
        {
            foreach (Pair action in ActionsPersistent.actions)
            {
                if (!action.value)
                    continue;
                
                switch (action.key)
                {
                    case ActionType.HiddenVirus:
                        HiddenVirus.CleanupHiddenVirusAction();
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}