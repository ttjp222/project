using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    public Vector3 playerPosition;
    public Dictionary<string, List<string>> sceneDestroyedObjects = new Dictionary<string, List<string>>();
    public Dictionary<string, Vector3> scenePlayerPositions = new Dictionary<string, Vector3>();
    
    // アイテム名のリスト
    public List<string> inventory = new List<string>();
    
    // アイテム名とSpriteの辞書（追加）
    private Dictionary<string, Sprite> itemSprites = new Dictionary<string, Sprite>();
    
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
    
    // アイテムを追加（Sprite付き）
    public void AddItem(string itemName, Sprite itemSprite)
    {
        if (!inventory.Contains(itemName))
        {
            inventory.Add(itemName);
            
            // Spriteも保存
            if (itemSprite != null)
            {
                itemSprites[itemName] = itemSprite;
            }
            
            Debug.Log(itemName + " を入手しました");
        }
    }
    
    // アイテムを持っているかチェック
    public bool HasItem(string itemName)
    {
        return inventory.Contains(itemName);
    }
    
    // アイテムのSpriteを取得（追加）
    public Sprite GetItemSprite(string itemName)
    {
        if (itemSprites.ContainsKey(itemName))
        {
            return itemSprites[itemName];
        }
        return null;
    }
    
    // アイテムを使用（削除）
    public void UseItem(string itemName)
    {
        if (inventory.Contains(itemName))
        {
            inventory.Remove(itemName);
            
            // Spriteも削除
            if (itemSprites.ContainsKey(itemName))
            {
                itemSprites.Remove(itemName);
            }
            
            Debug.Log(itemName + " を使用しました");
        }
    }
    
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
        }
    }
    
    public bool IsDestroyed(string objectName)
    {
        string sceneName = GetCurrentSceneName();
        
        if (sceneDestroyedObjects.ContainsKey(sceneName))
        {
            return sceneDestroyedObjects[sceneName].Contains(objectName);
        }
        return false;
    }
    
    public void SavePlayerPosition(Vector3 position)
    {
        string sceneName = GetCurrentSceneName();
        scenePlayerPositions[sceneName] = position;
    }
    
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