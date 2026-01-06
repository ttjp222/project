using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public string itemName = "鍵";  // ★ここで日本語名を設定★
    public Sprite itemSprite;
    public float detectionRange = 1.5f;
    
    private Transform player;
    private bool isPickedUp = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        
        if (itemSprite == null)
        {
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                itemSprite = sr.sprite;
            }
        }
        
        if (GameManager.Instance != null && GameManager.Instance.HasItem(itemName))
        {
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
        
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddItem(itemName);
        }
        
        if (InventoryUI.Instance != null)
        {
            InventoryUI.Instance.AddItemToUI(itemName, itemSprite);
        }
        
        Debug.Log(itemName + " を取得しました！");
        Destroy(gameObject);
    }
    
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
