using UnityEngine;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;

public class SaveTools {

    public static void SaveInPlayerPrefs(string playerPrefsKey, object objectToSave){
        string base64String = ObjectToString(objectToSave);
        PlayerPrefs.SetString(playerPrefsKey, base64String);
        Debug.Log("Saved " + playerPrefsKey + " to PlayerPrefs");
    }

    public static object LoadFromPlayerPrefs(string playerPrefsKey) {
        string base64String = PlayerPrefs.GetString(playerPrefsKey);
        Debug.Log("Loaded " + playerPrefsKey + " from PlayerPrefs");
        return StringToObject(base64String);
    }

    public static string ObjectToString(object obj) {
        using (MemoryStream ms = new MemoryStream()) {
            new BinaryFormatter().Serialize(ms, obj);
            return Convert.ToBase64String(ms.ToArray());
        }
    }

    public static object StringToObject(string base64String) {
        byte[] bytes = Convert.FromBase64String(base64String);
        using (MemoryStream ms = new MemoryStream(bytes, 0, bytes.Length)) {
            ms.Write(bytes, 0, bytes.Length);
            ms.Position = 0;
            return new BinaryFormatter().Deserialize(ms);
        }
    }

}
