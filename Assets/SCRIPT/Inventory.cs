using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI Instance;
    
    public GameObject inventoryPanel;
    public GameObject itemSlotPrefab;
    public Transform itemContainer;
    
    private Dictionary<string, GameObject> itemSlots = new Dictionary<string, GameObject>();
    
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
    // GameManagerからアイテムを復元（Sprite付き）
        if (GameManager.Instance != null)
        {
            foreach (string itemName in GameManager.Instance.inventory)
            {
                Sprite itemSprite = GameManager.Instance.GetItemSprite(itemName);
                AddItemToUI(itemName, itemSprite);
            }  
        }
    }
   
    // アイテムをUIに追加
    public void AddItemToUI(string itemName, Sprite itemSprite)
    {
        // 既に表示されていたら追加しない
        if (itemSlots.ContainsKey(itemName))
        {
            Debug.Log(itemName + " は既に表示されています");
            return;
        }
        
        // 新しいスロットを作成
        GameObject newSlot = Instantiate(itemSlotPrefab, itemContainer);
        
        // スロット内のImageとTextを検索して設定
        Image icon = null;
        Text text = null;
        
        // 子オブジェクトを検索
        Transform iconTransform = newSlot.transform.Find("ItemIcon");
        Transform textTransform = newSlot.transform.Find("ItemText");
        
        if (iconTransform != null)
        {
            icon = iconTransform.GetComponent<Image>();
        }
        
        if (textTransform != null)
        {
            text = textTransform.GetComponent<Text>();
        }
        
        // アイコンを設定
        if (icon != null && itemSprite != null)
        {
            icon.sprite = itemSprite;
            icon.enabled = true;
        }
        else if (icon != null)
        {
            icon.enabled = false;  // スプライトがない場合は非表示
        }
        
        // テキストを設定（★ここが重要★）
        if (text != null)
        {
            text.text = itemName;  // アイテム名を動的に設定
        }
        else
        {
            Debug.LogWarning("ItemTextが見つかりません！");
        }
        
        // 辞書に追加
        itemSlots[itemName] = newSlot;
        
        Debug.Log("インベントリに追加: " + itemName);
    }
    
    // アイテムをUIから削除
    public void RemoveItemFromUI(string itemName)
    {
        if (itemSlots.ContainsKey(itemName))
        {
            Destroy(itemSlots[itemName]);
            itemSlots.Remove(itemName);
            Debug.Log("インベントリから削除: " + itemName);
        }
    }
    
    // 全アイテムを非表示
    public void ClearInventory()
    {
        foreach (var slot in itemSlots.Values)
        {
            Destroy(slot);
        }
        itemSlots.Clear();
    }
}