using UnityEngine;
using UnityEngine.UI;
public class Monitor : MonoBehaviour
{
    private MonitorPage[] _pages;
    private int pageIndex = 0;
    public void Init(Workplace workplace)
    {
        Transform pageObject = transform.Find("Pages");
        Transform iconBar = transform.Find("PanelBar").Find("Icons");
        GameObject iconPrefab = G.Instance.GetService<AssetService>().GetAsset<GameObject>("MonitorIcon");
        if (pageObject != null)
        {
            Debug.Log("Im found pageObject");
            _pages = pageObject.GetComponentsInChildren<MonitorPage>();
            for(int i = 0; i < _pages.Length; i++)
            {
                var page = _pages[i];
                page.order = i;
                Debug.Log("Page "+page.name);
                page.gameObject.SetActive(i==pageIndex);
                var newIcon = Instantiate(iconPrefab,iconBar);
                newIcon.GetComponent<Button>().onClick.AddListener(()=>SwitchPage(page.order));
                newIcon.GetComponent<Image>().sprite = page.iconSprite;
                foreach(var indicator in page.GetComponentsInChildren<Indicator>())
                {
                    switch (indicator.id)
                    {
                        case "wp":
                        if(indicator is SliderIndicator)
                        {
                            ((SliderIndicator)indicator).maxValue = G.Instance.maxWP;
                        }
                        G.Instance.onWPChanged += indicator.ToIndicate;
                        break;
                        case "penality":
                        G.Instance.onPenalitiesChanged += indicator.ToIndicate;
                        break;
                    }
                }
            }
        }
        else
        {
            Debug.LogWarning("Монитор ничего не покажет без страниц");
        }
    }
    private void SwitchPage(int newIndex)
    {
        _pages[pageIndex].gameObject.SetActive(false);
        _pages[newIndex].gameObject.SetActive(true);
        pageIndex = newIndex;
    }
}