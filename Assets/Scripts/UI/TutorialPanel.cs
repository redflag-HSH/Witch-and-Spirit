using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPanel : MonoBehaviour
{
    public TutorialPanel NextPanel;
    private float timer = 0f;

    private void OnEnable()
    {
        timer = 0f;
    }

    private void Update()
    {
        timer += Time.unscaledDeltaTime;
        if((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) && timer > 0.5f)
        {
            gameObject.SetActive(false);
            if(NextPanel != null)
            {
                NextPanel.gameObject.SetActive(true);
            }
            else
            {
                TutorialManager.Instance.Hide();
            }
        }
    }
}
