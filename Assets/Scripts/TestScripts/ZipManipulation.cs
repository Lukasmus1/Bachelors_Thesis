using UnityEngine;
using Ionic.Zip;

namespace TestScripts
{
    public class ZipManipulation : MonoBehaviour
    {
        public void CreateZip()
        {
            using (var zip = new ZipFile())
            {
                string path = @"C:\Users\Lukáš Píšek\Desktop\Bachelors_Thesis\test.zip";
                zip.Password = "123";
                zip.AddDirectory(@"C:\Users\Lukáš Píšek\Desktop\Bachelors_Thesis\TestFolder");
                zip.Save(path);
                print("Zip created: " + path);
            }
        }
    }
}
