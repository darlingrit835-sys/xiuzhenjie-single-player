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
    private const string Seen = "Xiuzhenjie.RecoveryPlayMode.Seen";
    private const double WindowSeconds = 90d;

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
        SessionState.SetBool(Seen, false);
        SessionState.SetString(Deadline, string.Empty);
        EditorApplication.EnterPlaymode();
    }

    private static void OnPlayModeStateChanged(PlayModeStateChange state)
    {
        if (!SessionState.GetBool(Active, false))
            return;
        if (state == PlayModeStateChange.EnteredPlayMode)
        {
            SessionState.SetBool(Entered, true);
            SessionState.SetString(Deadline, DateTime.UtcNow.AddSeconds(WindowSeconds).Ticks.ToString());
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
        if (!SessionState.GetBool(Active, false) || !EditorApplication.isPlaying)
            return;

        var panels = UnityEngine.Object.FindObjectsOfType<XiuZhenJie.SignInPanel>();
        bool loginActive = panels.Any(panel => panel.isActiveAndEnabled);
        if (loginActive && !SessionState.GetBool(Seen, false))
        {
            SessionState.SetBool(Seen, true);
            Debug.Log("RECOVERY_PLAY_MODE_LOGIN_UI=True t=" + Time.realtimeSinceStartup.ToString("F1"));
            Finish();
            return;
        }

        if (!long.TryParse(SessionState.GetString(Deadline, ""), out var deadline) || DateTime.UtcNow.Ticks < deadline)
            return;

        var loadPanel = UnityEngine.Object.FindObjectsByType<XiuZhenJie.StartLoadScenePanel>(FindObjectsSortMode.None);
        Debug.Log("RECOVERY_PLAY_MODE_LOGIN_UI=" + loginActive
            + " panels=" + panels.Length
            + " loadPanel=" + loadPanel.Length
            + " t=" + Time.realtimeSinceStartup.ToString("F1")
            + " activeScene=" + SceneManager.GetActiveScene().name);
        Finish();
    }

    private static void Finish()
    {
        var scene = SceneManager.GetActiveScene();
        Debug.Log("RECOVERY_PLAY_MODE_FINAL scene=" + scene.name
            + " roots=" + string.Join(",", scene.GetRootGameObjects().Select(obj => obj.name)));
        SessionState.EraseString(Deadline);
        SessionState.EraseBool(Active);
        EditorApplication.ExitPlaymode();
    }
}
