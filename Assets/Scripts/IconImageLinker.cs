using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class to link app sprites to the context.
/// </summary>
public class IconImageLinker : MonoBehaviour
{
    public static readonly Dictionary<string, Sprite> AppIconDictionary = new();

    [SerializeField] private Sprite fileManager;
    [SerializeField] private Sprite fileViewer;
    [SerializeField] private Sprite autostereogramSolver;
    [SerializeField] private Sprite chatTerminal;
    [SerializeField] private Sprite vigenereCipher;
    [SerializeField] private Sprite virusFinder;
    [SerializeField] private Sprite compilationHelper;

    private void Awake()
    {
        AppIconDictionary.Clear();
        AppIconDictionary.Add("FileManager", fileManager);
        AppIconDictionary.Add("FileViewer", fileViewer);
        AppIconDictionary.Add("AutostereogramSolver", autostereogramSolver);
        AppIconDictionary.Add("ChatTerminal", chatTerminal);
        AppIconDictionary.Add("CipherSolver", vigenereCipher);
        AppIconDictionary.Add("VirusFinder", virusFinder);
        AppIconDictionary.Add("CompilationHelper", compilationHelper);
    }
}
