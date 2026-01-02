using System.IO;
using TMPro;
using UnityEngine;

namespace Apps.Autostereogram.Models
{
    public class AutostereogramModel
    {
        //Constants
        private const int GRAYSCALE_TEXTURE_WIDTH = 704;
        private const int GRAYSCALE_TEXTURE_HEIGHT = 300;
        //25% offsets for autostereogram pattern
        private const int REPEATING_PATTERN_COUNT = 5;
        private const int WIDTH_OFFSET = GRAYSCALE_TEXTURE_WIDTH / (REPEATING_PATTERN_COUNT - 1);
        private const int HEIGHT_OFFSET = GRAYSCALE_TEXTURE_HEIGHT / (REPEATING_PATTERN_COUNT - 1);
        //Default font size
        private const int DEFAULT_FONT_SIZE = 40;
        
        /// <summary>
        /// Creates a 2D texture representing the autostereogram of the provided text using the specified font.
        /// </summary>
        /// <param name="inputString">Text to be encoded in the autostereogram</param>
        /// <param name="font">Font used for the text</param>
        /// <returns>Autostereogram as Texture2D</returns>
        public Texture2D CreateAutostereogram(string inputString, TMP_FontAsset font)
        {
            //Final autostereogram texture
            var mainTexture = new Texture2D(GRAYSCALE_TEXTURE_WIDTH + WIDTH_OFFSET, GRAYSCALE_TEXTURE_HEIGHT)
            {
                filterMode = FilterMode.Point
            };

            //Get pattern texture
            Color[] patternPixels = GenerateAutostereogramPattern(WIDTH_OFFSET, GRAYSCALE_TEXTURE_HEIGHT);
            
            //Generate greyscale texture from input string
            Texture2D greyscaleTexture = GenerateGrayscale(inputString, font);
            
            //Initial pattern fill
            mainTexture.SetPixels(0, 0, WIDTH_OFFSET, GRAYSCALE_TEXTURE_HEIGHT, patternPixels);

            //Pre-allocate array for row pixels
            var rowPixels = new Color[GRAYSCALE_TEXTURE_WIDTH];

            //Fill the rest of the texture with shifted pixels based on depth from greyscale texture
            for (int x = WIDTH_OFFSET; x < GRAYSCALE_TEXTURE_WIDTH + WIDTH_OFFSET; x++)
            {
                for (var y = 0; y < GRAYSCALE_TEXTURE_HEIGHT; y++)
                {
                    //Get depth from grayscale texture
                    float depth = greyscaleTexture.GetPixel(x - WIDTH_OFFSET, y).grayscale;
        
                    //Calculate shift based on depth
                    int shift = Mathf.RoundToInt(depth * 5);
        
                    //Copy pixel from already generated part of texture
                    Color pixelColor = mainTexture.GetPixel(x - WIDTH_OFFSET + shift, y);
                    mainTexture.SetPixel(x, y, pixelColor);
                }
            }
            mainTexture.Apply();
            
            return mainTexture;
        }

        /// <summary>
        /// Generates a grayscale texture from the input string using TextMeshPro.
        /// </summary>
        /// <param name="input">Text to be rendered in grayscale</param>
        /// <param name="font">Font to be used</param>
        /// <returns>grayscale Texture2D</returns>
        private static Texture2D GenerateGrayscale(string input, TMP_FontAsset font)
        {
            //If the input is longer than 8 characters, reduce font size by 5 for each additional character
            int fontSize = input.Length > 8 ? DEFAULT_FONT_SIZE - (input.Length - 8) * 5 : DEFAULT_FONT_SIZE;
            
            //Create temporary GameObject with TMP
            var parent = new GameObject("TMPObject");
            var tmp = parent.AddComponent<TextMeshPro>();
            tmp.fontStyle = FontStyles.Bold;
            tmp.font = font;
            tmp.fontSize = fontSize;
            tmp.text = input;
            tmp.color = Color.white;
            tmp.alignment = TextAlignmentOptions.Center;

            //Force rebuild
            tmp.ForceMeshUpdate();

            //Render texture
            var rt = new RenderTexture(GRAYSCALE_TEXTURE_WIDTH, GRAYSCALE_TEXTURE_HEIGHT, 24);
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
            var tex = new Texture2D(GRAYSCALE_TEXTURE_WIDTH, GRAYSCALE_TEXTURE_HEIGHT, TextureFormat.RGBA32, false);
            tex.ReadPixels(new Rect(0, 0, GRAYSCALE_TEXTURE_WIDTH, GRAYSCALE_TEXTURE_HEIGHT), 0, 0);
            tex.Apply();
            RenderTexture.active = null;

            //Clean up
            Object.Destroy(parent);
            Object.Destroy(tempCam.gameObject);
            Object.Destroy(rt);
            
            return tex;
        }

        /// <summary>
        /// Created a random pattern of pixels for the autostereogram background.
        /// </summary>
        /// <param name="width">Width of the pattern</param>
        /// <param name="height">Height of the pattern</param>
        /// <returns>Pixels with random colors</returns>
        private static Color[] GenerateAutostereogramPattern(int width, int height)
        {
            var patternPixels = new Color[width * height];

            //Fill with random colors
            for (var i = 0; i < patternPixels.Length; i++)
            {
                patternPixels[i] = Random.ColorHSV(0f, 1f, 0f, 1f, 0f, 1f, 1f, 1f);
            }
            
            return patternPixels;
        }
    }
}
