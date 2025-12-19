using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// シーン遷移を管理するスクリプト
/// </summary>
public class SceneController : MonoBehaviour
{

    /// <summary>
    /// 初期化処理
    /// </summary>
    private void Start()
    {
    
    }

    /// <summary>
    /// 指定された名前のシーンに遷移する処理
    /// </summary>
    public void LoadScene(string targetSceneName)
    {
        // 指定されたシーン名のシーンに遷移
        SceneManager.LoadScene(targetSceneName);
    }
    
    
    /// <summary>
    /// ゲームを終了する処理
    /// </summary>
    public void OnQuitButtonClick()
    {
        Debug.Log("ゲームを終了します");
#if UNITY_EDITOR
        // エディタの場合
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // ビルドされたゲームの場合
        Application.Quit();
#endif
    }

}