using UnityEngine;

public class CarLight : MonoBehaviour
{
    private Light m_Light;
    private float m_Cooldown;
    private bool m_Active;
    private float m_RandomSwitchTime;

    private void Start()
    {
        m_Light = gameObject.GetComponent<Light>();
        m_Active = true;
        RandomSwitchTime();
    }

    void Update()
    {
        if(m_Cooldown > m_RandomSwitchTime)
        {
            m_Cooldown = 0f;
            SwitchLightState();
            RandomSwitchTime();
        }
        else
        {
            m_Cooldown += Time.deltaTime;
        }
    }

    void SwitchLightState()
    {
        if(!m_Active)
        {
            m_Active = true;
            m_Light.intensity = 25f;
        }
        else
        {
            m_Active = false;
            m_Light.intensity = 0f;
        }
    }

    void RandomSwitchTime()
    {
        m_RandomSwitchTime = Random.Range(0.1f, 1f);
    }
}
