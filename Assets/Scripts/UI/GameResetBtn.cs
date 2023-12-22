using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameResetBtn : MonoBehaviour
{
    public void Exit()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void ResetGame()
    {
        PlayerPrefs.DeleteAll();
        Exit();
    }
}
