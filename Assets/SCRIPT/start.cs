using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance { get; private set; }
    
    public string exitDirection = "";
    public float playerYPosition = 0f;
    public float playerXPosition = 0f;
    
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
    
    public void TransitionToScene(string sceneName, string direction, Vector3 playerPosition)
    {
        exitDirection = direction;
        playerYPosition = playerPosition.y;
        playerXPosition = playerPosition.x;
        
        Debug.Log($"シーン遷移: {sceneName}, 方向: {direction}, 座標: ({playerXPosition}, {playerYPosition})");
        
        SceneManager.LoadScene(sceneName);
    }
}