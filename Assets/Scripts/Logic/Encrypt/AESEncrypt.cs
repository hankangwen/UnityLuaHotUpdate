using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.Security.Cryptography;

/// <summary>
/// AES���ܽ���
/// </summary>
public class AESEncrypt 
{
    /// <summary>
    /// Ĭ����Կ-��Կ�ĳ��ȱ�����32
    /// </summary>
    private const string PUBLIC_KEY = "Hello_I_am_linxinfa.WelcomeUnity";

    /// <summary>
    /// Ĭ������
    /// </summary>
    private const string IV = "abcdefghijklmnop";

    /// <summary>
    /// AES����
    /// </summary>
    /// <param name="str">��Ҫ���ܵ��ַ���</param>
    /// <param name="key">32λ��Կ</param>
    /// <returns>���ܺ���ַ���</returns>
    public static byte[] Encrypt(byte[] toEncryptArray)
    {
        byte[] keyArray = Encoding.UTF8.GetBytes(PUBLIC_KEY);
        var rijndael = new RijndaelManaged();
        rijndael.Key = keyArray;
        rijndael.Mode = CipherMode.ECB;
        rijndael.Padding = PaddingMode.PKCS7;
        rijndael.IV = Encoding.UTF8.GetBytes(IV);
        ICryptoTransform cTransform = rijndael.CreateEncryptor();
        byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
        return resultArray;
    }

    /// <summary>
    /// AES����
    /// </summary>
    /// <param name="str">��Ҫ���ܵ��ַ���</param>
    /// <param name="key">32λ��Կ</param>
    /// <returns>���ܺ���ַ���</returns>
    public static byte[] Decrypt(byte[] toDecryptArray)
    {
        byte[] keyArray = Encoding.UTF8.GetBytes(PUBLIC_KEY);

        var rijndael = new RijndaelManaged();
        rijndael.Key = keyArray;
        rijndael.Mode = CipherMode.ECB;
        rijndael.Padding = PaddingMode.PKCS7;
        rijndael.IV = Encoding.UTF8.GetBytes(IV);
        ICryptoTransform cTransform = rijndael.CreateDecryptor();
        byte[] resultArray = cTransform.TransformFinalBlock(toDecryptArray, 0, toDecryptArray.Length);
        return resultArray;
    }
}
