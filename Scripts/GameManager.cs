using System;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Screen[] m_Screens;
    [SerializeField] private float m_TownHealth;
    [SerializeField] private Image m_HealthBar;
    private float m_MaxTownHealth;
    private int m_EnemiesKilled;
    private ScreenType m_CurrentScreen;

    void Start()
    {
        m_MaxTownHealth = m_TownHealth;
        m_CurrentScreen = ScreenType.Playing;
        m_HealthBar.color = Color.green;
    }

    void Update()
    {
        if (m_TownHealth <= 0 || Input.GetKeyDown(KeyCode.E))
        {
            EndGame();
        }

        if(m_CurrentScreen != ScreenType.EndScreen && Input.GetKeyDown(KeyCode.P))
        {
            if (m_CurrentScreen != ScreenType.Pause)
            {
                EnableScreen("Pause");
                m_CurrentScreen = ScreenType.Pause;
                Time.timeScale = 0f;
            }
            else
            {
                DisableScreen("Pause");
                m_CurrentScreen = ScreenType.Playing;
                Time.timeScale = 1f;
            }
        }
    }

    private void SetHealthBarColour()
    {
        float healthPercent = m_TownHealth / m_MaxTownHealth;
        if (healthPercent >= 0.75f)
        {
            m_HealthBar.color = Color.green;
        }
        if (healthPercent < 0.75f && healthPercent > 0.35f)
        {
            m_HealthBar.color = Color.yellow;
        }
        if (healthPercent <= 0.35f)
        {
            m_HealthBar.color = Color.red;
        }
    }

    private void EndGame()
    {
        m_CurrentScreen = ScreenType.EndScreen;
        EnableScreen("EndScreen");
    }

    public void EnableScreen(string name)
    {
        Screen screenToEnable = Array.Find(m_Screens, screen => screen.m_Name == name);
        screenToEnable.m_GameObject.SetActive(true);
    }

    public void DisableScreen(string name)
    {
        Screen screenToDisable = Array.Find(m_Screens, screen => screen.m_Name == name);
        screenToDisable.m_GameObject.SetActive(false);
    }

    public ScreenType GetCurrentScreen()
    {
        return m_CurrentScreen;
    }

    public void SetCurrentScreen(ScreenType type)
    {
        m_CurrentScreen = type;
    }

    public void DecreaseLife()
    {
        m_TownHealth -= 10f;
        if (m_TownHealth <= 0f)
        {
            m_HealthBar.fillAmount = 0f;
        }
        else
        {
            m_HealthBar.fillAmount = m_TownHealth / m_MaxTownHealth;
        }

        SetHealthBarColour();
    }

    public void IncreaseEnemiesKilled()
    {
        m_EnemiesKilled++;
    }

    public int GetEnemiesKilled()
    {
        return m_EnemiesKilled;
    }
}
