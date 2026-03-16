using System;

namespace Story.Models.States
{
    [Serializable]
    public class EndHelpCuratorStateClass : StateClass
    {
        public override int State { get; } = (int)StatesEnum.EndingFightForCurator;
        public override int NextState { get; set; } = (int)StatesEnum.Default;

        public override void OnEnter()
        {
            //Fragmented AI compiling into a secret folder, if that process completes, the player will have to disconnect from the internet.
            //By exploring and adjusting the computer's settings they will slowly uncover the locations.
            //First file will be on the desktop
            //Second file will be revealed by muting the computer's audio
            //Third file will be revealed once they change their system theme -> fallback for unactivated windows is regedit?
            //For last file, the player will have to edit a custom registry entry
            //The AI will turn on the game itself, corrupting parts of it overtime. If that completes, the game shuts down and will be unable to load again due using a save file. 
            throw new Exception("DEFAULT STATE SHOULD NOT BE USED");
        }

        public override void OnExit()
        {
            throw new Exception("DEFAULT STATE SHOULD NOT BE USED");
        }

        public override void LoadFromState()
        {
            throw new Exception("DEFAULT STATE SHOULD NOT BE USED");
        }
    }
}