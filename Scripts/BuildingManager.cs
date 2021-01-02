using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class BuildingManager : MonoBehaviour
{
    [SerializeField] private GameObject m_MachineGunTurret;
    [SerializeField] private GameObject m_RocketLauncherTurret;
    [SerializeField] private GameObject m_LaserTurret;
    [SerializeField] private Text m_MoneyText;
    [SerializeField] private GameObject m_AmountChangedText;
    [SerializeField] private GameManager m_GameManager;

    [SerializeField] private GameObject m_ShopMenu;
    private GameObject m_Shop;
    [SerializeField] private GameObject m_TurretMenu;
    private GameObject m_TurretUI;

    private Tile m_TileSelected;
    public int m_TotalMoney;
    private GameObject m_TurretSelected;


    void Start()
    {
        m_MoneyText.text = m_TotalMoney.ToString();
    }

    void Update()
    {
        if(m_GameManager.GetCurrentScreen() == ScreenType.Playing)
        {
            if(Input.GetMouseButtonDown(2))
            {
                if(m_Shop != null)
                {
                    m_TileSelected.SetMenuOpen(false);
                    Destroy(m_Shop.gameObject);
                }

                if (m_TurretUI != null)
                {
                    m_TileSelected.SetMenuOpen(false);
                    Destroy(m_TurretUI.gameObject);
                }
            }
        }
    }

    public int GetTurretCost()
    {
        if(m_TurretSelected != null)
            return m_TurretSelected.GetComponent<Turret>().GetCost();
        return 0;
    }

    public int GetTurretWorth()
    {
        if (m_TurretSelected != null)
            return m_TurretSelected.GetComponent<Turret>().GetWorth();
        return 0;
    }

    public int GetUpgradeCost()
    {
        if (m_TurretSelected != null)
            return int.Parse(m_TurretSelected.GetComponent<Turret>().GetUpgradeCost());
        return 0;
    }

    public int GetMoney()
    {
        return m_TotalMoney;
    }

    public void ShowText(Vector3 position, string text, Color color)
    {
        position.y += 5;
        GameObject temp = Instantiate(m_AmountChangedText, position, Quaternion.identity);
        temp.GetComponent<TextMesh>().text = text;
        temp.GetComponent<TextMesh>().color = color;
    }

    public void ChangeTotalMoney(int amount, Vector3 position)
    {
        m_TotalMoney += amount;
        m_MoneyText.text = m_TotalMoney.ToString();
        if(amount < 0)
            ShowText(position, amount.ToString(), Color.red);
        else
        {
            string text = "+" + amount.ToString();
            ShowText(position, text, Color.green);
        }
    }

    public void SelectTile(Tile tile)
    {
        if(m_TileSelected != null)
            m_TileSelected.SetMenuOpen(false);
        m_TileSelected = tile;
        m_TurretSelected = null;
    }

    public void SetTurretSelected(TurretType type)
    {
        if (type == TurretType.MachineGun)
        {
            m_TurretSelected = m_MachineGunTurret;
        }

        if (type == TurretType.RocketLauncher)
        {
            m_TurretSelected = m_RocketLauncherTurret;
        }

        if (type == TurretType.Laser)
        {
            m_TurretSelected = m_LaserTurret;
        }

        m_TileSelected.BuildTurret(m_TurretSelected, GetTurretCost());
        m_TileSelected.SetMenuOpen(false);
        m_TileSelected = null;
    }

    public void OpenMenu(string type)
    {
        if(type == "Shop")
        {
            DestroyMenus();
            Vector3 position = m_TileSelected.transform.position;
            position.y += 8f;
            m_Shop = Instantiate(m_ShopMenu, position, Quaternion.identity);
            m_TileSelected.SetMenuOpen(true);

        }
        if(type == "Upgrade/Sell")
        {
            DestroyMenus();
            m_TurretSelected = m_TileSelected.GetTurret();
            Vector3 position = m_TileSelected.transform.position;
            position.y += 8f;
            m_TurretUI = Instantiate(m_TurretMenu, position, Quaternion.identity);
            string temp = "+" + m_TurretSelected.GetComponent<Turret>().GetWorth().ToString();
            string temp2 = m_TurretSelected.GetComponent<Turret>().GetUpgradeCost();

            if(temp2 == "Max Level")
                m_TurretUI.GetComponent<TurretUI>().SetText("Upgrade", temp2, 225);
            else
                m_TurretUI.GetComponent<TurretUI>().SetText("Upgrade", temp2, 300);

            m_TurretUI.GetComponent<TurretUI>().SetText("Sell", temp, 0);

            m_TileSelected.SetMenuOpen(true);
        }
    }

    private void DestroyMenus()
    {
        if (m_Shop != null)
        {
            Destroy(m_Shop.gameObject);
        }
        if (m_TurretUI != null)
        {
            Destroy(m_TurretUI.gameObject);
        }
    }

    public void UpgradeTurret()
    {
        m_TileSelected.SetMenuOpen(false);

        if(m_TileSelected.GetComponent<Tile>().GetTurret().GetComponent<Turret>().GetLevel() < 4)
        {
            if(m_TotalMoney + GetUpgradeCost() > 0)
            {
                ChangeTotalMoney(GetUpgradeCost(), m_TileSelected.transform.position);
                m_TileSelected.GetComponent<Tile>().GetTurret().GetComponent<Turret>().Upgrade();
            }
            else
            {
                m_GameManager.GetComponent<BuildingManager>().ShowText(m_TileSelected.transform.position, "Insufficent funds!", Color.red);
            }
        }
        else
        {
            m_GameManager.GetComponent<BuildingManager>().ShowText(m_TileSelected.transform.position, "Turret Max Level!", Color.red);
        }

        m_TileSelected = null;
    }

    public void DestroyTurret()
    {
        m_TileSelected.SetMenuOpen(false);
        ChangeTotalMoney(GetTurretWorth(), m_TileSelected.transform.position);
        m_TileSelected.GetComponent<Tile>().GetTurret().GetComponent<Turret>().Destroy();
        m_TileSelected = null;
    }
}
