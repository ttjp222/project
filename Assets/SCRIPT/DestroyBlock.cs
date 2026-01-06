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
        }
        else if (distance > detectionRange && isNearby)
        {
            isNearby = false;
        }
        
        if (isNearby && Input.GetKeyDown(KeyCode.E))
        {
            TryUseItem();
        }
    }

    void TryUseItem()
    {
        if (GameManager.Instance != null && GameManager.Instance.HasItem(requiredItem))
        {
            GameManager.Instance.UseItem(requiredItem);
            
            // UIから削除
            if (InventoryUI.Instance != null)
            {
                InventoryUI.Instance.RemoveItemFromUI(requiredItem);
            }
            
            GameManager.Instance.RegisterDestroyed(gameObject.name);
            
            Debug.Log(requiredItem + " を使ってブロックを破壊しました！");
            Destroy(gameObject);
        }
        else
        {
            Debug.Log(requiredItem + " が必要です");
        }
    }
}
