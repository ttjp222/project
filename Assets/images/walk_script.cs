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
    float lastMoveY = -1;  // 初期値は下向き

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
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
}