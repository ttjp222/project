using UnityEngine;

public class DestroyOnTextInput : MonoBehaviour
{
    public string targetWord = "kiko";
    public float detectionRange = 2f;
    private string currentInput = "";
    private Transform player;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
            Debug.Log("Player見つかりました");
        }
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);
        
        if (distance <= detectionRange)
        {
            // K, I, O のキーを個別に検出
            if (Input.GetKeyDown(KeyCode.K))
            {
                currentInput += "k";
                Debug.Log("現在の入力: " + currentInput);
            }
            if (Input.GetKeyDown(KeyCode.I))
            {
                currentInput += "i";
                Debug.Log("現在の入力: " + currentInput);
            }
            if (Input.GetKeyDown(KeyCode.O))
            {
                currentInput += "o";
                Debug.Log("現在の入力: " + currentInput);
            }
            
            // Enterで確定
            if (Input.GetKeyDown(KeyCode.Return))
            {
                Debug.Log("入力確定: " + currentInput);
                if (currentInput == targetWord)
                {
                    Debug.Log("一致！削除");
                    Destroy(gameObject);
                }
                currentInput = "";
            }
        }
        else
        {
            currentInput = "";
        }
    }
}