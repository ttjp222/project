using UnityEngine;

public class ObjectReplacer : MonoBehaviour
{
    [Header("Settings")]
    public GameObject beforeObject;  // 変化前のオブジェクト（このオブジェクト自身でもOK）
    public GameObject afterObject;   // 変化後のオブジェクト
    public bool needsKeyPress = false; // Eキーを押す必要があるか
    
    private bool isPlayerNear = false;
    private bool hasChanged = false;

    void Start()
    {
        // 最初はafterObjectを非表示
        if (afterObject != null)
            afterObject.SetActive(false);
    }

    void Update()
    {
        if (isPlayerNear && needsKeyPress && Input.GetKeyDown(KeyCode.E) && !hasChanged)
        {
            ChangeObject();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;
            
            if (!needsKeyPress && !hasChanged)
            {
                ChangeObject();
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
        }
    }

    void ChangeObject()
    {
        hasChanged = true;
        
        // beforeObjectを消す
        if (beforeObject != null)
        {
            beforeObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(false);
        }
        
        // afterObjectを表示
        if (afterObject != null)
        {
            afterObject.SetActive(true);
            Debug.Log("オブジェクトが変化しました");
        }
    }
}