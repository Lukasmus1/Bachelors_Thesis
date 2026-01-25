using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class to link app sprites to the context.
/// </summary>
public class IconImageLinker : MonoBehaviour
{
    public static readonly Dictionary<string, Sprite> appIconDictionary = new();

    [SerializeField] private Sprite fileManager;
    [SerializeField] private Sprite fileViewer;
    [SerializeField] private Sprite autostereogramSolver;
    [SerializeField] private Sprite chatTerminal;
    [SerializeField] private Sprite vigenereCipher;

    private void Awake()
    {
        appIconDictionary.Clear();
        appIconDictionary.Add("FileManager", fileManager);
        appIconDictionary.Add("FileViewer", fileViewer);
        appIconDictionary.Add("AutostereogramSolver", autostereogramSolver);
        appIconDictionary.Add("ChatTerminal", chatTerminal);
        appIconDictionary.Add("CipherSolver", vigenereCipher); 
    }
}
