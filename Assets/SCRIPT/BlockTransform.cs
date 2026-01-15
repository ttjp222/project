using UnityEngine;

public class BlockTransform : MonoBehaviour
{
    [Header("変換条件")]
    public string requiredItem = "おの";
    public float detectionRange = 2f;

    [Header("変換後オブジェクト（子）")]
    public GameObject afterBlock; // ★ 子オブジェクト（最初は非表示）

    [Header("一意のID")]
    public string blockID;

    private Transform player;
    private bool isNearby = false;
    private bool isTransformed = false;

    void OnValidate()
    {
        if (string.IsNullOrEmpty(blockID))
        {
            blockID = gameObject.name + "_" +
                      transform.position.x.ToString("F2") + "_" +
                      transform.position.y.ToString("F2");
        }
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        // 変換後は必ず非表示から
        if (afterBlock != null)
            afterBlock.SetActive(false);

        // すでに変換済みなら即切り替え
        if (GameManager.Instance != null &&
            GameManager.Instance.IsTransformed(blockID))
        {
            ApplyTransformedState();
        }
    }

    void Update()
    {
        if (player == null || isTransformed) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= detectionRange && !isNearby)
        {
            isNearby = true;
            Debug.Log($"{requiredItem} を使用できます");
        }
        else if (distance > detectionRange && isNearby)
        {
            isNearby = false;
        }
    }

    // ★ アイテム使用時に呼ぶ
    public void TransformBlock()
    {
        if (isTransformed) return;

        isTransformed = true;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.MarkAsTransformed(blockID);
        }

        ApplyTransformedState();

        Debug.Log("ブロックを変換しました : " + blockID);
    }

    void ApplyTransformedState()
    {
        isTransformed = true;

        // 変更前（このオブジェクト）を非表示
        gameObject.SetActive(false);

        // 変更後を表示
        if (afterBlock != null)
            afterBlock.SetActive(true);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
