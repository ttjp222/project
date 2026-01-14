using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI Instance;
    
    public GameObject inventoryPanel;
    public GameObject itemSlotPrefab;
    public Transform itemContainer;
    
    [Header("Confirmation Dialog")]
    public GameObject confirmDialog;
    public Text confirmText;
    public Button confirmYesButton;
    public Button confirmNoButton;
    
    private Dictionary<string, GameObject> itemSlots = new Dictionary<string, GameObject>();
    private List<string> itemOrder = new List<string>();
    private string pendingItemName = "";
    private PlayerMovement playerMovement;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerMovement = player.GetComponent<PlayerMovement>();
        }
        
        if (confirmDialog != null)
        {
            confirmDialog.SetActive(false);
        }
        
        if (confirmYesButton != null)
        {
            confirmYesButton.onClick.AddListener(OnConfirmYes);
        }
        if (confirmNoButton != null)
        {
            confirmNoButton.onClick.AddListener(OnConfirmNo);
        }
        
        if (GameManager.Instance != null)
        {
            foreach (string itemName in GameManager.Instance.inventory)
            {
                Sprite itemSprite = GameManager.Instance.GetItemSprite(itemName);
                AddItemToUI(itemName, itemSprite);
            }
        }
    }
    
    void Update()
    {
        if (confirmDialog != null && confirmDialog.activeSelf)
        {
            return;
        }
        
        for (int i = 1; i <= 9; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha0 + i))
            {
                UseItemByNumber(i);
            }
        }
    }
    
    void UseItemByNumber(int number)
    {
        int index = number - 1;
        
        if (index >= 0 && index < itemOrder.Count)
        {
            string itemName = itemOrder[index];
            ShowConfirmDialog(itemName);
        }
        else
        {
            Debug.Log("アイテム" + number + "は存在しません");
        }
    }
    
    void ShowConfirmDialog(string itemName)
    {
        pendingItemName = itemName;
        
        if (confirmDialog != null)
        {
            confirmDialog.SetActive(true);
            
            if (playerMovement != null)
            {
                playerMovement.enabled = false;
            }
            
            if (confirmText != null)
            {
                confirmText.text = itemName + " を使いますか？";
            }
        }
    }
    
    void OnConfirmYes()
    {
        if (!string.IsNullOrEmpty(pendingItemName))
        {
            TryUseItem(pendingItemName);
        }
        
        CloseDialog();
    }
    
    void OnConfirmNo()
    {
        Debug.Log("使用をキャンセルしました");
        CloseDialog();
    }
    
    void CloseDialog()
    {
        if (confirmDialog != null)
        {
            confirmDialog.SetActive(false);
        }
        
        if (playerMovement != null)
        {
            playerMovement.enabled = true;
        }
        
        pendingItemName = "";
    }
    
    void TryUseItem(string itemName)
    {
        bool canUse = CheckCanUseItem(itemName);
        
        if (canUse)
        {
            Debug.Log(itemName + " をつかった");
            
            if (GameManager.Instance != null)
            {
                GameManager.Instance.UseItem(itemName);
            }
            
            RemoveItemFromUI(itemName);
            
            ExecuteItemEffect(itemName);
            
            if (MessageDisplay.Instance != null)
            {
                MessageDisplay.Instance.ShowMessage(itemName + " をつかった");
            }
        }
        else
        {
            Debug.Log("ここではつかえない");
            
            if (MessageDisplay.Instance != null)
            {
                MessageDisplay.Instance.ShowMessage("ここではつかえない");
            }
        }
    }
    
    bool CheckCanUseItem(string itemName)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return false;
        
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.transform.position, 2f);
        
        foreach (Collider2D col in colliders)
        {
            BlockWithItem block = col.GetComponent<BlockWithItem>();
            if (block != null && block.requiredItem == itemName)
            {
                return true;
            }
        }
        
        return false;
    }
    
    void ExecuteItemEffect(string itemName)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;
        
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.transform.position, 2f);
        
        foreach (Collider2D col in colliders)
        {
            BlockWithItem block = col.GetComponent<BlockWithItem>();
            if (block != null && block.requiredItem == itemName)
            {
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.RegisterDestroyed(col.gameObject.name);
                }
                Destroy(col.gameObject);
                Debug.Log("ブロックをはかいしました");
                return;
            }
        }
    }
    
    public void AddItemToUI(string itemName, Sprite itemSprite)
    {
        if (itemSlots.ContainsKey(itemName))
        {
            Debug.Log(itemName + " は既に表示されています");
            return;
        }
        
        GameObject newSlot = Instantiate(itemSlotPrefab, itemContainer);
        
        Transform iconTransform = newSlot.transform.Find("ItemIcon");
        Transform textTransform = newSlot.transform.Find("ItemText");
        
        Image icon = null;
        Text text = null;
        
        if (iconTransform != null)
        {
            icon = iconTransform.GetComponent<Image>();
        }
        
        if (textTransform != null)
        {
            text = textTransform.GetComponent<Text>();
        }
        
        if (icon != null && itemSprite != null)
        {
            icon.sprite = itemSprite;
            icon.enabled = true;
        }
        else if (icon != null)
        {
            icon.enabled = false;
        }
        
        if (text != null)
        {
            int itemNumber = itemOrder.Count + 1;
            text.text = itemNumber + ":" + itemName;
        }
        
        itemSlots[itemName] = newSlot;
        itemOrder.Add(itemName);
        
        Debug.Log("インベントリに追加: " + itemName);
    }
    
    public void RemoveItemFromUI(string itemName)
    {
        if (itemSlots.ContainsKey(itemName))
        {
            Destroy(itemSlots[itemName]);
            itemSlots.Remove(itemName);
            itemOrder.Remove(itemName);
            
            UpdateItemNumbers();
            
            Debug.Log("インベントリから削除: " + itemName);
        }
    }
    
    void UpdateItemNumbers()
    {
        for (int i = 0; i < itemOrder.Count; i++)
        {
            string itemName = itemOrder[i];
            if (itemSlots.ContainsKey(itemName))
            {
                GameObject slot = itemSlots[itemName];
                Transform textTransform = slot.transform.Find("ItemText");
                if (textTransform != null)
                {
                    Text text = textTransform.GetComponent<Text>();
                    if (text != null)
                    {
                        text.text = (i + 1) + ":" + itemName;
                    }
                }
            }
        }
    }
    
    public void ClearInventory()
    {
        foreach (var slot in itemSlots.Values)
        {
            Destroy(slot);
        }
        itemSlots.Clear();
        itemOrder.Clear();
    }
}
