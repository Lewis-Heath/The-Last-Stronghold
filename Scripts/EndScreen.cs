using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndScreen : MonoBehaviour
{
    [SerializeField] private Text m_RoundsSurvivedText;
    [SerializeField] private Text m_EnemiesKilledText;
    [SerializeField] private GameObject m_GameManager;

    private void OnEnable()
    {
        GameObject[] turrets = GameObject.FindGameObjectsWithTag("Turret");
        for (int i = 0; i < turrets.Length; i++)
        {
            turrets[i].GetComponent<Turret>().Destroy();
        }
        m_RoundsSurvivedText.text = m_GameManager.GetComponent<WaveSpawner>().GetRound().ToString();
        m_EnemiesKilledText.text = m_GameManager.GetComponent<GameManager>().GetEnemiesKilled().ToString();
    }

    public void RetrySelected()
    {
        Debug.Log("Playing");
        SceneManager.LoadScene(1);
    }

    public void MenuSelected()
    {
        Debug.Log("Menu");
        SceneManager.LoadScene(0);
    }
}
