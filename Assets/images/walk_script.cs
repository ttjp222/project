using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 3f;
    Animator anim;
    Rigidbody2D rb;
    SpriteRenderer sr;

    float moveX;
    float moveY;

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

        // Blend Tree にパラメータを送る
        anim.SetFloat("MoveX", moveX);
        anim.SetFloat("MoveY", moveY);
    }

    void LateUpdate()
    {
        // 左右の向きを制御（Animatorの後に実行）
        if (moveX < 0)
        {
            sr.flipX = true;
        }
        else if (moveX > 0)
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