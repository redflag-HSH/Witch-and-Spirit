using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingCredit : MonoBehaviour
{
    public float time;

    private float timer = 0f;

    private void Update()
    {
        if((timer += Time.deltaTime) > time)
        {
            SceneManager.LoadSceneAsync("StartScene");
        }
    }
}
