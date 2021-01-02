using UnityEngine;

public class Canvas : MonoBehaviour
{
    [SerializeField] GameObject[] m_CanvasUI;

    private void Awake()
    {
        for (int i = 0; i < m_CanvasUI.Length; i++)
        {
            m_CanvasUI[i].SetActive(true);
        }
    }
}
