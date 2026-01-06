using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    // プレイヤーの位置を保存
    public Vector3 playerPosition;
    
    // 消えたオブジェクトの名前リスト
    public Dictionary<string, List<string>> sceneDestroyedObjects = new Dictionary<string, List<string>>();
    
    // シーンごとのプレイヤー位置
    public Dictionary<string, Vector3> scenePlayerPositions = new Dictionary<string, Vector3>();
    
    // アイテムのリスト
    public List<string> inventory = new List<string>();
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    string GetCurrentSceneName()
    {
        return SceneManager.GetActiveScene().name;
    }
    
    // アイテムを追加
    public void AddItem(string itemName)
    {
        if (!inventory.Contains(itemName))
        {
            inventory.Add(itemName);
            Debug.Log(itemName + " を入手しました");
        }
    }
    
    // アイテムを持っているかチェック
    public bool HasItem(string itemName)
    {
        return inventory.Contains(itemName);
    }
    
    // アイテムを使用（削除）
    public void UseItem(string itemName)
    {
        if (inventory.Contains(itemName))
        {
            inventory.Remove(itemName);
            Debug.Log(itemName + " を使用しました");
        }
    }
    
    // オブジェクトが消えたことを記録
    public void RegisterDestroyed(string objectName)
    {
        string sceneName = GetCurrentSceneName();
        
        if (!sceneDestroyedObjects.ContainsKey(sceneName))
        {
            sceneDestroyedObjects[sceneName] = new List<string>();
        }
        
        if (!sceneDestroyedObjects[sceneName].Contains(objectName))
        {
            sceneDestroyedObjects[sceneName].Add(objectName);
            Debug.Log(sceneName + "で" + objectName + "を記録");
        }
    }
    
    // このシーンでこのオブジェクトは消えているか？
    public bool IsDestroyed(string objectName)
    {
        string sceneName = GetCurrentSceneName();
        
        if (sceneDestroyedObjects.ContainsKey(sceneName))
        {
            return sceneDestroyedObjects[sceneName].Contains(objectName);
        }
        return false;
    }
    
    // プレイヤー位置を保存
    public void SavePlayerPosition(Vector3 position)
    {
        string sceneName = GetCurrentSceneName();
        scenePlayerPositions[sceneName] = position;
    }
    
    // プレイヤー位置を取得
    public Vector3 GetPlayerPosition()
    {
        string sceneName = GetCurrentSceneName();
        
        if (scenePlayerPositions.ContainsKey(sceneName))
        {
            return scenePlayerPositions[sceneName];
        }
        return Vector3.zero;
    }
}