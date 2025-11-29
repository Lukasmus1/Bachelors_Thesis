using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Apps.Autospectogram.Models
{
    public class AutospectogramGenerator : MonoBehaviour
    {
        public RawImage displayImage;
        public TMP_FontAsset font;
        
        public string inputString = "cc";
        
        //Constants
        //At least 1080p texture
        private const int GREYSCALE_TEXTURE_WIDTH = 1920;
        private const int GREYSCALE_TEXTURE_HEIGHT = 1080;
        //Default font size
        private const int DEFAULT_FONT_SIZE = 40;
        
        /// <summary>
        /// Generates a greyscale texture from the input string using TextMeshPro.
        /// </summary>
        /// <param name="input">Text to be rendered in greyscale</param>
        /// <returns>Greyscale Texture2D</returns>
        private Texture2D GenerateGreyscale(string input)
        {
            //If the input is longer than 8 characters, reduce font size by 5 for each additional character
            int fontSize = input.Length > 8 ? DEFAULT_FONT_SIZE - (input.Length - 8) * 5 : DEFAULT_FONT_SIZE;
            
            //Create temporary GameObject with TMP
            var parent = new GameObject("TMPObject");
            var tmp = parent.AddComponent<TextMeshPro>();
            tmp.font = font;
            tmp.fontSize = fontSize;
            tmp.text = input;
            tmp.color = Color.white;
            tmp.alignment = TextAlignmentOptions.Center;

            //Force rebuild
            tmp.ForceMeshUpdate();

            //Render texture
            var rt = new RenderTexture(GREYSCALE_TEXTURE_WIDTH, GREYSCALE_TEXTURE_HEIGHT, 0);
            var tempCam = new GameObject("TempCam").AddComponent<Camera>();
            tempCam.clearFlags = CameraClearFlags.SolidColor;
            tempCam.backgroundColor = Color.black;
            tempCam.orthographic = true;
            tempCam.targetTexture = rt;

            //Position text
            tmp.transform.position = Vector3.zero;
            tempCam.transform.position = new Vector3(0, 0, -5);

            //Render
            tempCam.Render();

            //Read pixels from RenderTexture to Texture2D
            RenderTexture.active = rt;
            var tex = new Texture2D(GREYSCALE_TEXTURE_WIDTH, GREYSCALE_TEXTURE_HEIGHT, TextureFormat.RGBA32, false);
            tex.ReadPixels(new Rect(0, 0, GREYSCALE_TEXTURE_WIDTH, GREYSCALE_TEXTURE_HEIGHT), 0, 0);
            tex.Apply();
            RenderTexture.active = null;

            //Clean up
            Destroy(parent);
            Destroy(tempCam.gameObject);
            Destroy(rt);
            
            displayImage.texture = tex;
            return tex;
        }

        private void Awake()
        {
            GenerateGreyscale(inputString);
        }
    }
}
