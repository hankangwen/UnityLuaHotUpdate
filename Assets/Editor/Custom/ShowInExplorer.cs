#if !UNITY_EDITOR_OSX
 
using System.IO;
using System;
using System.Text;
using System.ComponentModel;
using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;
using UnityEditor;
 
public static class ShowInExplorer
{
    [DllImport("kernel32.dll", EntryPoint = "CreateFileW", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern SafeFileHandle CreateFile(string lpFileName, int dwDesiredAccess, int dwShareMode, IntPtr SecurityAttributes, int dwCreationDisposition, int dwFlagsAndAttributes, IntPtr hTemplateFile);
 
    [DllImport("kernel32.dll", EntryPoint = "GetFinalPathNameByHandleW", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern int GetFinalPathNameByHandle([In] IntPtr hFile, [Out] StringBuilder lpszFilePath, [In] int cchFilePath, [In] int dwFlags);
 
    private const int CREATION_DISPOSITION_OPEN_EXISTING = 3;
    private const int FILE_FLAG_BACKUP_SEMANTICS = 0x02000000;
 
    private static string GetRealPath(string path)
    {
        if (!Directory.Exists(path) && !File.Exists(path))
        {
            throw new IOException("Path not found");
        }
 
        DirectoryInfo symlink = new DirectoryInfo(path);// No matter if it's a file or folder
        SafeFileHandle directoryHandle = CreateFile(symlink.FullName, 0, 2, System.IntPtr.Zero, CREATION_DISPOSITION_OPEN_EXISTING, FILE_FLAG_BACKUP_SEMANTICS, System.IntPtr.Zero); //Handle file / folder
 
        if (directoryHandle.IsInvalid)
        {
            throw new Win32Exception(Marshal.GetLastWin32Error());
        }
 
        StringBuilder result = new StringBuilder(512);
        int mResult = GetFinalPathNameByHandle(directoryHandle.DangerousGetHandle(), result, result.Capacity, 0);
 
        if (mResult < 0)
        {
            throw new Win32Exception(Marshal.GetLastWin32Error());
        }
 
        if (result.Length >= 4 && result[0] == '\\' && result[1] == '\\' && result[2] == '?' && result[3] == '\\')
        {
            return result.ToString().Substring(4); // "\\?\" remove
        }
        else
        {
            return result.ToString();
        }
    }
 
    [MenuItem("Assets/Show in Explorer(Link) %K", false, 18)]
    private static void openInExplore()
    {
        string path = AssetDatabase.GUIDToAssetPath(Selection.assetGUIDs[0]);
        path = GetRealPath(path);
        string param = string.Format("/select,{0}", path);
        System.Diagnostics.Process.Start("explorer.exe", param);
    }
}
 
#endif