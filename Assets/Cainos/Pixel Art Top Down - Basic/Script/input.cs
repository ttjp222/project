using UnityEngine;

public class DestroyOnTextInput : MonoBehaviour
{
    public string targetWord = "kiko";
    public float detectionRange = 2f;  // 検出範囲
    private string currentInput = "";
    private Transform player;

    void Start()
    {
        // Playerを探す
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (player == null) return;

        // プレイヤーとの距離を計算
        float distance = Vector2.Distance(transform.position, player.position);
        
        // 範囲内にいる時だけ入力を受け付ける
        if (distance <= detectionRange && Input.anyKeyDown)
        {
            foreach (char c in Input.inputString)
            {
                currentInput += c;
                Debug.Log("現在の入力: " + currentInput);
                
                if (currentInput.Contains(targetWord))
                {
                    Debug.Log("一致！オブジェクトを削除");
                    Destroy(gameObject);
                }
            }
        }
        
        // 範囲外に出たら入力をリセット
        if (distance > detectionRange)
        {
            currentInput = "";
        }
    }
}