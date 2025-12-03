using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]

// ジャンプキーを押したら、アニメーションを切り換える
public class OnJumpKeyPressChangeAnime : MonoBehaviour
{
    //-------------------------------------
    public string AnimeName = "";         //［ジャンプアニメ］
    public string defaultAnimeName = "";  //［元のアニメ］
    //-------------------------------------
    private Animator animator;
    private bool isPlaying = false;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void OnJump()
    {
        if (!isPlaying)
        {
            isPlaying = true;
            animator.Play(AnimeName); // ジャンプアニメ再生
        }
    }

    void Update()
    {
        if (isPlaying && IsAnimationFinished(AnimeName))
        {
            isPlaying = false;
            animator.Play(defaultAnimeName); // 元のアニメに戻す
        }
    }

    bool IsAnimationFinished(string animationName)
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        return stateInfo.IsName(animationName) && stateInfo.normalizedTime >= 1.0f;
    }
}
