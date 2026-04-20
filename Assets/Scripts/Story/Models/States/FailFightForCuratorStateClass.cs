using System;
using System.Collections;
using Apps.ChatTerminal.Commons;
using Commons;
using Desktop.Commons;
using FourthWall.Commons;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Story.Models.States
{
    [Serializable]
    public class FailFightForCuratorStateClass : StateClass
    {
        public override int State { get; } = (int)StatesEnum.FailFightForCurator;
        public override int NextState { get; set; } = (int)StatesEnum.FailFightForCuratorEnding;

        [NonSerialized] private AsyncOperation _sceneLoadOp;
        [NonSerialized] private MonoBehaviour _mb;
        
        public override void OnEnter()
        {
            DesktopMvc.Instance.DesktopGeneratorController.ToggleIcon("Compilation Helper", false);
            DesktopMvc.Instance.DesktopGeneratorController.CloseApp("CompilationHelper");
            
            //Preload the user's desktop scene
            _mb = Tools.GetScriptReferenceLinker().GetMonoBehavior();
            _mb.StartCoroutine(PreloadScene());
            DesktopMvc.Instance.DesktopGeneratorController.PreloadUserDesktop();

            ChatTerminalMvc.Instance.ChatTerminalController.QueueSecondaryMessage("curator", "curatorFailEnding", true);
            
            LoadFromState();
        }

        private IEnumerator PreloadScene()
        {
            _sceneLoadOp = SceneManager.LoadSceneAsync("UserDesktop");
            if (_sceneLoadOp == null)
            {
                throw new Exception("Scene load operation failed");
            }
            
            _sceneLoadOp.allowSceneActivation = false;

            while (_sceneLoadOp.progress < 0.9f)
            {
                yield return null;
            }
        }
        
        public override void OnExit()
        {
            ChatTerminalMvc.Instance.MessageSystemController.messageTyped -= NextMessageCheck;
            ChatTerminalMvc.Instance.MessageSystemController.messageTyped -= TransitionCheck;
            
            //Load the new scene
            _sceneLoadOp.allowSceneActivation = true;
        }

        public override void LoadFromState()
        {
            ChatTerminalMvc.Instance.MessageSystemController.messageTyped += NextMessageCheck;
            ChatTerminalMvc.Instance.MessageSystemController.messageTyped += TransitionCheck;
        }

        private void NextMessageCheck(string messageID)
        {
            if (messageID != "curatorFailEndingEnd")
                return;
            
            ChatTerminalMvc.Instance.ChatTerminalController.QueueSecondaryMessage("kp", "kpFailCuratorEnding", true);
        }

        private void TransitionCheck(string messageID)
        {
            if (messageID != "kpFailCuratorEndingEnd")
                return;
            
            var t = new AsyncTimer();
            _ = t.StartTimer(2, () =>
            {
                FourthWallMvc.Instance.CommonsController.ThrowWindowsDialog(DialogType.Info, "...is mine.", "");
                _mb.StartCoroutine(ChangeScene());
                
                t.Dispose();
            });
        }

        private IEnumerator ChangeScene()
        {
            _sceneLoadOp.allowSceneActivation = true;

            while (!_sceneLoadOp.isDone)
            {
                yield return null;
            }
            
            ChangeToNextState();
        }
    }
}