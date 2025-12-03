using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]

public class OnMouseClickCountChange : MonoBehaviour
{
	//-------------------------------------
	public CounterType kind = CounterType.Keys; //［カウンターの種類］
	public int addValue = 1; //［増加量］
	//-------------------------------------
    public void OnClick(InputValue value)
    {
        if (value.isPressed)   // 押された瞬間
        {
            GameCounter.counters[kind] = GameCounter.counters[kind] + addValue;
        }
    }
}
