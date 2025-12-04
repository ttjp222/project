using UnityEngine;
using UnityEngine.SceneManagement;

public class OnCollisionSwitchScene : MonoBehaviour
{
    public string sceneName = "stage2";

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("何かと衝突しました: " + collision.gameObject.name);
        
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Playerと衝突！シーン切り替え");
            SceneManager.LoadScene(sceneName);
        }
    }
}