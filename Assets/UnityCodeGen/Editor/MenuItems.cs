using UnityEditor;

namespace UnityCodeGen
{
    static class MenuItems
    {
        const string MENU_GENERATE = "Tools/UnityCodeGen/Generate";
        const string MENU_TOGGLE_AUTO_GENERATE = "Tools/UnityCodeGen/Auto-generate on Compile";

        [InitializeOnLoadMethod]
        static void Init()
        {
            Menu.SetChecked(MENU_TOGGLE_AUTO_GENERATE, UnityCodeGenSettings.autoGenerateOnCompile);
        }

        [MenuItem(MENU_GENERATE)]
        static void Generate()
        {
            ScriptFileGenerator.Generate();
        }

        [MenuItem(MENU_TOGGLE_AUTO_GENERATE)]
        static void ToggleAutoGenerate()
        {
            UnityCodeGenSettings.autoGenerateOnCompile = !UnityCodeGenSettings.autoGenerateOnCompile;
            Menu.SetChecked(MENU_TOGGLE_AUTO_GENERATE, UnityCodeGenSettings.autoGenerateOnCompile);
        }
    }
}
