using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveSpawner : MonoBehaviour
{
    public Text m_WaveCounterText;
    public Text m_EnemyCounterText;
    public Text m_RoundCountdownText;
    public Transform m_EnemyPrefab;
    private GameObject m_SpawnPoints;
    public List<Transform> m_SpawnPointsList = new List<Transform>();
    private int m_RandomNum;
    public float m_TimeBetweenSpawns = 5f;
    private float m_Countdown = 0f;
    private int m_GroupSpawnCount = 1;
    public int m_EnemyCount;
    private int m_StartingEnemyCount;
    private float m_NewEnemyCount;
    private int m_WaveCount = 1;
    private float m_TimeBetweenWaves = 10f;
    public float m_Difficulty = 1.25f;
    private bool m_RoundChanged = false;

    void Start()
    {
        m_StartingEnemyCount = m_EnemyCount;
    }

    void Update()
    {
        if(m_Countdown <= 0f && m_EnemyCount > 0)
        {
            SpawnWave();
            m_Countdown = m_TimeBetweenSpawns;
            m_EnemyCounterText.text = m_EnemyCount.ToString();
        }

        else if (m_EnemyCount == 0 && m_TimeBetweenWaves <= 0f)
        {
            m_RoundCountdownText.text = " ";
            m_RoundChanged = false;
            m_TimeBetweenWaves = 10f;
            m_GroupSpawnCount = 1;
            m_NewEnemyCount = m_StartingEnemyCount * m_Difficulty;
            m_EnemyCount = Mathf.RoundToInt(m_NewEnemyCount);
            m_StartingEnemyCount = m_EnemyCount;
            m_EnemyCounterText.text = m_EnemyCount.ToString();
        }

        else if (m_EnemyCount == 0 && m_TimeBetweenWaves > 0f && !enemiesActive())
        {
            m_TimeBetweenWaves -= Time.deltaTime;
            float displayTimeBetweenWaves = m_TimeBetweenWaves;
            m_RoundCountdownText.text = Mathf.RoundToInt(displayTimeBetweenWaves).ToString();
            if(!m_RoundChanged)
            {
                m_WaveCount++;
                m_WaveCounterText.text = m_WaveCount.ToString();
                m_RoundChanged = true;
            }
        }
        m_Countdown -= Time.deltaTime;
    }

    void SpawnWave()
    {
        int spawnCount;

        if(m_EnemyCount - m_GroupSpawnCount >= 0)
        {
            spawnCount = m_GroupSpawnCount;
        }
        else
        {
            spawnCount = m_EnemyCount;
        }
        for (int i = 0; i < spawnCount; i++)
        {
            SpawnEnemy();
        }

        m_EnemyCount -= spawnCount;
        m_GroupSpawnCount++;
    }

    void SpawnEnemy()
    {
        m_SpawnPoints = GameObject.Find("SpawnPoints");
        foreach (Transform t in m_SpawnPoints.GetComponentInChildren<Transform>())
        {
            m_SpawnPointsList.Add(t);
        }
        m_RandomNum = Random.Range(0, 10);
        Instantiate(m_EnemyPrefab, m_SpawnPointsList[m_RandomNum].position, m_SpawnPointsList[m_RandomNum].rotation);
    }

    bool enemiesActive()
    {
        GameObject[] activeEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in activeEnemies)
        {
            return true;
        }
        return false;
    }

    public int GetRound()
    {
        return m_WaveCount;
    }

    public string GetDifficulty()
    {
        string temp = "x" + m_Difficulty;
        return temp;
    }
}
