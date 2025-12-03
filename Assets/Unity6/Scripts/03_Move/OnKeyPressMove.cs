using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]

// キーを押すと、移動する 
public class OnKeyPressMove : MonoBehaviour 
{
	//-------------------------------------
	public float speed = 5f; //［速度］
	//-------------------------------------
    private Vector2 moveInput;
    private Rigidbody2D rbody;
    private SpriteRenderer sr;

    void Awake()
    {
        rbody = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        rbody.gravityScale = 0;
        // 回転しないようにする
        rbody.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
        // 移動方向にキャラを向ける
        if (moveInput.x != 0) sr.flipX = moveInput.x < 0;
    }

    void FixedUpdate()
    {
        // 物理的に勢い重視で移動（速度を与えて移動）
        rbody.linearVelocity = moveInput * speed;
    }
}
