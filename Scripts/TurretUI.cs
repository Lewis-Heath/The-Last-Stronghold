using UnityEngine;
using UnityEngine.UI;

public class TurretUI : MonoBehaviour
{
    public void SelectedUpgrade()
    {
        UpgradeTurret();
        Destroy(gameObject);
    }

    public void SelectedSell()
    {
        SellTurret();
        Destroy(gameObject);
    }

    [SerializeField] private GameObject m_UpgradePrice;
    [SerializeField] private GameObject m_SellPrice;
    private GameObject m_GameManager;

    private void Start()
    {
        m_GameManager = GameObject.FindGameObjectWithTag("GameManager");
    }

    private void UpgradeTurret()
    {
        m_GameManager.GetComponent<BuildingManager>().UpgradeTurret();
    }

    private void SellTurret()
    {
        m_GameManager.GetComponent<BuildingManager>().DestroyTurret();
    }

    public void SetText(string name, string text, int fontSize)
    {
        if(name == "Upgrade")
        {
            m_UpgradePrice.GetComponent<Text>().text = text;
            m_UpgradePrice.GetComponent<Text>().fontSize = fontSize;
        }
        if (name == "Sell")
        {
            m_SellPrice.GetComponent<Text>().text = text;
        }
    }
}
