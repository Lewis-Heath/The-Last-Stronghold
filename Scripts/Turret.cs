using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] public TurretType m_TurretType;
    [SerializeField] private float m_FireRate;
    [SerializeField] private float m_Damage;
    [SerializeField] private float m_Range;
    [SerializeField] private int m_Cost;
    [SerializeField] private int m_UpgradeCost;
    [SerializeField] private int m_Worth;
    [SerializeField] private Transform m_RotationPoint;
    [SerializeField] private float m_RotationSpeed;
    [SerializeField] private ParticleSystem m_MuzzleFlash;
    [SerializeField] private GameObject m_Projectile;
    [SerializeField] private LineRenderer m_LaserLine;
    [SerializeField] private ParticleSystem m_LaserFireEffect;
    [SerializeField] private ParticleSystem m_LaserHitEffect;
    [SerializeField] private Transform m_FirePoint;
    [SerializeField] private AudioClip m_FireSound;
    [SerializeField] private GameObject m_TempSound;

    [SerializeField] private GameObject[] m_Explosions;
    [SerializeField] private AudioClip m_ExplosionSound;
    [SerializeField] private GameObject m_TurretExplosionEffect;

    private GameObject m_TempLaserSound;
    private ParticleSystem m_TempMuzzleFlash;
    private ParticleSystem m_TempLaserFire;
    private ParticleSystem m_TempLaserHit;
    private float m_TimeToFire;
    public Transform m_Target;

    private int m_Level;

    void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
        m_Level = 1;
    }

    void Update()
    {
        if(m_Target != null)
        {
            Vector3 direction = m_Target.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            Vector3 rotation = Quaternion.Lerp(m_RotationPoint.rotation, lookRotation, Time.deltaTime * m_RotationSpeed).eulerAngles;
            m_RotationPoint.rotation = Quaternion.Euler(0f, rotation.y, rotation.z);

            if(m_TurretType != TurretType.Laser) //Machine gun or rocket launcher
            {
                if (CanFire())
                {
                    ProjectileFire();
                    m_TimeToFire = 60f / m_FireRate;
                }
            }
            else //Laser
            {
                if (m_TurretType == TurretType.Laser)
                {
                    if (m_LaserLine.enabled == false)
                        m_LaserLine.enabled = true;
                }
                LaserFire();
            }
            
        }
        else
        {
            if(m_TurretType == TurretType.Laser)
            {
                if (m_LaserLine.enabled == true)
                    m_LaserLine.enabled = false;
                if (m_TempLaserFire != null)
                    Destroy(m_TempLaserFire.gameObject);
                if (m_TempLaserHit != null)
                    Destroy(m_TempLaserHit.gameObject);
                if (m_TempLaserSound != null)
                    Destroy(m_TempLaserSound.gameObject);
            }
        }
        if (!CanFire() &&m_TurretType != TurretType.Laser)
        {
            m_TimeToFire -= Time.deltaTime;
        }
    }

    private void UpdateTarget()
    {
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        GameObject[] activeEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in activeEnemies)
        {
            if(!enemy.GetComponent<Enemy>().GetDying())
            {
                float distanceBetween = Vector3.Distance(transform.position, enemy.transform.position);
                if (distanceBetween < shortestDistance)
                {
                    shortestDistance = distanceBetween;
                    nearestEnemy = enemy;
                }
            }
        }

        if (nearestEnemy != null && shortestDistance <= m_Range)
        {
            m_Target = nearestEnemy.transform;
        }
        else
        {
            m_Target = null;
        }
    }

    public void UpdateTargetProjectile(GameObject projectile)
    {
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        GameObject[] activeEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in activeEnemies)
        {
            if (!enemy.GetComponent<Enemy>().GetDying())
            {
                float distanceBetween = Vector3.Distance(transform.position, enemy.transform.position);
                if (distanceBetween < shortestDistance)
                {
                    shortestDistance = distanceBetween;
                    nearestEnemy = enemy;
                }
            }
        }

        if (nearestEnemy != null && shortestDistance <= m_Range)
        {
            m_Target = nearestEnemy.transform;
        }
        else
        {
            m_Target = null;
        }

        if (projectile != null)
        {
            projectile.GetComponent<Projectile>().SetTarget(m_Target);
        }
    }

    private void ProjectileFire()
    {
        AudioSource.PlayClipAtPoint(m_FireSound, m_FirePoint.position);
        m_TempMuzzleFlash = Instantiate(m_MuzzleFlash, m_FirePoint.transform);
        Destroy(m_TempMuzzleFlash.gameObject, 0.25f);
        GameObject projectile = Instantiate(m_Projectile, m_FirePoint.position, m_FirePoint.rotation);
        projectile.transform.SetParent(transform);
        projectile.GetComponent<Projectile>().SetTarget(m_Target);
    }

    private void LaserFire()
    {
        Vector3 middleOfTarget = m_Target.position;
        middleOfTarget.y += 2.2f;

        if(m_TempLaserSound == null)
        {
            m_TempLaserSound = Instantiate(m_TempSound, transform);
            m_TempLaserSound.GetComponent<AudioSource>().clip = m_FireSound;
            m_TempLaserSound.GetComponent<AudioSource>().volume = 0.01f;
            m_TempLaserSound.GetComponent<AudioSource>().Play();
            Destroy(m_TempLaserSound.gameObject, 2f);
        }
        
        if (m_TempLaserFire == null)
        {
            m_TempLaserFire = Instantiate(m_LaserFireEffect, m_FirePoint.transform);
            Destroy(m_TempLaserFire.gameObject, 0.5f);
        }

        if(m_TempLaserHit == null)
        {
            m_TempLaserHit = Instantiate(m_LaserHitEffect, middleOfTarget, Quaternion.identity);
            Destroy(m_TempLaserHit.gameObject, 0.5f);
        }
        else
        {
            m_TempLaserHit.transform.position = middleOfTarget;
        }

        m_LaserLine.SetPosition(0, m_FirePoint.position);
        m_LaserLine.SetPosition(1, middleOfTarget);
        m_Target.GetComponent<Enemy>().TakeDamage(m_Damage * Time.deltaTime, ProjectileType.Laser);
    }

    public void Destroy()
    {
        AudioSource.PlayClipAtPoint(m_ExplosionSound, transform.position);

        GameObject temp = (GameObject)Instantiate(m_Explosions[Random.Range(0, m_Explosions.Length)], transform.position, transform.rotation);
        Destroy(temp.gameObject, 3f);

        Vector3 position = transform.position;
        position.y += 3f;
        GameObject temp2 = (GameObject)Instantiate(m_TurretExplosionEffect, position, transform.rotation);
        Destroy(temp2.gameObject, 3f);

        Destroy(gameObject);
    }

    public void Upgrade()
    {
        m_Level++;
        m_FireRate *= 1.25f;
        m_Damage *= 1.25f;
        m_Range *= 1.25f;
        float estimatedWorth = m_Worth * 2f;
        m_Worth = Mathf.FloorToInt(estimatedWorth);
        float estimatedUpgradeCost = m_UpgradeCost * 2.5f;
        m_UpgradeCost = Mathf.FloorToInt(estimatedUpgradeCost);
    }

    private bool CanFire()
    {
        return (m_TimeToFire <= 0f);
    }

    public Transform GetTarget()
    {
        return m_Target;
    }

    public TurretType GetTurretType()
    {
        return m_TurretType;
    }

    public int GetCost()
    {
        return m_Cost;
    }

    public int GetWorth()
    {
        return m_Worth;
    }

    public float GetDamage()
    {
        return m_Damage;
    }
    
    public int GetLevel()
    {
        return m_Level;
    }

    public string GetUpgradeCost()
    {
        if(m_Level < 4)
        {
            string temp = "-" + m_UpgradeCost;
            return temp;
        }
        else
        {
            return "Max Level";
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, m_Range);
    }
}
