using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    // シーンごとのデータ
    // キー：シーン名、値：そのシーンで消えたオブジェクトのリスト
    public Dictionary<string, List<string>> sceneDestroyedObjects = new Dictionary<string, List<string>>();
    
    // シーンごとのプレイヤー位置
    public Dictionary<string, Vector3> scenePlayerPositions = new Dictionary<string, Vector3>();
    
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
    
    // 現在のシーン名を取得
    string GetCurrentSceneName()
    {
        return SceneManager.GetActiveScene().name;
    }
    
    // オブジェクトが消えたことを記録（現在のシーンのみ）
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
    
    // プレイヤー位置を保存（現在のシーンのみ）
    public void SavePlayerPosition(Vector3 position)
    {
        string sceneName = GetCurrentSceneName();
        scenePlayerPositions[sceneName] = position;
        Debug.Log(sceneName + "のプレイヤー位置を保存: " + position);
    }
    
    // プレイヤー位置を取得（現在のシーンのみ）
    public Vector3 GetPlayerPosition()
    {
        string sceneName = GetCurrentSceneName();
        
        if (scenePlayerPositions.ContainsKey(sceneName))
        {
            return scenePlayerPositions[sceneName];
        }
        return Vector3.zero;  // 保存されていない場合
    }
}