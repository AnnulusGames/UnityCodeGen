namespace UnityCodeGen
{
    public static class UnityCodeGenUtility
    {
        public const string defaultFolderPath = "Assets/UnityCodeGen.Generated";

        public static void Generate()
        {
            ScriptFileGenerator.Generate();
        }
    }
}