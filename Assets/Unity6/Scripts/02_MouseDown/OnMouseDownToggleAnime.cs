using UnityEngine;
using UnityEngine.InputSystem;

// タッチしたら、アニメーションの再生と停止を切り替える
public class OnMouseDownToggleAnime : MonoBehaviour 
{
    //-------------------------------------
    public float speed = 0; //［速度］
    //-------------------------------------

    void Start ()
    {
        this.GetComponent<Animator>().speed = speed;
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
                speed = 1 - speed;               // 再生と停止を切り換える
                GetComponent<Animator>().speed = speed;
            }
        }
    }
}

