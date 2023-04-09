using System;
using System.Linq;
using System.Reflection;
using System.IO;
using UnityEditor;

namespace UnityCodeGen
{
    internal static class ScriptFileGenerator
    {
        const string KEY_ISGENERATING = "UnityCodeGen-IsGenerating";
        const string EXTENSION_META = ".meta";

        static bool isGenerating
        {
            get
            {
                if (bool.TryParse(EditorUserSettings.GetConfigValue(KEY_ISGENERATING), out var result))
                {
                    return result;
                }
                return false;
            }
            set
            {
                EditorUserSettings.SetConfigValue(KEY_ISGENERATING, value.ToString());
            }
        }

        // static List<string> fileNames = new List<string>();

        [InitializeOnLoadMethod]
        static void Initialize()
        {
            if (isGenerating)
            {
                isGenerating = false;
            }
            else if (UnityCodeGenSettings.autoGenerateOnCompile)
            {
                Generate();
            }
        }

        internal static void Generate()
        {
            if (isGenerating) return;
            isGenerating = true;

            // fileNames.Clear();
            var generatorTypes = TypeCache.GetTypesDerivedFrom<ICodeGenerator>()
                .Where(x => !x.IsAbstract && x.GetCustomAttribute<GeneratorAttribute>() != null);

            var changed = false;
            foreach (var t in generatorTypes)
            {
                var generator = (ICodeGenerator)Activator.CreateInstance(t);
                var context = new GeneratorContext();
                generator.Execute(context);

                if (GenerateScriptFromContext(context))
                {
                    changed = true;
                }
            }

            // foreach (var file in Directory.GetFiles(FOLDER_PATH))
            // {
            //     var name = Path.GetFileName(file);
            //     if (Path.GetExtension(name) != EXTENSION_META && !fileNames.Contains(name))
            //     {
            //         AssetDatabase.DeleteAsset(file);
            //         changed = true;
            //     }
            // }

            if (changed)
            {
                AssetDatabase.Refresh();
                AssetDatabase.SaveAssets();
            }
        }

        static bool GenerateScriptFromContext(GeneratorContext context)
        {
            var changed = false;

            var folderPath = context.overrideFolderPath ?? UnityCodeGenUtility.defaultFolderPath;

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            foreach (var code in context.codeList)
            {
                var hierarchy = code.fileName.Split('/');
                var fileName = hierarchy[hierarchy.Length - 1];
                var path = folderPath;
                for (int i = 0; i < hierarchy.Length; i++)
                {
                    path += "/" + hierarchy[i];
                    if (i == hierarchy.Length - 1) break;
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                }

                if (File.Exists(path))
                {
                    var text = File.ReadAllText(path);
                    if (text == code.text)
                    {
                        // fileNames.Add(code.fileName);
                        continue;
                    }
                }

                File.WriteAllText(path, code.text);
                // fileNames.Add(code.fileName);
                changed = true;
            }
            
            return changed;
        }
    }

}