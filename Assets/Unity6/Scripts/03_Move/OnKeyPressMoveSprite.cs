using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]

// キーを押すと、スプライトが移動する
public class OnKeyPressMoveSprite : MonoBehaviour 
{
	//-------------------------------------
	public float speed = 5f; //［速度］
	//-------------------------------------
    private Vector2 moveInput;
    private SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
        // 移動方向にキャラを向ける
        if (moveInput.x != 0) sr.flipX = moveInput.x < 0;
    }

    void FixedUpdate()
    {
        // 位置を直接更新して移動
        Vector2 delta = moveInput * speed * Time.fixedDeltaTime;
        transform.Translate(delta.x, delta.y, 0);
    }
}
