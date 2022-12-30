using UnityEditor;
using UnityEngine;

namespace XSJGame.Finder.Editor
{
    static class MenuItems
    {
        [MenuItem("Assets/Finder/Find Missing Script - All Assets", priority = 1000)]
        static void FindMissingOnAllAssets()
        {
            ShowWindow(typeof(FindMissingOnAllAssets));
        }
        [MenuItem("Assets/Finder/Find Missing Script - Current Scene", priority = 1000)]
        static void FindMissingOnCurrentScene()
        {
            ShowWindow(typeof(FindMissingOnCurrentScene));
        }
        [MenuItem("Assets/Finder/Find Missing Prop - All Assets", priority = 1000)]
        static void FindMissingPropertyOnAllAssets()
        {
            ShowWindow(typeof(FindMissingPropertyOnAllAssets));
        }
        [MenuItem("Assets/Finder/Find Missing Prop - Current Scene", priority = 1000)]
        static void FindMissingPropertyOnCurrentScene()
        {
            ShowWindow(typeof(FindMissingPropertyOnCurrentScene));
        }
        [MenuItem("Assets/Finder/Find Prefabs By Shader", priority = 1100)]
        static void FindPrefabByShader()
        {
            ShowWindow<FindPrefabByShader>();
        }
        [MenuItem("Assets/Finder/Find Prefabs By Material", priority = 1100)]
        static void FindPrefabByMaterial()
        {
            ShowWindow<FindPrefabByMaterial>();
        }
        [MenuItem("Assets/Finder/Find Prefabs By Script", priority = 1100)]
        static void FindPrefabByScript()
        {
            ShowWindow<FindPrefabByScript>();
        }
        [MenuItem("Assets/Finder/Find Prefabs By Script2(recommend)", priority = 1100)]
        static void ShowMyEditorWindow()
        {
            EditorWindow.GetWindow<ScriptReferences>("查找脚本引用").Show();
        }
        [MenuItem("Assets/Finder/Find Prefabs By Animation Clip", priority = 1100)]
        static void FindPrefabByAnimation()
        {
            ShowWindow<FindPrefabByAnimation>();
        }
        [MenuItem("Assets/Finder/Find Prefabs By Textue", priority = 1100)]
        static void FindPrefabByTextue()
        {
            ShowWindow<FindPrefabByTextue>();
        }
        [MenuItem("Assets/Finder/Find Prefabs By Sprite", priority = 1100)]
        static void FindPrefabBySprite()
        {
            ShowWindow<FindPrefabBySprite>();
        }
        
        [MenuItem("Assets/Finder/输出文件夹内图片的信息(UI)", priority = 1100)]
        static void TextureReferences()
        {
            EditorWindow.GetWindow<TextureReferences>("查找图片引用(UI)").Show();
        }
        
        [MenuItem("Assets/Finder/输出文件夹内图片的信息(UI)", true)]
        [MenuItem("Assets/Finder/输出文件夹内图片的信息(特效)", true)]
        static bool ValidateLogFolderTextureMsg()
        {
            bool open = Selection.objects.Length == 1 && 
                        Selection.GetFiltered(typeof(Object), SelectionMode.Assets)[0].GetType().Name == "DefaultAsset";
            if (!open)
                open = Selection.activeObject?.GetType().Name == "Texture2D";
            return open;
        }
        
        [MenuItem("Assets/Finder/输出文件夹内图片的信息(特效)", priority = 1100)]
        static void EffectTextureReferences()
        {
            EditorWindow.GetWindow<EffectTextureReferences>("查找图片引用(特效)").Show();
        }
        
        [MenuItem("Assets/Finder/Find Prefabs By Child Name", priority = 1100)]
        static void FindPrefabByChildrenName()
        {
            ShowWindow(typeof(FindPrefabByChildrenName));
        }
        [MenuItem("Assets/Finder/Find Materials By Texture", priority = 1200)]
        static void FindMaterialByTexture()
        {
            ShowWindow<FindMaterialByTexture>();
        }
        [MenuItem("Assets/Finder/Find Materials By Shader", priority = 1200)]
        static void FindMaterialByShader()
        {
            ShowWindow<FindMaterialByShader>();
        }
        //[MenuItem("Assets/Finder/FindSprite")]
        //static void FindSprite()
        //{
        //    ShowWindow<FindSprite>();
        //}
        //[MenuItem("Assets/Finder/FindText")]
        //static void FindText()
        //{
        //    ShowWindow<FindText>();
        //}
        //[MenuItem("Assets/Finder/FindFontInPrefab")]
        //static void FindFontInPrefab()
        //{
        //    ShowWindow<FindFontInPrefab>();
        //}
        [MenuItem("Assets/Finder/Find Usage", priority = 1300)]
        static void FindUsage()
        {
            ShowWindow<FindUsage>();
        }
        [MenuItem("Assets/Finder/Find Usage - Current Scene", priority = 1301)]
        static void FindUsageOnCurrentScene()
        {
            ShowWindow<FindUsageOnCurrentScene>();
        }

        private static void ShowWindow<T>(string title = null) where T : EditorWindow, IFinderWindow
        {
            if (string.IsNullOrEmpty(title))
            {
                title = typeof(T).Name;
            }
            var window = EditorWindow.GetWindow<T>();
            window.InitAsset(Selection.activeObject);
            window.titleContent = new UnityEngine.GUIContent(title);
            window.Show();
        }

        private static void ShowWindow(System.Type winType, string title = null)
        {
            if (string.IsNullOrEmpty(title))
            {
                title = winType.Name;
            }
            var window = EditorWindow.GetWindow(winType);
            window.titleContent = new UnityEngine.GUIContent(title);
            window.Show();
        }
    }
}
