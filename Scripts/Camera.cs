using UnityEngine;

public class Camera : MonoBehaviour
{
    [SerializeField] private float m_MoveSpeed;
    [SerializeField] private float m_ZoomSpeed;
    [SerializeField] private float m_MaxZoom;
    [SerializeField] private float m_MinZoom;
    [SerializeField] private float m_ScreenMaxOffset;
    [SerializeField] private GameManager m_GameManager;

    private Vector3 m_OriginalPosition;

    void Start()
    {
        m_OriginalPosition = transform.position;
    }

    void Update()
    {
        if(m_GameManager.GetCurrentScreen() == ScreenType.Playing)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                transform.position = m_OriginalPosition;
            }

            if (Input.GetKey(KeyCode.A))
            {
                transform.Translate(Vector3.left * m_MoveSpeed * Time.deltaTime, Space.World);
            }
            if (Input.GetKey(KeyCode.D))
            {
                transform.Translate(Vector3.right * m_MoveSpeed * Time.deltaTime, Space.World);
            }
            if (Input.GetKey(KeyCode.W))
            {
                transform.Translate(Vector3.forward * m_MoveSpeed * Time.deltaTime, Space.World);
            }
            if (Input.GetKey(KeyCode.S))
            {
                transform.Translate(Vector3.back * m_MoveSpeed * Time.deltaTime, Space.World);
            }

            float scrollAmount = Input.GetAxis("Mouse ScrollWheel");
            Vector3 position = transform.position;
            position.y -= scrollAmount * 1000 * m_ZoomSpeed * Time.deltaTime;
            position.y = Mathf.Clamp(position.y, m_MinZoom, m_MaxZoom);
            transform.position = position;
        }
        else
        {
            if (transform.position != m_OriginalPosition)
                transform.position = m_OriginalPosition;
        }
    }
}
