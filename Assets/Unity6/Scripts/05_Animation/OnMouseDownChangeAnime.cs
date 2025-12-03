using UnityEngine;
using UnityEngine.InputSystem;

// マウスでクリックしたら、アニメーションを切り替える
public class OnMouseDownChangeAnime : MonoBehaviour 
{
    //-------------------------------------
    public string normalAnime = "";  //［通常アニメ］
    public string nextAnime   = "";  //［次のアニメ］
    //-------------------------------------
    private bool touchFlag = false;
    private Animator animator;

    void Awake()    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // タッチしたら
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            var ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            var hit = Physics2D.GetRayIntersection(ray, 100f, 1 << gameObject.layer);
            if (hit.collider && hit.collider.gameObject == gameObject)
            {
                touchFlag = !touchFlag;
                if (touchFlag){
                    animator.Play(nextAnime);
                }
                else{
                    animator.Play(normalAnime);
                }
            }
        }
    }
}
