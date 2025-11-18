using System;
using Apps.ChatTerminal.Commons;
using Apps.ChatTerminal.Views;
using UnityEngine;

/// <summary>
/// This class links all the necessary script references for the game.
/// Sometimes the objects are not enabled by default and can't pass their references, hence this linker.
/// </summary>
public class ScriptReferenceLinker : MonoBehaviour
{
    //Chat terminal app
    [SerializeField] private ChatTerminalView chatTerminalView;
    [SerializeField] private MessageSystemView messageSystemView;
    
    private void Awake()
    {
        //Chat terminal app
        ChatTerminalMvc.Instance.ChatTerminalController.SetChatTerminalView(chatTerminalView);
        ChatTerminalMvc.Instance.MessageSystemController.SetView(messageSystemView);
    }
}