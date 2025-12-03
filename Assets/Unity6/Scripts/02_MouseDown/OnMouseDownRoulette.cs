using UnityEngine;
using UnityEngine.InputSystem;

// タッチしたら、ルーレットのように回転する
public class OnMouseDownRoulette : MonoBehaviour
{
    //-------------------------------------
    public float maxSpeed = 50; //［最大速度］
    //-------------------------------------
    private float rotateAngle = 0;

    void Update()
    {
        // タッチしたら
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            var ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            var hit = Physics2D.GetRayIntersection(ray, 100f, 1 << gameObject.layer);
            if (hit.collider && hit.collider.gameObject == gameObject)
             {
                rotateAngle = maxSpeed; // 最大スピードを出す
            }
        }
    }

    void FixedUpdate() 
    {
        rotateAngle *= 0.98f;               // 少しずつ減らして
        transform.Rotate(0, 0, rotateAngle); // 回転する
    }
}
