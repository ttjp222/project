using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public string sceneName;
    private static float lastSceneChangeTime = -999f;
    public float cooldown = 0.5f;  // 0.5秒で十分です

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (Time.time - lastSceneChangeTime > cooldown)
            {
                Debug.Log(sceneName + "に移動します");
                lastSceneChangeTime = Time.time;
                
                // SceneTransitionManagerの情報をクリア
                if (SceneTransitionManager.Instance != null)
                {
                    SceneTransitionManager.Instance.exitDirection = "";
                    Debug.Log("[SceneChanger] SceneTransition情報をクリア");
                }
                
                SceneManager.LoadScene(sceneName);
            }
        }
    }
}
