#if UNITY_EDITOR

using UnityEditor;

/// <summary>
/// エディタ再生前にアセットを更新します。
/// </summary>
public class AutoCompiler
{
    [InitializeOnLoadMethod]
    public static void Run()
    {
        EditorApplication.playModeStateChanged += (PlayModeStateChange state) =>
        {
            if (state == PlayModeStateChange.ExitingEditMode)
            {
                AssetDatabase.Refresh();
            }
        };
    }
}

#endif