using System;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

[InitializeOnLoad]
internal static class RecoveryPlayModeProbe
{
    private const string Active = "Xiuzhenjie.RecoveryPlayMode.Active";
    private const string Deadline = "Xiuzhenjie.RecoveryPlayMode.Deadline";
    private const string Entered = "Xiuzhenjie.RecoveryPlayMode.Entered";

    static RecoveryPlayModeProbe()
    {
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        EditorApplication.update += Update;
    }

    public static void Run()
    {
        EditorSceneManager.OpenScene("Assets/Scenes/StartLoadScene.unity", OpenSceneMode.Single);
        SessionState.SetBool(Active, true);
        SessionState.SetBool(Entered, false);
        SessionState.SetString(Deadline, DateTime.UtcNow.AddSeconds(20).Ticks.ToString());
        EditorApplication.EnterPlaymode();
    }

    private static void OnPlayModeStateChanged(PlayModeStateChange state)
    {
        if (!SessionState.GetBool(Active, false))
            return;
        if (state == PlayModeStateChange.EnteredPlayMode)
        {
            SessionState.SetBool(Entered, true);
            Debug.Log("RECOVERY_PLAY_MODE_ENTERED " + SceneManager.GetActiveScene().name);
            SceneManager.sceneLoaded += (scene, mode) => Debug.Log("RECOVERY_PLAY_SCENE " + scene.name);
        }
        if (state == PlayModeStateChange.EnteredEditMode)
        {
            SessionState.SetBool(Active, false);
            EditorApplication.Exit(SessionState.GetBool(Entered, false) ? 0 : 1);
        }
    }

    private static void Update()
    {
        if (!SessionState.GetBool(Active, false) || !long.TryParse(SessionState.GetString(Deadline, ""), out var deadline)
            || DateTime.UtcNow.Ticks < deadline)
            return;
        if (!EditorApplication.isPlaying)
        {
            Debug.LogError("RECOVERY_PLAY_MODE_TIMEOUT");
            EditorApplication.Exit(1);
            return;
        }
        var scene = SceneManager.GetActiveScene();
        bool loginActive = UnityEngine.Object.FindObjectsOfType<XiuZhenJie.SignInPanel>().Any(panel => panel.isActiveAndEnabled);
        Debug.Log("RECOVERY_PLAY_MODE_LOGIN_UI=" + loginActive);
        Debug.Log("RECOVERY_PLAY_MODE_FINAL scene=" + scene.name + " roots=" + string.Join(",", scene.GetRootGameObjects().Select(obj => obj.name)));
        SessionState.EraseString(Deadline);
        EditorApplication.ExitPlaymode();
    }
}
