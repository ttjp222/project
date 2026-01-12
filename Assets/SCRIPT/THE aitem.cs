using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public string itemName = "あいてむ";
    public Sprite itemSprite;
    public float detectionRange = 1.5f;
    
    private Transform player;
    private bool isPickedUp = false;

    void Start()
    {
        Debug.Log("★ItemPickup Start: オブジェクト名=" + gameObject.name + ", アイテム名=" + itemName);
        
        player = GameObject.FindGameObjectWithTag("Player").transform;
        
        if (itemSprite == null)
        {
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                itemSprite = sr.sprite;
                Debug.Log("★Spriteを自動取得: " + itemSprite.name);
            }
        }
        
        if (GameManager.Instance != null && GameManager.Instance.HasItem(itemName))
        {
            Debug.Log("★" + itemName + "は既に取得済みなので削除");
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (player == null || isPickedUp) return;

        float distance = Vector2.Distance(transform.position, player.position);
        
        if (distance <= detectionRange && Input.GetKeyDown(KeyCode.E))
        {
            PickupItem();
        }
    }

    void PickupItem()
    {
        isPickedUp = true;
        
        Debug.Log("★★★ PickupItem開始 ★★★");
        Debug.Log("★拾うアイテム名: 「" + itemName + "」");
        Debug.Log("★Sprite: " + (itemSprite != null ? itemSprite.name : "null"));
        
        // Spriteも一緒に保存
        if (GameManager.Instance != null)
        {
            Debug.Log("★GameManagerに追加: " + itemName);
            GameManager.Instance.AddItem(itemName, itemSprite);
        }
        else
        {
            Debug.LogError("★GameManager.Instance が null です！");
        }
        
        if (InventoryUI.Instance != null)
        {
            Debug.Log("★InventoryUIに追加: " + itemName);
            InventoryUI.Instance.AddItemToUI(itemName, itemSprite);
        }
        else
        {
            Debug.LogError("★InventoryUI.Instance が null です！");
        }
        
        Debug.Log("★" + itemName + " を取得しました！");
        Debug.Log("★★★ PickupItem終了 ★★★");
        
        Destroy(gameObject);
    }
    
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}