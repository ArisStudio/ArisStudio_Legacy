namespace EasyUI.Tabs
{
    public class TabsUIVertical : TabsUI
    {
        #if UNITY_EDITOR
        void OnValidate()
        {
            base.Validate(TabsType.Vertical);
        }
        #endif
    }
}
