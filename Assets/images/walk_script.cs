using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 3f;
    Animator anim;
    Rigidbody2D rb;
    SpriteRenderer sr;

    float moveX;
    float moveY;
    float lastMoveX = 0;
    float lastMoveY = -1;

    // 入力を無効化するフラグ
    public static bool canMove = true;

    void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        // SceneTransitionがある場合は何もしない
        if (SceneTransitionManager.Instance != null && 
            !string.IsNullOrEmpty(SceneTransitionManager.Instance.exitDirection))
        {
            Debug.Log("[PlayerMovement] SceneTransition使用中 - 位置復元スキップ");
            return; // ここで終了
        }
        
        // SceneTransitionがない場合のみGameManagerから復元
        if (GameManager.Instance != null)
        {
            Vector3 savedPos = GameManager.Instance.GetPlayerPosition();
            if (savedPos != Vector3.zero)
            {
                transform.position = savedPos;
                Debug.Log("[PlayerMovement] プレイヤー位置を復元: " + savedPos);
            }
        }
    }

    void Update()
    {
        // 移動可能な時のみ入力を受け付ける
        if (canMove)
        {
            // 入力取得
            moveX = Input.GetAxisRaw("Horizontal");
            moveY = Input.GetAxisRaw("Vertical");

            // 移動している時は方向を記憶
            if (moveX != 0 || moveY != 0)
            {
                lastMoveX = moveX;
                lastMoveY = moveY;
            }
        }
        else
        {
            // 移動不可の時は入力をゼロに
            moveX = 0;
            moveY = 0;
        }

        // Blend Treeには常に最後の方向を送る
        anim.SetFloat("MoveX", lastMoveX);
        anim.SetFloat("MoveY", lastMoveY);
    }

    void LateUpdate()
    {
        // 左右の向きを保持
        if (lastMoveX < 0)
        {
            sr.flipX = true;
        }
        else if (lastMoveX > 0)
        {
            sr.flipX = false;
        }
    }

    void FixedUpdate()
    {
        // 移動処理
        rb.linearVelocity = new Vector2(moveX, moveY) * speed;
    }
    
    void OnDestroy()
    {
        // SceneTransitionで遷移する場合は位置を保存しない
        if (SceneTransitionManager.Instance != null && 
            !string.IsNullOrEmpty(SceneTransitionManager.Instance.exitDirection))
        {
            Debug.Log("[PlayerMovement] SceneTransition使用中 - 位置保存スキップ");
            return;
        }
        
        // 通常時のみシーンのプレイヤー位置を保存
        if (GameManager.Instance != null)
        {
            GameManager.Instance.SavePlayerPosition(transform.position);
            Debug.Log("[PlayerMovement] プレイヤー位置を保存: " + transform.position);
        }
    }
}
