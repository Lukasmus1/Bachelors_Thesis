using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Debug = System.Diagnostics.Debug;

namespace DesktopGeneration
{
    public class WallpaperGenerator
    {
        private List<string> _wallpapersPaths = new();
        
        private RawImage _wallpaperImage;
        
        public WallpaperGenerator(RawImage wallpaperImage)
        {
            _wallpaperImage = wallpaperImage;
            PopulateWallpapers();
        }

        public void SetUserWallpaper()
        {
            //Using PowerShell to get the current user's wallpaper path
            var psi = new ProcessStartInfo
            {
                FileName = "powershell",
                //NoProfile -> doesn't load the user's profile, which speeds up the process
                //Command -> executes the command in PowerShell
                //Get-ItemProperty -> gets the property of the registry key
                //HKCU:\Control Panel\Desktop -> the registry key for the current user's desktop settings
                //WallPaper -> the property that contains the wallpaper path
                Arguments = "-NoProfile -Command \"(Get-ItemProperty 'HKCU:\\Control Panel\\Desktop').WallPaper\"",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            //Starting the process and reading the output
            var process = Process.Start(psi);
            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            SetWallpaper(output.Trim());
        }
        
        public void SetRandomWallpaper()
        {
            //Getting a random index 
            int randomIndex = Random.Range(0, _wallpapersPaths.Count);
            
            //Returning a random wallpaper path
            string randomPath = _wallpapersPaths[randomIndex];
            
            SetWallpaper(randomPath);
        }
        
        private void SetWallpaper(string wallpaperPath)
        {
            //Get image bytes and create a texture
            byte[] imageBytes= File.ReadAllBytes(wallpaperPath);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(imageBytes);
            
            //Setting the texture to the RawImage
            _wallpaperImage.texture = texture;
            
            //Setting the wallpaper to the desktop instance
            Desktop.Instance.Wallpaper = texture;
        }
        
        private void PopulateWallpapers()
        {
            //Getting all files from a directory
            DirectoryInfo directoryInfo = new("Assets/Images/Desktop/DesktopWallpapers");
            foreach (FileInfo file in directoryInfo.GetFiles())
            {
                //Populating wallpaper paths
                _wallpapersPaths.Add(file.FullName);
            }   
        }
    }
}
