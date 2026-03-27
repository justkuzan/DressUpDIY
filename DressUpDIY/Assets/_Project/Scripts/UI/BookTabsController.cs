using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class BookTabsController : MonoBehaviour
{
    [System.Serializable]
    public class TabMapping
    {
        public Toggle toggle;
        public Image tabImage;
        public Sprite activeSprite;
        public Sprite inactiveSprite;
        public GameObject pageObject;
    }

    public List<TabMapping> tabs;

    void Start()
    {
        foreach (TabMapping tab in tabs)
        {
            tab.toggle.onValueChanged.AddListener(delegate { UpdateTabs(); });
        }
        UpdateTabs();
    }

    public void UpdateTabs()
    {
        foreach (TabMapping tab in tabs)
        {
            if (tab.toggle.isOn == true)
            {
                tab.tabImage.sprite = tab.activeSprite;
                tab.pageObject.SetActive(true);
            }
            else
            {
                tab.tabImage.sprite = tab.inactiveSprite;
                tab.pageObject.SetActive(false);
            }
        }
    }
}