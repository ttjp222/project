using UnityEngine;
using UnityEngine.InputSystem; 

// タッチすると、そこにプレハブを作る
public class OnMouseDownCreatePrefab : MonoBehaviour 
{
    //-------------------------------------
    public GameObject newPrefab; //［作るプレハブ］
    public int newZ = -5;        //［描画順］
    //-------------------------------------

    void Update() 
    {
        // タッチしたら
        if (Pointer.current != null && Pointer.current.press.wasPressedThisFrame)
        {
            // タッチした位置をカメラの中での位置に変換して
            Vector2 screenPos = Pointer.current.position.ReadValue();
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(
                new Vector3(screenPos.x, screenPos.y, Camera.main.nearClipPlane));
            worldPos.z = newZ;
            // プレハブを作る
            Instantiate(newPrefab, worldPos, Quaternion.identity);
        }
    }
}
