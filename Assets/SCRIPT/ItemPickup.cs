using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [Header("アイテム設定")]
    public string itemName = "あいてむ";
    public Sprite itemSprite;
    public float detectionRange = 1.5f;
    
    private Transform player;
    private bool isPickedUp = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        
        if (player == null)
        {
            Debug.LogError("Playerが見つかりません！");
            return;
        }
        
        // Spriteが設定されていない場合は自動取得
        if (itemSprite == null)
        {
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            if (sr != null && sr.sprite != null)
            {
                itemSprite = sr.sprite;
            }
        }
        
        // このアイテムが既に取得済みかチェック
        if (GameManager.Instance != null && GameManager.Instance.IsDestroyed(gameObject.name))
        {
            Destroy(gameObject);
            return;
        }
    }

    void Update()
    {
        if (player == null || isPickedUp) return;

        float distance = Vector2.Distance(transform.position, player.position);
        
        // Eキーでアイテムを拾う
        if (distance <= detectionRange && Input.GetKeyDown(KeyCode.E))
        {
            PickupItem();
        }
    }

    void PickupItem()
    {
        if (isPickedUp) return;
        
        isPickedUp = true;
        
        // GameManagerにアイテムを追加
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddItem(itemName, itemSprite);
            GameManager.Instance.RegisterDestroyed(gameObject.name);
        }
        
        // InventoryUIに追加
        if (InventoryUI.Instance != null)
        {
            InventoryUI.Instance.AddItemToUI(itemName, itemSprite);
        }
        
        // 取得メッセージ表示
        if (MessageDisplay.Instance != null)
        {
            MessageDisplay.Instance.ShowMessage(itemName + " をひろった");
        }
        
        Debug.Log(itemName + " を取得しました");
        
        Destroy(gameObject);
    }
    
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
