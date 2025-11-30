using System;
using Apps.ChatTerminal.Commons;
using Apps.ChatTerminal.Views;
using Apps.FileManager.Commons;
using Apps.FileManager.Views;
using UnityEngine;

/// <summary>
/// This class links all the necessary script references for the game.
/// Sometimes the objects are not enabled by default and can't pass their references, hence this linker.
/// </summary>
public class ScriptReferenceLinker : MonoBehaviour
{
    //Chat terminal app
    public GameObject chatTerminalApp;
    [SerializeField] private ChatTerminalView chatTerminalView;
    [SerializeField] private MessageSystemView messageSystemView;
    
    //File loader app
    public GameObject fileLoaderApp;
    [SerializeField] private FileLoaderView fileLoaderView;
    
    //File viewer app
    public GameObject fileViewerApp;
    
    private void Awake()
    {
        //Chat terminal app
        ChatTerminalMvc.Instance.ChatTerminalController.SetChatTerminalView(chatTerminalView);
        ChatTerminalMvc.Instance.MessageSystemController.SetView(messageSystemView);
        
        //File loader app
        FileLoaderMvc.Instance.FileLoaderController.SetFileLoaderView(fileLoaderView);
    }

    /// <summary>
    /// This is mostly used because I can't reference by "FindGameObjectWithTag".
    /// </summary>
    /// <param name="appTag">Tag of the app to open</param>
    /// <returns>GameObject reference to the app</returns>
    public GameObject GetApp(string appTag)
    {
        return appTag switch
        {
            "ChatTerminal" => chatTerminalApp,
            "FileLoader" => fileLoaderApp,
            "FileViewer" => fileViewerApp,
            _ => null
        };
    }
}