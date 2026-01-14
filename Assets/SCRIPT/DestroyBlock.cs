using UnityEngine;

public class BlockWithItem : MonoBehaviour
{
    public string requiredItem = "Key";
    public float detectionRange = 2f;
    
    private Transform player;
    private bool isNearby = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        
        if (GameManager.Instance != null && GameManager.Instance.IsDestroyed(gameObject.name))
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);
        
        if (distance <= detectionRange && !isNearby)
        {
            isNearby = true;
            Debug.Log("ブロックにちかづきました。" + requiredItem + " を数字キーで使用してください");
        }
        else if (distance > detectionRange && isNearby)
        {
            isNearby = false;
        }
        
        // Eキーでの使用を削除（数字キーのみに）
    }
}