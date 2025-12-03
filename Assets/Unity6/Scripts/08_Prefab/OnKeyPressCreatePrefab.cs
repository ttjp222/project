using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]

// キーを押すと、近くにプレハブを作る
public class OnKeyPressCreatePrefab : MonoBehaviour 
{
    //-------------------------------------
    public GameObject newPrefab;                 //［作るプレハブ］
    public Vector2 offset = new Vector2(0.7f, 0); //［オフセット］
    public int newZ = -5;                         //［描画順］
    //-------------------------------------
    private Vector2 moveInput;
    private bool leftFlag = false;
    private bool pushFlag = false;

    void Update()
    {
        if (moveInput.x < 0)      leftFlag = true;
        else if (moveInput.x > 0) leftFlag = false;
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

	public void OnAttack(InputValue value)
	{
		if (pushFlag) return;
		pushFlag = true;

		var go = Instantiate(newPrefab);
		var pos = transform.position;
		pos.x += leftFlag ? -offset.x : offset.x;
		pos.y += offset.y;
		pos.z = newZ;
		go.transform.position = pos;
	}

	void LateUpdate()
	{
		bool keyPressed = Keyboard.current?.zKey?.isPressed ?? false;
		bool padPressed = Gamepad.current?.buttonSouth?.isPressed ?? false;
		if (!keyPressed && !padPressed)
		{
			pushFlag = false;
		}
	}
}