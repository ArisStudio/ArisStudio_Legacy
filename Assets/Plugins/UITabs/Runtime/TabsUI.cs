using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

//------- Created by  : Hamza Herbou
//------- Email       : hamza95herbou@gmail.com

namespace EasyUI.Tabs
{
    public enum TabsType
    {
        Horizontal,
        Vertical
    }

    public abstract class TabsUI : MonoBehaviour
    {
        [Header("Tabs Customization")]
        [SerializeField]
        Color m_ThemeColor = Color.gray;

#if UNITY_EDITOR
        [SerializeField]
        bool m_ColorizeTabContent = false;

        [SerializeField, Range(0.1f, 0.6f)]
        float m_ThemeColorDifference = 0.1f;

        [SerializeField, Range(0, 10f)]
        float m_TabSpacing = 0f;
#endif

        [Space]
        [Header("OnTabChange Event")]
        public TabsUIEvent m_OnTabChange;

        [System.Serializable]
        public class TabsUIEvent : UnityEvent<int> { }

        List<Button> tabButtons = new List<Button>();
        List<RectTransform> tabContents = new List<RectTransform>();

#if UNITY_EDITOR
        LayoutGroup layoutGroup;
#endif

        Color tabColorActive,
            tabColorInactive,
            oldTabContentColor;
        int current,
            previous;

        Transform parentButtons,
            parentContents;

        int tabButtonsLength,
            tabContentsLength;

        void Start()
        {
            InitializeTabButtons();
        }

        void GetTabButtonsAndContents()
        {
            parentButtons = transform.GetChild(0);
            parentContents = transform.GetChild(1);
            tabButtonsLength = parentButtons.childCount;
            tabContentsLength = parentContents.childCount;

            tabButtons.Clear();
            tabContents.Clear();

            for (int i = 0; i < tabButtonsLength; i++)
            {
                tabButtons.Add(parentButtons.GetChild(i).GetComponent<Button>());
                tabContents.Add(parentContents.GetChild(i).GetComponent<RectTransform>());
            }
        }

        void InitializeTabButtons()
        {
            GetTabButtonsAndContents();

            if (tabButtonsLength != tabContentsLength)
            {
                Debug.LogError(
                    $"Number of Tab Buttons [{tabButtonsLength}] is not same as Tab Contents [{tabContentsLength}]!"
                );
                return;
            }

            for (int i = 0; i < tabButtonsLength; i++)
            {
                int i_copy = i;
                tabButtons[i].onClick.RemoveAllListeners();
                tabButtons[i].onClick.AddListener(() => OnTabButtonClicked(i_copy));

                tabContents[i].gameObject.SetActive(false);
            }

            previous = current = 0;

            tabColorActive = tabButtons[0].GetComponent<Image>().color;
            tabColorInactive = tabButtons[1].GetComponent<Image>().color;

            tabButtons[0].interactable = false;
            tabContents[0].gameObject.SetActive(true);
        }

        public void OnTabButtonClicked(int tabIndex)
        {
            if (current != tabIndex)
            {
                m_OnTabChange?.Invoke(tabIndex);

                previous = current;
                current = tabIndex;

                tabContents[previous].gameObject.SetActive(false);
                tabContents[current].gameObject.SetActive(true);

                tabButtons[previous].GetComponent<Image>().color = tabColorInactive;
                tabButtons[current].GetComponent<Image>().color = tabColorActive;

                tabButtons[previous].interactable = true;
                tabButtons[current].interactable = false;
            }
        }

#if UNITY_EDITOR
        public void UpdateThemeColor(Color color)
        {
            Color colorDark = DarkenColor(color, m_ThemeColorDifference);

            foreach (Button btn in tabButtons)
            {
                Image tabButtonImage = btn.GetComponent<Image>();

                if (btn != tabButtons[0])
                    tabButtonImage.color = colorDark;
                else
                    tabButtonImage.color = color;
            }

            Image parentContentsImage = parentContents.GetComponent<Image>();

            if (m_ColorizeTabContent)
                parentContentsImage.color = color;
            else
            {
                oldTabContentColor = parentContentsImage.color;
                parentContentsImage.color = oldTabContentColor;
            }
        }

        Color DarkenColor(Color color, float amount)
        {
            float h,
                s,
                v;
            Color.RGBToHSV(color, out h, out s, out v);
            v = Mathf.Max(0f, v - amount);
            return Color.HSVToRGB(h, s, v);
        }

        public void Validate(TabsType type)
        {
            GetTabButtonsAndContents();
            UpdateThemeColor(m_ThemeColor);

            if (layoutGroup == null)
                layoutGroup = parentButtons.GetComponent<LayoutGroup>();

            if (type == TabsType.Horizontal)
                ((HorizontalLayoutGroup)layoutGroup).spacing = m_TabSpacing;
            else if (type == TabsType.Vertical)
                ((VerticalLayoutGroup)layoutGroup).spacing = m_TabSpacing;
        }
#endif
    }
}
