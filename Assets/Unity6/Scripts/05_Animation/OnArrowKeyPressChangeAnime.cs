using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]

// 方向キーを押したら、アニメーションを切り替える
public class OnArrowKeyPressChangeAnime : MonoBehaviour
{
    //-------------------------------------
    public string upAnime = "";      //［上向きアニメ］
    public string downAnime = "";    //［下向きアニメ］
    public string rightAnime = "";   //［右向きアニメ（左右共通）］
    //-------------------------------------
    private string nowMode = "";
    private string oldMode = "";
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
        nowMode = downAnime;
    }

    public void OnMove(InputValue value)
    {
        Vector2 moveInput = value.Get<Vector2>();

        if (moveInput.y > 0)
        {
            nowMode = upAnime;
        }
        else if (moveInput.y < 0)
        {
            nowMode = downAnime;
        }
        else if (moveInput.x != 0)
        {
            nowMode = rightAnime; // 左右共通
        }

        if (nowMode != oldMode)
        {
            oldMode = nowMode;
            animator.Play(nowMode); // アニメーション切り替え
        }
    }
}
