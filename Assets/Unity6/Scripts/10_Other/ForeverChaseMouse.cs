using UnityEngine;
using UnityEngine.InputSystem;

// ずっと、マウスを追いかける
public class ForeverChaseMouse : MonoBehaviour 
{
    //-------------------------------------
    public int drawOrder = 100; //［描画順］
    //-------------------------------------
    void Start () 
    {
        GetComponent<SpriteRenderer>().sortingOrder = drawOrder; // 手前に表示
    }

    void Update () 
    {
        if (Mouse.current == null) return; // マウスがない環境対策

        Vector2 screenPos = Mouse.current.position.ReadValue();  // 新InputSystemのマウス座標
        Vector3 pos = new Vector3(screenPos.x, screenPos.y, 10f); // Zは固定
        transform.position = Camera.main.ScreenToWorldPoint(pos);
    }
}
