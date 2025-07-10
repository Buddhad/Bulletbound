using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class QuitGame : MonoBehaviour
{
    public void Quit()
    {
#if UNITY_EDITOR
        // Stop playing in the editor
        EditorApplication.isPlaying = false;
#elif UNITY_WEBGL
        // WebGL can't quit, but you can redirect or show a message
        Debug.Log("Quit requested in WebGL build.");
        // Optionally: Application.OpenURL("https://your-website.com");
#else
        // Quit for standalone builds (Windows, Mac, etc.)
        Application.Quit();
#endif
    }
}
