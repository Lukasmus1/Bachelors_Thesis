using UnityEditor;
using UnityEditor.SceneManagement;

namespace Editor
{
    /// <summary>
    /// Class that automatically loads a specific scene when entering Play Mode in the Unity Editor.
    /// </summary>
    [InitializeOnLoad]
    public static class SceneAutoLoader
    {
        static SceneAutoLoader()
        {
            EditorApplication.playModeStateChanged += OnPlayModeChanged;
        }

        private static void OnPlayModeChanged(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.ExitingEditMode)
            {
                EditorSceneManager.playModeStartScene = 
                    AssetDatabase.LoadAssetAtPath<SceneAsset>("Assets/Scenes/Bootstrapper.unity");
            }
        }
    }
}