using UnityEngine;

public class Shop : MonoBehaviour
{

    public void SelectedMachineGunTurret()
    {
        SetCurrentSelected(TurretType.MachineGun);
        Destroy(gameObject);
    }

    public void SelectedRocketLauncherTurret()
    {
        SetCurrentSelected(TurretType.RocketLauncher);
        Destroy(gameObject);
    }

    public void SelectedLaserTurret()
    {
        SetCurrentSelected(TurretType.Laser);
        Destroy(gameObject);
    }

    private GameObject m_GameManager;
    private GameObject m_TurretSelected;

    private void Start()
    {
        m_GameManager = GameObject.FindGameObjectWithTag("GameManager");
        m_TurretSelected = GameObject.FindGameObjectWithTag("TurretSelected");
    }

    public void SetCurrentSelected(TurretType type)
    {
        m_GameManager.GetComponent<BuildingManager>().SetTurretSelected(type);
    }
}
