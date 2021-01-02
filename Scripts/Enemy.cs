using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float m_Health;
    [SerializeField] private GameObject m_RocketDeath;
    [SerializeField] private GameObject m_LaserDeath;
    [SerializeField] private GameObject m_BulletDeath;
    [SerializeField] public AudioClip[] m_DeathSounds;
    [SerializeField] private GameObject m_SpawnEffect;
    [SerializeField] public AudioClip m_TeleportSound;
    [SerializeField] private Image m_HealthBar;

    private float m_MaxHealth;
    private Animator m_Animator;
    private AIMovement m_AIMovement;
    private bool m_Dying = false;
    private float m_DeathTimer = 0f;
    private GameObject m_DeathParticles;
    private BuildingManager m_BuildingManager;
    private GameManager m_GameManager;
    private bool m_DeathSoundPlayed = false;
    private AudioClip m_DeathSound;
    private bool m_MoneyAdded = false;
    private bool m_BeingLasered = false;
    private float m_LaserCooldownTime = 0.0f;

    void Start()
    {
        Vector3 position = transform.position;
        position.x -= 2f;
        AudioSource.PlayClipAtPoint(m_TeleportSound, position);
        GameObject temp = (GameObject)Instantiate(m_SpawnEffect, position, transform.rotation);
        Destroy(temp.gameObject, 2f);
        m_BuildingManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<BuildingManager>();
        m_GameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        m_Animator = GetComponent<Animator>();
        m_AIMovement = GetComponent<AIMovement>();
        m_DeathSound = m_DeathSounds[Random.Range(0, 4)];
        m_MaxHealth = m_Health;
        m_HealthBar.color = Color.green;
    }

    private void Update()
    {
        if(!m_Dying)
        {
            if(m_BeingLasered)
            {
                if(m_AIMovement.GetSpeed() == 1.0f)
                    m_AIMovement.SetSpeed(0.8f);
            }
            if (m_AIMovement.GetSpeed() == 0.8f)
            {
                if (m_LaserCooldownTime > 0.5f)
                {
                    m_BeingLasered = false;
                    m_AIMovement.SetSpeed(1.0f);
                    m_LaserCooldownTime = 0.0f;
                }
                else
                {
                    m_LaserCooldownTime += Time.deltaTime;
                }
            }
        }
    }

    public void TakeDamage(float amount, ProjectileType hitType)
    {
        if(hitType == ProjectileType.Laser)
        {
            m_BeingLasered = true;
        }

        m_Health -= amount;
        if (m_Health <= 0f)
        {
            m_HealthBar.fillAmount = 0f;
        }
        else
        {
            m_HealthBar.fillAmount = m_Health / m_MaxHealth;
        }
        SetHealthBarColour();

        if(m_Health <= 0f)
        {
            Die(hitType);
        }
    }

    private void SetHealthBarColour()
    {
        float healthPercent = m_Health / m_MaxHealth;
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

    private void Die(ProjectileType hitType)
    {
        if (hitType == ProjectileType.Rocket)
        {
            Vector3 deathParticlePosition = transform.position;
            deathParticlePosition.y += 3f;
            m_DeathParticles = (GameObject)Instantiate(m_RocketDeath, deathParticlePosition, transform.rotation);
            Destroy(m_DeathParticles.gameObject, 2.5f);
            Destroy(gameObject);
            
        }
        if (hitType == ProjectileType.Bullet)
        {
            SetDying(true);
            GetComponent<AIMovement>().enabled = false;
            m_Animator.SetTrigger("DeathTrigger");
            m_HealthBar.transform.parent.gameObject.SetActive(false);

            Vector3 deathParticlePosition = transform.position;
            deathParticlePosition.z -= 0.5f;
            m_DeathParticles = (GameObject)Instantiate(m_BulletDeath, deathParticlePosition, Quaternion.identity);
            Destroy(m_DeathParticles.gameObject, 3.3f);

            Destroy(gameObject, 0.95f);
        }
        if(hitType == ProjectileType.Laser)
        {
            Vector3 deathParticlePosition = transform.position;
            deathParticlePosition.y += 3;
            m_DeathParticles = (GameObject)Instantiate(m_LaserDeath, deathParticlePosition, transform.rotation);
            Destroy(m_DeathParticles.gameObject, 2.5f);
            Destroy(gameObject);

        }

        if (!m_MoneyAdded)
        {
            m_BuildingManager.ChangeTotalMoney(15, transform.position);
            m_GameManager.IncreaseEnemiesKilled();
            m_MoneyAdded = true;
        }

        if (m_DeathSoundPlayed == false)
        {
            AudioSource.PlayClipAtPoint(m_DeathSound, transform.position);
            m_DeathSoundPlayed = true;
        }
    }

    private void SetDying(bool decision)
    {
        m_Dying = decision;
    }

    public bool GetDying()
    {
        return m_Dying;
    }
}
