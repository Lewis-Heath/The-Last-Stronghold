using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScreen : MonoBehaviour
{
    public void PlaySelected()
    {
        Debug.Log("Playing");
        SceneManager.LoadScene(1);
    }

    public void ControlsSelected()
    {
        Debug.Log("Controls");
        SceneManager.LoadScene(2);
    }

    public void ExitSelected()
    {
        Debug.Log("Exit");
        Application.Quit();
    }
}
