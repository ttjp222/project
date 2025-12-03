using UnityEngine;
using UnityEngine.InputSystem;

// タッチしたら、回転する
public class OnMouseDownRotate : MonoBehaviour
{
    //-------------------------------------
    public float angle = 360; //［角度］
    //-------------------------------------
    private float rotateAngle = 0;

    void Update()
    {
        // タッチしたら
        if (Mouse.current?.leftButton.wasPressedThisFrame == true)
        {
            var ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            var hit = Physics2D.GetRayIntersection(ray, 100f, 1 << gameObject.layer);
            if (hit.collider && hit.collider.gameObject == gameObject)
                rotateAngle = angle; // 押した瞬間 回転開始
        }
        // タッチをやめたら
        if (Mouse.current?.leftButton.wasReleasedThisFrame == true)
        {
            rotateAngle = 0; // 離した瞬間 停止
        }
    }

    void FixedUpdate()
    {
        transform.Rotate(0, 0, rotateAngle * Time.deltaTime); // 回転する
    }
}
