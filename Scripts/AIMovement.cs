using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

public class AIMovement : MonoBehaviour
{
    [SerializeField] public AudioClip[] m_ScreamSounds;
    public NavMeshAgent m_Agent;
    private GameObject m_Destinations;
    public ThirdPersonCharacter m_Character;
    public List<Transform> m_DestinationsList = new List<Transform>();
    private float m_SpawnCooldown = 1f;
    private int m_RandomNum;
    private GameManager m_GameManager;

    void Start()
    {
        m_GameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        //Agent settings
        m_Agent.updateRotation = false;

        //Destination picking
        m_Destinations = GameObject.Find("Destinations");
        foreach (Transform t in m_Destinations.GetComponentInChildren<Transform>())
        {
            m_DestinationsList.Add(t);
        }
        m_RandomNum = Random.Range(0, 10);
        m_Agent.SetDestination(m_DestinationsList[m_RandomNum].position);
    }

    void Update()
    {
        if(!GetComponent<Enemy>().GetDying())
        {
            //Character movement
            if (m_Agent.remainingDistance > m_Agent.stoppingDistance)
            {
                m_Character.Move(m_Agent.desiredVelocity, false, false);
            }
            else
            {
                m_Character.Move(Vector3.zero, false, false);
            }

            if (m_Agent.remainingDistance < 1.0f && m_SpawnCooldown < 0.5f)
            {
                m_GameManager.DecreaseLife();
                AudioSource.PlayClipAtPoint(m_ScreamSounds[Random.Range(0, m_ScreamSounds.Length)], transform.position);
                Destroy(gameObject);
            }

            m_SpawnCooldown -= Time.deltaTime;
        }
    }

    public void SetSpeed(float speed)
    {
        m_Agent.speed = speed;
    }

    public float GetSpeed()
    {
        return m_Agent.speed;
    }


}
