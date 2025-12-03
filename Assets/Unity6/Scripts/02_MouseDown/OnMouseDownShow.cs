using UnityEngine;
using UnityEngine.InputSystem;

// タッチしたら、オブジェクトを表示する
public class OnMouseDown_Show : MonoBehaviour 
{
    //-------------------------------------
    public GameObject showObject; //［表示するオブジェクト］
    //-------------------------------------

    void Start()
    {
        showObject.SetActive(false); // 非表示にする
    }

    // タッチしたら
    void Update()
    {
        if (Mouse.current?.leftButton.wasPressedThisFrame == true)
        {
            var ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            var hit = Physics2D.GetRayIntersection(ray, 100f, 1 << gameObject.layer);

            if (hit.collider && hit.collider.gameObject == gameObject)
            {
                if (showObject != null) showObject.SetActive(true); // 表示
            }
        }
    }
}
