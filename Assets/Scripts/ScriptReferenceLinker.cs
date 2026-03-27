using System;
using System.Collections.Generic;
using Apps.ChatTerminal.Commons;
using Apps.ChatTerminal.Views;
using Apps.CompilationHelper.Commons;
using Apps.CompilationHelper.Views;
using Apps.FileManager.Commons;
using Apps.FileManager.Views;
using Apps.FileUploader.Commons;
using Apps.FileUploader.Views;
using Apps.VirusFinder.Commons;
using Apps.VirusFinder.Views;
using Desktop.BottomBar.Commons;
using Desktop.BottomBar.Views;
using Desktop.Commons;
using Desktop.Views;
using Sounds.Commons;
using Sounds.Views;
using UnityEngine;

/// <summary>
/// This class links all the necessary script references for the game.
/// Sometimes the objects are not enabled by default and can't pass their references, hence this linker.
/// </summary>
public class ScriptReferenceLinker : MonoBehaviour
{
    public GameObject chatTerminalApp;
    [SerializeField] private ChatTerminalView chatTerminalView;
    [SerializeField] private MessageSystemView messageSystemView;
    
    public GameObject fileLoaderApp;
    [SerializeField] private FileLoaderView fileLoaderView;
    
    [SerializeField] private BottomBarView bottomBarView;
    
    public GameObject fileViewerApp;
    
    public GameObject autostereoApp;
    
    public GameObject cipherSolverApp;
    
    public GameObject virusFinderApp;
    [SerializeField] private VirusFinderView virusFinderView;
    
    [SerializeField] private GameObject mainCanvas;
    
    [SerializeField] private DesktopGeneratorView desktopGeneratorView;
    
    [SerializeField] private GameObject iconParent;
    
    [SerializeField] private GameObject compilationHelper;
    [SerializeField] private CompilationHelperView compilationHelperView;
    
    public GameObject fileUploaderApp;
    [SerializeField] private FileUploaderView fileUploaderView;
    
    [SerializeField] private SoundView soundView;
    
    public GameObject settingsApp;
    
    private void Awake()
    {
        //Chat terminal app
        ChatTerminalMvc.Instance.ChatTerminalController.SetChatTerminalView(chatTerminalView);
        ChatTerminalMvc.Instance.MessageSystemController.SetView(messageSystemView);
        
        //File loader app
        FileManagerMvc.Instance.FileManagerController.SetFileLoaderView(fileLoaderView);
        
        //Bottom bar
        BottomBarMvc.Instance.BottomBarController.SetBottomBarView(bottomBarView);
        
        //Virus finder app
        VirusFinderMvc.Instance.VirusFinderController.SetView(virusFinderView);
        
        //Desktop
        DesktopMvc.Instance.DesktopGeneratorController.SetDesktopView(desktopGeneratorView);
        
        //Compilation progress bar
        CompilationHelperMvc.Instance.CompilationHelperController.SetView(compilationHelperView);
        
        //File uploader app
        FileUploaderMvc.Instance.FileUploaderController.SetView(fileUploaderView);
        
        //Audio management
        SoundMvc.Instance.SoundController.SetView(soundView);
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
            "AutostereogramSolver" => autostereoApp,
            "CipherSolver" => cipherSolverApp,
            "VirusFinder" => virusFinderApp,
            "CompilationHelper" => compilationHelper,
            "FileUploader" => fileUploaderApp,
            "Settings" => settingsApp,
            _ => throw new Exception("The app tag has not been found!")
        };
    }

    /// <summary>
    /// Gets all the apps in a list, used for example to close all apps at once.
    /// </summary>
    /// <returns>List of all apps on the desktop</returns>
    public List<GameObject> GetAllApps()
    {
        return new List<GameObject>
        {
            chatTerminalApp,
            fileLoaderApp,
            fileViewerApp,
            autostereoApp,
            cipherSolverApp,
            virusFinderApp,
            compilationHelper,
            fileUploaderApp,
            settingsApp
        };
    }
    
    /// <summary>
    /// Gets the MonoBehaviour reference of this script, used for coroutines in other classes that can't inherit from MonoBehaviour.
    /// </summary>
    /// <returns>MonoBehavior instance</returns>
    public MonoBehaviour GetMonoBehavior() => this;
    
    /// <summary>
    /// Gets the desktop holder game object.
    /// </summary>
    /// <returns>Desktop holder GameObject</returns>
    public GameObject GetMainCanvas() => mainCanvas;

    /// <summary>
    /// Gets the IconParent game object.
    /// </summary>
    /// <returns>GameObject parent of icons on the desktop</returns>
    public GameObject GetIconParent() => iconParent;
}