using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AdminRightsTesting : MonoBehaviour
{
    [SerializeField] private TMP_Text debugTest;
    
    public void GetPCTime()
    {
        debugTest.text = System.DateTime.Now.ToLongTimeString();    
    }

    public void CRUDFile()
    {
        //"C:\Program Files\TestFile.txt" for example will require admin rights
        string path = @"C:\Users\kosti\TestFile.txt";
        File.Create(path).Close();
        File.WriteAllText(path, "This is written in a file");
        debugTest.text = File.ReadAllText(path);
        File.Delete(path);
    }

    public void PingGoogle()
    {
        StartCoroutine(StartPing());
    }

    private IEnumerator StartPing()
    {
        Ping p = new Ping("8.8.8.8");
        while (!p.isDone)
        {
            yield return new WaitForSeconds(0.05f);
        }
        
        debugTest.text = $"Time to ping '8.8.8.8': {p.time.ToString()}ms";
        
    }

    public void GetPCName()
    {
        Process process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "powershell.exe",
                Arguments = "Get-WmiObject Win32_UserAccount | Where-Object { $_.Name -eq $env:USERNAME } | Select-Object -ExpandProperty FullName",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };

        process.Start();
        string result = process.StandardOutput.ReadToEnd();
        process.WaitForExit();
        
        debugTest.text = Environment.MachineName + " " + Environment.UserName + " " + result.Trim();
    }
}
