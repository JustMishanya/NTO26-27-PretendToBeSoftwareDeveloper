using UnityEngine;
using UnityEngine.UI;

public class Git : MonoBehaviour
{
    [SerializeField] GameObject pushButton;
    void Awake()
    {
        if(pushButton == null)
        {
            Debug.LogWarning("У объекта Git отсутствует ссылка на соответствующую кнопку в связи могут быть приколы");
            return;
        }
        else
        {
            pushButton.GetComponent<Button>().onClick.AddListener(G.Instance.Push);
            pushButton.SetActive(false);
            G.Instance.onMaxWPReached += onMaxWPReached;
            G.Instance.onPushChanged += onPushChanged;
        }
    }
    void OnDestroy()
    {
        if(pushButton != null)
        {
            G.Instance.onMaxWPReached -= onMaxWPReached;
            G.Instance.onPushChanged -= onPushChanged;
        }
    }
    void onMaxWPReached()
    {
        pushButton.SetActive(true);
    }
    void onPushChanged(int pushes)
    {
        pushButton.SetActive(false);
    }
}
