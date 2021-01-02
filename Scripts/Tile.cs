using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Color m_HoverEmptyColor;
    [SerializeField] private Color m_HoverFullColor;
    [SerializeField] private Color m_HoverSelectedColor;
    [SerializeField] private Color m_SelectedColor1;
    [SerializeField] private Color m_SelectedColor2;
    [SerializeField] private Color m_SelectedColor3;
    [SerializeField] private AudioClip m_TurretPlacedSound;

    private Color m_OriginalColor;
    private Renderer m_Renderer;
    private GameObject m_CurrentTurret;
    private GameObject m_GameManagerGO;
    private GameManager m_GameManager;
    private BuildingManager m_BuildingManager;
    private bool m_MenuOpen;

    void Start()
    {
        m_Renderer = GetComponent<Renderer>();
        m_OriginalColor = m_Renderer.material.color;
        m_GameManagerGO = GameObject.FindGameObjectWithTag("GameManager");
        m_GameManager = m_GameManagerGO.GetComponent<GameManager>();
        m_BuildingManager = m_GameManagerGO.GetComponent<BuildingManager>();
    }

    public void BuildTurret(GameObject turretToBuild, int cost)
    {
        if(m_GameManager.GetCurrentScreen() == ScreenType.Playing)
        {
            if (m_CurrentTurret == null)
            {
                if (m_BuildingManager.GetMoney() - cost >= 0)
                {
                    m_BuildingManager.ChangeTotalMoney(-cost, transform.position);
                    AudioSource.PlayClipAtPoint(m_TurretPlacedSound, transform.position);
                    m_CurrentTurret = Instantiate(turretToBuild, transform.position, transform.rotation);
                }
                else
                {
                    m_BuildingManager.ShowText(transform.position, "Insufficent funds!", Color.red);
                }
            }
        }
    }

    void OnMouseOver()
    {
        if(!m_MenuOpen)
        {
            if (m_CurrentTurret == null)
            {
                SetMaterialColour("Free");
                if (Input.GetMouseButtonDown(1))
                {
                    //Open shop
                    m_BuildingManager.SelectTile(this);
                    m_BuildingManager.OpenMenu("Shop");
                    SetMaterialColour("Selected");
                }
            }
            else
            {
                SetMaterialColour("Full");
                if (Input.GetMouseButtonDown(1))
                {
                    //Open Upgrade menu
                    m_BuildingManager.SelectTile(this);
                    m_BuildingManager.OpenMenu("Upgrade/Sell");
                    if(m_CurrentTurret.GetComponent<Turret>().GetTurretType() == TurretType.MachineGun)
                        SetMaterialColour("MachineGun");
                    if (m_CurrentTurret.GetComponent<Turret>().GetTurretType() == TurretType.RocketLauncher)
                        SetMaterialColour("RocketLauncher");
                    if (m_CurrentTurret.GetComponent<Turret>().GetTurretType() == TurretType.Laser)
                        SetMaterialColour("Laser");
                }
            }
        }
    }

    void OnMouseExit()
    {
        if(!m_MenuOpen)
            SetMaterialColour("Blank");
    }

    void SetMaterialColour(string type)
    {
        if(type == "Free")
        {
            m_Renderer.material.color = m_HoverEmptyColor;
        }
        if (type == "Selected")
        {
            m_Renderer.material.color = m_HoverSelectedColor;
        }
        if (type == "Full")
        {
            m_Renderer.material.color = m_HoverFullColor;
        }
        if(type == "Blank")
        {
            m_Renderer.material.color = m_OriginalColor;
        }
        if(type == "MachineGun")
        {
            m_Renderer.material.color = m_SelectedColor1;
        }
        if (type == "RocketLauncher")
        {
            m_Renderer.material.color = m_SelectedColor2;
        }
        if (type == "Laser")
        {
            m_Renderer.material.color = m_SelectedColor3;
        }
    }

    public void SetMenuOpen(bool decision)
    {
        m_MenuOpen = decision;
        if (decision == false)
        {
            SetMaterialColour("Blank");
        }
    }

    public GameObject GetTurret()
    {
        return m_CurrentTurret;
    }

    public bool GetMenuOpen()
    {
        return m_MenuOpen;
    }
}
