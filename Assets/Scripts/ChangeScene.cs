using UnityEngine;
using UnityEngine.SceneManagement;
public class ChangeScene : MonoBehaviour
{
    public void ChangeScenes(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void QuitGame()
    {
        // �ش� �� ����.
        Application.Quit();
    }
}
