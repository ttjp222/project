using UnityEngine;

public class SceneEntryPoint : MonoBehaviour
{
    [Header("Entry Point Settings")]
    public string entryType = "left"; // "left", "right", "up", "down"
    public float spawnX = 0f; // このエントリーポイントのX座標
    public float spawnY = 0f; // このエントリーポイントのY座標（上下用）
    
    [Header("Auto Setup (Optional)")]
    public bool useTransformPosition = true; // このオブジェクトの位置を使う
    
    void Start()
    {
        if (useTransformPosition)
        {
            spawnX = transform.position.x;
            spawnY = transform.position.y;
        }
        
        // このシーンに入った時の処理
        if (SceneTransitionManager.Instance != null)
        {
            string exitDir = SceneTransitionManager.Instance.exitDirection;
            
            // 前のシーンの出口方向と一致するか確認
            bool shouldSpawnHere = false;
            
            if (exitDir == "right" && entryType == "left") shouldSpawnHere = true;
            if (exitDir == "left" && entryType == "right") shouldSpawnHere = true;
            if (exitDir == "up" && entryType == "down") shouldSpawnHere = true;
            if (exitDir == "down" && entryType == "up") shouldSpawnHere = true;
            
            if (shouldSpawnHere)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                {
                    Vector3 newPos = player.transform.position;
                    
                    // 左右の遷移：X座標を変更、Y座標を維持
                    if (exitDir == "right" || exitDir == "left")
                    {
                        newPos.x = spawnX;
                        newPos.y = SceneTransitionManager.Instance.playerYPosition;
                    }
                    // 上下の遷移：Y座標を変更、X座標を維持
                    else if (exitDir == "up" || exitDir == "down")
                    {
                        newPos.y = spawnY;
                        newPos.x = SceneTransitionManager.Instance.playerXPosition;
                    }
                    
                    player.transform.position = newPos;
                    Debug.Log($"プレイヤーを配置: {newPos} (Entry: {entryType})");
                }
            }
        }
    }
}