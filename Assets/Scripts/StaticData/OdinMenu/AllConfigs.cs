#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;
using UnityEditor;


namespace StaticData.OdinMenu
{
    public class AllConfigs : OdinMenuEditorWindow
    {
        [MenuItem("Tools/All Configs")]
        private static void OpenWindow()
        {
            GetWindow<AllConfigs>().Show();
        }


        protected override OdinMenuTree BuildMenuTree()
        {
            OdinMenuTree tree = new OdinMenuTree();
            
            return tree;
        }
    }
}
#endif
