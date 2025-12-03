using UnityEngine;
using UnityEngine.InputSystem;

// タッチしたら、他のものを非表示にする
public class OnMouseDownOtherHide : MonoBehaviour
{
    //-------------------------------------
    public GameObject hideObject; //［消すオブジェクト］
    //-------------------------------------

    void Update()
    {
        // タッチしたら
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            var ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            var hit = Physics2D.GetRayIntersection(ray, 100f, 1 << gameObject.layer);
            if (hit.collider && hit.collider.gameObject == gameObject)
             {
                hideObject.SetActive(false); // 他のものを消す
            }
        }
    }
}
