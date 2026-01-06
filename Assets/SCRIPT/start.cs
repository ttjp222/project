using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance { get; private set; }
    
    // 遷移情報
    public string exitDirection = ""; // "left", "right", "up", "down"
    public float playerYPosition = 0f; // プレイヤーのY座標
    public float playerXPosition = 0f; // プレイヤーのX座標
    
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
    
    // シーン遷移を実行
    public void TransitionToScene(string sceneName, string direction, Vector3 playerPosition)
    {
        exitDirection = direction;
        playerYPosition = playerPosition.y;
        playerXPosition = playerPosition.x;
        
        Debug.Log($"シーン遷移: {sceneName}, 方向: {direction}, 座標: ({playerXPosition}, {playerYPosition})");
        
        SceneManager.LoadScene(sceneName);
    }
}
