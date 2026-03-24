using System;
using System.Collections;
using Commons;
using Desktop.Commons;
using FourthWall.Commons;
using UnityEngine;

namespace Story.Models.States
{
    [Serializable]
    public class CuratorEndingLastChance : StateClass
    {
        public override int State { get; } = (int)StatesEnum.FightForCuratorLastChance;
        public override int NextState { get; set; } = (int)StatesEnum.Default;

        private bool _shouldCoroutineStop;
        
        public override void OnEnter()
        {
            DesktopMvc.Instance.DesktopGeneratorController.ToggleIcon("Compilation Helper", false);
            DesktopMvc.Instance.DesktopGeneratorController.CloseApp("CompilationHelper");

            //Several seconds timer for disconnecting from the net
            //5 seconds for reading
            //25 seconds for disconnecting
            //30 seconds total
            
            LoadFromState();
        }

        public override void OnExit()
        { }

        public override void LoadFromState()
        {
            //If the user disconnects in time
            FourthWallMvc.Instance.UserInformationController.SetupInternetDisconnectDetection(() =>
            {
                _shouldCoroutineStop = true;
                NextState = (int)StatesEnum.SuccessFightForCurator;
                ChangeToNextState();
            });
            
            DateTime currentDate = DateTime.Now;
            
            //Countdown 30 seconds to fail state
            MonoBehaviour mb = Tools.GetScriptReferenceLinker().GetMonoBehavior();
            _shouldCoroutineStop = false;
            mb.StartCoroutine(CheckSecondsPassed(30, currentDate, () =>
            {
                FourthWallMvc.Instance.UserInformationController.StopRunningInternetDisconnectDetection();
                NextState = (int)StatesEnum.FailFightForCurator;
                ChangeToNextState();
            }));
            
            FourthWallMvc.Instance.CommonsController.ThrowWindowsDialog(DialogType.Error, "YOU LET HIM FINISH THE COMPILATION! DISCONNECT FORM THE INTERNET NOW! IT'S YOUR LAST CHANCE!",
                "DISCONNECT FROM THE INTERNET");
        }

        /// <summary>
        /// Checks if seconds time passed from the given date, if so, invokes the callback.
        /// </summary>
        /// <param name="seconds">How many seconds should I check</param>
        /// <param name="currentDate">What is the date to check against</param>
        /// <param name="callback">What should happen after seconds time</param>
        /// <returns>IEnumerator</returns>
        private IEnumerator CheckSecondsPassed(int seconds, DateTime currentDate, Action callback)
        {
            while (currentDate.AddSeconds(seconds) > DateTime.Now)
            {
                if (_shouldCoroutineStop)
                    yield break;
                yield return new WaitForSeconds(0.2f); // Slight delay to not waste more resources than needed, .2 seconds is negligible.
            }
            
            if (_shouldCoroutineStop)
                yield break;
            callback?.Invoke();
        }
    }
}