using UnityEngine;

public class BlockWithItem : MonoBehaviour
{
    public string requiredItem = "Key";
    public float detectionRange = 2f;
    
    [Header("一意のID（自動生成）")]
    public string blockID;
    
    private Transform player;
    private bool isNearby = false;

    void OnValidate()
    {
        // IDが空の場合、自動生成
        if (string.IsNullOrEmpty(blockID))
        {
            blockID = gameObject.name + "_" + transform.position.x + "_" + transform.position.y;
        }
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        
        // IDが設定されていない場合は生成
        if (string.IsNullOrEmpty(blockID))
        {
            blockID = gameObject.name + "_" + transform.position.x + "_" + transform.position.y;
        }
        
        // GameManagerに破壊済みか確認
        if (GameManager.Instance != null && GameManager.Instance.IsDestroyed(blockID))
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
    }

    // ブロックが破壊されたときに呼ばれる
    public void DestroyBlock()
    {
        // GameManagerに破壊を記録
        if (GameManager.Instance != null)
        {
            GameManager.Instance.MarkAsDestroyed(blockID);
        }
        
        Destroy(gameObject);
    }
}