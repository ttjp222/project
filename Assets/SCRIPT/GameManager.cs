using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

[System.Serializable]
public class GameData
{
    public List<string> inventory = new List<string>();
    public List<string> destroyedBlocks = new List<string>();
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    public Vector3 playerPosition;
    public Dictionary<string, List<string>> sceneDestroyedObjects = new Dictionary<string, List<string>>();
    public Dictionary<string, Vector3> scenePlayerPositions = new Dictionary<string, Vector3>();
    
    // アイテム名のリスト
    public List<string> inventory = new List<string>();
    
    // アイテム名とSpriteの辞書
    private Dictionary<string, Sprite> itemSprites = new Dictionary<string, Sprite>();
    
    // 永続的に破壊されたブロックのリスト
    private List<string> destroyedBlocks = new List<string>();
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadGame();
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
    
    // ============================================
    // アイテム関連
    // ============================================
    
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
            
            SaveGame();
            Debug.Log(itemName + " を入手しました");
        }
    }
    
    // アイテムを持っているかチェック
    public bool HasItem(string itemName)
    {
        return inventory.Contains(itemName);
    }
    
    // アイテムのSpriteを取得
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
            
            SaveGame();
            Debug.Log(itemName + " を使用しました");
        }
    }
    
    // ============================================
    // シーン内一時破壊（元からあった機能）
    // ============================================
    
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
    
    // ============================================
    // ブロック永続破壊（新機能）
    // ============================================
    
    // ブロックを永続的に破壊済みとしてマーク
    public void MarkAsDestroyed(string blockID)
    {
        if (!destroyedBlocks.Contains(blockID))
        {
            destroyedBlocks.Add(blockID);
            SaveGame();
            Debug.Log(blockID + " を永続破壊リストに追加しました");
        }
    }
    
    // ブロックが破壊済みかチェック（永続破壊とシーン内破壊の両方をチェック）
    public bool IsDestroyed(string objectName)
    {
        // 永続破壊リストをチェック
        if (destroyedBlocks.Contains(objectName))
        {
            return true;
        }
        
        // シーン内一時破壊もチェック（元の機能を維持）
        string sceneName = GetCurrentSceneName();
        if (sceneDestroyedObjects.ContainsKey(sceneName))
        {
            return sceneDestroyedObjects[sceneName].Contains(objectName);
        }
        
        return false;
    }
    
    // ============================================
    // プレイヤー位置管理
    // ============================================
    
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
    
    // ============================================
    // セーブ・ロード
    // ============================================
    
    void SaveGame()
    {
        GameData data = new GameData();
        data.inventory = new List<string>(inventory);
        data.destroyedBlocks = new List<string>(destroyedBlocks);
        
        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString("GameData", json);
        PlayerPrefs.Save();
        
        Debug.Log("ゲームデータを保存しました");
    }
    
    void LoadGame()
    {
        if (PlayerPrefs.HasKey("GameData"))
        {
            string json = PlayerPrefs.GetString("GameData");
            GameData data = JsonUtility.FromJson<GameData>(json);
            
            inventory = data.inventory != null ? data.inventory : new List<string>();
            destroyedBlocks = data.destroyedBlocks != null ? data.destroyedBlocks : new List<string>();
            
            Debug.Log("ゲームデータを読み込みました");
            Debug.Log("インベントリ数: " + inventory.Count);
            Debug.Log("破壊済みブロック数: " + destroyedBlocks.Count);
        }
    }
    
    // デバッグ用：セーブデータをリセット
    public void ResetSaveData()
    {
        PlayerPrefs.DeleteKey("GameData");
        inventory.Clear();
        destroyedBlocks.Clear();
        itemSprites.Clear();
        Debug.Log("セーブデータをリセットしました");
    }
}