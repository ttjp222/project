using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangerWithSpawn : MonoBehaviour
{
    public string sceneName;
    public Vector3 spawnPosition;  // 移動先での位置
    public float cooldown = 1f;
    
    private static float lastSceneChangeTime = -999f;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (Time.time - lastSceneChangeTime > cooldown)
            {
                Debug.Log(sceneName + "に移動します");
                lastSceneChangeTime = Time.time;
                
                // スポーン位置を保存
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.scenePlayerPositions[sceneName] = spawnPosition;
                }
                
                SceneManager.LoadScene(sceneName);
            }
        }
    }
    
    // Sceneビューで位置を可視化
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(spawnPosition, 0.5f);
    }
}
