using UnityEngine;
using UnityEngine.InputSystem;

// タッチしたら、ゲームを止める
public class OnMouseDownStopGame : MonoBehaviour
{
    void Start ()
    {
        Time.timeScale = 1; // 時間を動かす
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
                Time.timeScale = 0; // 時間を止める
            }
        }
    }
}
