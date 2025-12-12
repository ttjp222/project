using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public string sceneName;
    private static float lastSceneChangeTime = -999f;  // staticを戻す
    public float cooldown = 0.00000000000001f;  // 少し短めに

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (Time.time - lastSceneChangeTime > cooldown)
            {
                Debug.Log(sceneName + "に移動します");
                lastSceneChangeTime = Time.time;
                SceneManager.LoadScene(sceneName);
            }
        }
    }
}