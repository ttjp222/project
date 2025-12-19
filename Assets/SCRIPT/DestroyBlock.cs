using UnityEngine;

public class BlockWithItem : MonoBehaviour
{
    public string requiredItem = "Key";  // 必要なアイテム
    public float detectionRange = 2f;
    
    private Transform player;
    private bool isNearby = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        
        // このブロックが既に消えていたら非表示
        if (THEGameManager.Instance != null && THEGameManager.Instance.IsDestroyed(gameObject.name))
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
            Debug.Log("ブロックに近づきました。" + requiredItem + " を使うにはEキーを押してください");
        }
        else if (distance > detectionRange && isNearby)
        {
            isNearby = false;
        }
        
        // 近くにいて、アイテムを持っている場合
        if (isNearby && Input.GetKeyDown(KeyCode.E))
        {
            TryUseItem();
        }
    }

    void TryUseItem()
    {
        if (THEGameManager.Instance != null && THEGameManager.Instance.HasItem(requiredItem))
        {
            // アイテムを使用
            THEGameManager.Instance.UseItem(requiredItem);
            
            // ブロックを記録して削除
            THEGameManager.Instance.RegisterDestroyed(gameObject.name);
            
            Debug.Log(requiredItem + " を使ってブロックを破壊しました！");
            Destroy(gameObject);
        }
        else
        {
            Debug.Log(requiredItem + " が必要です");
        }
    }
}