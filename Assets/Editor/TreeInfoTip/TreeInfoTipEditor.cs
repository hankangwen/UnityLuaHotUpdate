using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;
using UnityEditor;
using UnityEngine;

namespace TreeInfoTip
{
    /// <summary>
    /// 为Project窗口的项目目录树添加备注信息
    /// </summary>
    public class TreeInfoTipEditor : EditorWindow
    {
        [MenuItem("Tools/项目目录树添加备注信息")]
        static void ShowTreeInfoTipEditor()
        {
            GetWindow<TreeInfoTipEditor>("项目目录树添加备注信息").Show();
        }

        private bool isOpenProjectWindowItemOnGUI = false;
        private void OnGUI()
        {
            if (GUILayout.Button("打开"))
            {
                // TreeInfoTipManager.Instance.SwitchTreeInfoTip(true);
                // if (isOpenProjectWindowItemOnGUI)
                //     return;
                // EditorApplication.projectWindowItemOnGUI += ProjectWindowItemOnGUI;
                // isOpenProjectWindowItemOnGUI = true;
            }
            if (GUILayout.Button("关闭"))
            {
                // TreeInfoTipManager.Instance.SwitchTreeInfoTip(false);
                // EditorApplication.projectWindowItemOnGUI -= ProjectWindowItemOnGUI;
                // isOpenProjectWindowItemOnGUI = false;
            }
            if (GUILayout.Button("测试"))
            {
                string path = Application.dataPath + "/Editor/TreeInfoTip/DirectoryV2.xml";
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(path);
                
                var treeNodes = xmlDoc.SelectNodes("trees/tree");
                if (treeNodes != null)
                {
                    foreach (XmlNode node in treeNodes)
                    {
                        var element = node as XmlElement;
                        if (element == null)
                            continue;

                        var elementName = element.GetAttribute("path");
                        Debug.Log(elementName);
                        elementName = element.GetAttribute("title");
                        Debug.Log(elementName);
                    }
                }
            }
        }

        private Dictionary<string, string> Guid2Title = new Dictionary<string, string>();
        private float _column = 5.0f; 
        private GUIStyle _style;
        private Color32 _textColor = new Color32(255, 0, 0, 200);
        private void ProjectWindowItemOnGUI(string guid, Rect selectionRect)
        {
            string message = "123456";
            // if (Guid2Title.TryGetValue(guid, out string message))
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                if (0 >= path.Length)
                    return;

                string nameRaw;
                var attr = File.GetAttributes(path);
                if (((attr & FileAttributes.Directory) == FileAttributes.Directory) && path.Contains("."))
                {
                    string[] arrays = path.Split('/');
                    nameRaw = arrays[arrays.Length - 1];
                }
                else
                {
                    nameRaw = Path.GetFileNameWithoutExtension(path);
                }

                if (_style is null)
                {
                    _style = new GUIStyle(EditorStyles.label);
                }

                _style.normal.textColor = new Color32(255, 0, 0, 200);
                var extSize = _style.CalcSize(new GUIContent(message));
                var nameSize = _style.CalcSize(new GUIContent(nameRaw));
                selectionRect.x += nameSize.x + (IsSingleColumnView ? 15 : 18) + _column;
                selectionRect.width = nameSize.x + 1 + extSize.x;
            
                var offsetRect = new Rect(selectionRect.position, selectionRect.size);
                EditorGUI.LabelField(offsetRect, message, _style);
            }
        }

        private static bool IsSingleColumnView {
            get {
                var projectWindow = GetProjectWindow();
                var columnsCount = (int) projectWindow.GetType().GetField("m_ViewMode", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(projectWindow);
                return columnsCount == 0;
            }
        }
        
        private static EditorWindow GetProjectWindow() {
            if (EditorWindow.focusedWindow != null && EditorWindow.focusedWindow.titleContent.text == "Project") {
                return EditorWindow.focusedWindow;
            }

            return GetExistingWindowByName("Project");
        }
        
        private static EditorWindow GetExistingWindowByName(string name) {
            EditorWindow[] windows = Resources.FindObjectsOfTypeAll<EditorWindow>();
            foreach (EditorWindow item in windows) {
                if (item.titleContent.text == name) {
                    return item;
                }
            }

            return default(EditorWindow);
        }
        
    }
}

