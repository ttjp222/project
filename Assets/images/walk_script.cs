using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerMovement : MonoBehaviour
{
    //-------------------------------------
    [Header("Movement Settings")]
    public float speed = 3f;

    [Header("Input System")]
    public bool useNewInputSystem = true; // trueならInput System、falseなら旧Input
    //-------------------------------------

    private Animator anim;
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    private Vector2 moveInput;
    private float moveX;
    private float moveY;
    private float lastMoveX = 0;
    private float lastMoveY = -1;

    // 入力を無効化するフラグ
    public static bool canMove = true;

    // ★★ 追加：押す方向判定用（外部参照OK） ★★
    public Vector2 LastMoveDirection
    {
        get
        {
            return new Vector2(lastMoveX, lastMoveY).normalized;
        }
    }

    void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        // 重力と回転を無効化
        rb.gravityScale = 0;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void Start()
    {
        // SceneTransitionがある場合は何もしない
        if (SceneTransitionManager.Instance != null &&
            !string.IsNullOrEmpty(SceneTransitionManager.Instance.exitDirection))
        {
            Debug.Log("[PlayerMovement] SceneTransition使用中 - 位置復元スキップ");
            return;
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

    // 新Input System用
    public void OnMove(InputValue value)
    {
        if (useNewInputSystem && canMove)
        {
            moveInput = value.Get<Vector2>();
            moveX = moveInput.x;
            moveY = moveInput.y;

            // 移動している時は方向を記憶
            if (moveX != 0 || moveY != 0)
            {
                lastMoveX = moveX;
                lastMoveY = moveY;
            }
        }
        else if (!canMove)
        {
            moveInput = Vector2.zero;
            moveX = 0;
            moveY = 0;
        }
    }

    void Update()
    {
        // 旧Input Managerを使う場合
        if (!useNewInputSystem)
        {
            if (canMove)
            {
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
                moveX = 0;
                moveY = 0;
            }
        }

        // Animatorがあれば更新
        if (anim != null)
        {
            anim.SetFloat("MoveX", lastMoveX);
            anim.SetFloat("MoveY", lastMoveY);
        }
    }

    void LateUpdate()
    {
        // 左右の向きを保持
        if (moveX != 0)
        {
            sr.flipX = moveX < 0;
        }
        // lastMoveXも使う（止まっている時用）
        else if (lastMoveX < 0)
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
        // 移動処理（canMoveがfalseなら自動的に0になる）
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
