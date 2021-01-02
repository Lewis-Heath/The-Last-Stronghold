using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float m_Speed;
    [SerializeField] private GameObject[] m_ImpactEffects;
    [SerializeField] private ProjectileType m_Type;
    [SerializeField] private AudioClip m_HitSound;

    public Transform m_Target;
    private Enemy m_Enemy;
    private float m_DistanceToTarget;
    private float m_TimeWithoutTarget;
    private Vector3 m_Direction;
    private float m_MoveAmount;

    public void SetTarget(Transform target)
    {
        m_Target = target;
    }

    void Update()
    {
        if (m_Target != null && !m_Target.GetComponent<Enemy>().GetDying())
        {
            Vector3 direction1 = m_Target.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(direction1);
            Vector3 rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * 10f).eulerAngles;
            transform.rotation = Quaternion.Euler(0f, rotation.y, rotation.z);

            Vector3 middleOfTarget = m_Target.position;
            middleOfTarget.y += 2.2f;
            m_Direction = middleOfTarget - transform.position;
            m_MoveAmount = Time.deltaTime * m_Speed;
            m_DistanceToTarget = m_Direction.magnitude;

            if(m_Direction.magnitude - 1.0f <= m_MoveAmount)
            {
                HitTarget();
            }

            transform.Translate(m_Direction.normalized * m_MoveAmount, Space.World);
        }
        else
        {
            if (m_Type == ProjectileType.Bullet)
                HitTarget();
            else
            {
                if (m_DistanceToTarget <= 10f)
                {
                    HitTarget();
                }
                else
                {
                    UpdateTarget();
                }
            }  
            if(m_Target == null)
            {
                HitTarget();
            }
        }
    }

    private void HitTarget()
    {
        if (m_Type == ProjectileType.Rocket)
        {
            SplashDamage();
        }

        if (m_Target != null && !m_Target.GetComponent<Enemy>().GetDying())
        {
            m_Enemy = m_Target.GetComponent<Enemy>();
            float damage = this.GetComponentInParent<Turret>().GetDamage();
            m_Enemy.TakeDamage(damage, m_Type);
            AudioSource.PlayClipAtPoint(m_HitSound, transform.position);
            GameObject temp = (GameObject)Instantiate(m_ImpactEffects[Random.Range(0, m_ImpactEffects.Length)], transform.position, transform.rotation);
            Destroy(temp.gameObject, 2f);
            Destroy(gameObject);

        }
        else
        {
            if(m_TimeWithoutTarget > 0.5f)
            {
                AudioSource.PlayClipAtPoint(m_HitSound, transform.position);
                GameObject temp = (GameObject)Instantiate(m_ImpactEffects[Random.Range(0, m_ImpactEffects.Length)], transform.position, transform.rotation);
                Destroy(temp.gameObject, 2f);
                Destroy(gameObject);
            }
            else
            {
                m_MoveAmount = Time.deltaTime * m_Speed;
                transform.Translate(m_Direction.normalized * m_MoveAmount, Space.World);
                m_TimeWithoutTarget += Time.deltaTime;
            }
        }
    }

    private void SplashDamage()
    {
        Collider[] objectsWithinExplosion = Physics.OverlapSphere(transform.position, 7.5f);
        foreach (Collider nearbyObject in objectsWithinExplosion)
        {
            if(nearbyObject.gameObject.tag == "Enemy")
            {
                if(m_Target != nearbyObject.transform)
                {
                    Vector3 middleOfEnemy = nearbyObject.transform.position;
                    middleOfEnemy.y += 2.2f;
                    Vector3 direction = middleOfEnemy - transform.position;
                    float distanceToTarget = direction.magnitude;
                    float distanceFromImpactToEnemy = distanceToTarget - 7.5f;
                    distanceFromImpactToEnemy = -distanceFromImpactToEnemy;
                    float damageMultiplier = distanceFromImpactToEnemy / 7.5f;
                    float damage = this.GetComponentInParent<Turret>().GetDamage() * damageMultiplier;
                    nearbyObject.GetComponent<Enemy>().TakeDamage(damage, ProjectileType.Rocket);
                }
            }
        }
    }

    private void UpdateTarget()
    {
        GetComponentInParent<Turret>().UpdateTargetProjectile(this.gameObject);
    }
}
