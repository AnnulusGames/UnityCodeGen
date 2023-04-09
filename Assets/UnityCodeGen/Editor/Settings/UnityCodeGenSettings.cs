using UnityEditor;

namespace UnityCodeGen
{
    public static class UnityCodeGenSettings
    {
        const string KEY_GENERATE_ON_COMPILE = "UnityCodeGen-AutoGenerateOnCompile";

        public static bool autoGenerateOnCompile
        {
            get
            {
                if (bool.TryParse(EditorUserSettings.GetConfigValue(KEY_GENERATE_ON_COMPILE), out var result))
                {
                    return result;
                }
                return false;
            }
            set
            {
                EditorUserSettings.SetConfigValue(KEY_GENERATE_ON_COMPILE, value.ToString());
            }
        }
    }
}