using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public string itemName = "Key";  // アイテムの名前
    public float detectionRange = 1.5f;
    
    private Transform player;
    private bool isPickedUp = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        
        // このアイテムが既に取得済みなら非表示
        if (THEGameManager.Instance != null && THEGameManager.Instance.HasItem(itemName))
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (player == null || isPickedUp) return;

        float distance = Vector2.Distance(transform.position, player.position);
        
        if (distance <= detectionRange)
        {
            // Eキーで取得
            if (Input.GetKeyDown(KeyCode.E))
            {
                PickupItem();
            }
        }
    }

    void PickupItem()
    {
        isPickedUp = true;
        
        // GameManagerに記録
        if (THEGameManager.Instance != null)
        {
            THEGameManager.Instance.AddItem(itemName);
        }
        
        Debug.Log(itemName + " を取得しました！");
        Destroy(gameObject);
    }
    
    // 視覚的なフィードバック（オプション）
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}