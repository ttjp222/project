using UnityEngine;

public class SceneEntryPoint : MonoBehaviour
{
    [Header("Entry Point Settings")]
    public string entryType = "left";
    public float spawnX = 0f;
    public float spawnY = 0f;
    
    [Header("Auto Setup (Optional)")]
    public bool useTransformPosition = true;
    
    void Start()
    {
        if (useTransformPosition)
        {
            spawnX = transform.position.x;
            spawnY = transform.position.y;
        }
        
        if (SceneTransitionManager.Instance != null)
        {
            string exitDir = SceneTransitionManager.Instance.exitDirection;
            
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
                    
                    if (exitDir == "right" || exitDir == "left")
                    {
                        newPos.x = spawnX;
                        newPos.y = SceneTransitionManager.Instance.playerYPosition;
                    }
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
