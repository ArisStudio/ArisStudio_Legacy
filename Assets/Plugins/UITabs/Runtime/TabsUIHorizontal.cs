namespace EasyUI.Tabs
{
    public class TabsUIHorizontal : TabsUI
    {
        #if UNITY_EDITOR
        void Reset()
        {
            OnValidate();
        }

        void OnValidate()
        {
            base.Validate(TabsType.Horizontal);
        }
        #endif
    }
}
