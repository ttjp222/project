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
        // IDが空の場合、自動生成（エディタでのみ実行）
        if (string.IsNullOrEmpty(blockID))
        {
            blockID = gameObject.name + "_" + transform.position.x.ToString("F2") + "_" + transform.position.y.ToString("F2");
        }
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        
        // IDが設定されていない場合は生成
        if (string.IsNullOrEmpty(blockID))
        {
            blockID = gameObject.name + "_" + transform.position.x.ToString("F2") + "_" + transform.position.y.ToString("F2");
        }
        
        Debug.Log("BlockWithItem Start: " + gameObject.name + " / blockID: " + blockID);
        
        // GameManagerに破壊済みか確認
        if (GameManager.Instance != null && GameManager.Instance.IsDestroyed(blockID))
        {
            Debug.Log("このブロックは既に破壊済みです: " + blockID);
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