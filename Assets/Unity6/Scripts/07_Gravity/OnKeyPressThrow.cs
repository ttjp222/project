using UnityEngine;
using UnityEngine.InputSystem;

// キーを押すと、プレハブを作って投げる
public class OnKeyPressThrow : MonoBehaviour 
{
	//-------------------------------------
	public GameObject newPrefab; //［作るプレハブ］
    public Vector2 power = new Vector2(6, 6); //［投げる力］
    public Vector2 offset = new Vector2(0.7f, 1); //［オフセット］
    public int newZ = -5; //［描画順］
	//-------------------------------------
   private PlayerInput playerInput;
    private InputAction attackAction;
    private bool leftFlag = false;
    private bool pushFlag = false;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        attackAction = playerInput.actions["Attack"];
    }

    void Update()
    {
        if (!attackAction.IsPressed()) pushFlag = false;
    }

    public void OnMove(InputValue value)
    {
        Vector2 moveInput = value.Get<Vector2>();
        if (moveInput.x < 0) leftFlag = true;
        else if (moveInput.x > 0) leftFlag = false;
    }

    public void OnAttack()
    {
        if (!pushFlag)
        {
            pushFlag = true;

            // プレハブ生成
            GameObject newGO = Instantiate(newPrefab);
            Vector3 pos = transform.position;
            pos.x += leftFlag ? -offset.x : offset.x;
            pos.y += offset.y;
            pos.z = newZ;
            newGO.transform.position = pos;

            // 力を加える
            Rigidbody2D rbody = newGO.GetComponent<Rigidbody2D>();
            Vector2 force = new Vector2(leftFlag ? -power.x : power.x, power.y);
            rbody.AddForce(force, ForceMode2D.Impulse);
        }
    }
}
