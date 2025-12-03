using UnityEngine;
using UnityEngine.InputSystem;

// タッチすると、カウントを変更する
public class OnMouseDownCountChange : MonoBehaviour 
{
	//-------------------------------------
	public CounterType kind = CounterType.Keys; //［カウンターの種類］
	public int addValue = 1; //［増加量］
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
				// カウンターの値を変更する
				GameCounter.counters[kind] = GameCounter.counters[kind] + addValue;
            }
        }
    }
}

