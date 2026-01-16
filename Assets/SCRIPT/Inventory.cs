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
    
    [Header("拡大演出")]
    public float zoomScale = 2f;
    public float zoomDuration = 0.3f;
    private bool isZooming = false;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            playerMovement = player.GetComponent<PlayerMovement>();

        if (confirmDialog != null)
            confirmDialog.SetActive(false);

        if (confirmYesButton != null)
            confirmYesButton.onClick.AddListener(OnConfirmYes);

        if (confirmNoButton != null)
            confirmNoButton.onClick.AddListener(OnConfirmNo);

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
            return;
        
        if (isZooming)
            return;

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
            ShowConfirmDialog(itemOrder[index]);
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
                playerMovement.enabled = false;

            if (confirmText != null)
                confirmText.text = itemName + " をつかいますか？";
        }
    }

    void OnConfirmYes()
    {
        if (!string.IsNullOrEmpty(pendingItemName))
        {
            StartCoroutine(ZoomAndUseItem(pendingItemName));
        }

        CloseDialog();
    }

    void OnConfirmNo()
    {
        CloseDialog();
    }

    void CloseDialog()
    {
        if (confirmDialog != null)
            confirmDialog.SetActive(false);

        if (playerMovement != null)
            playerMovement.enabled = true;

        pendingItemName = "";
    }

    System.Collections.IEnumerator ZoomAndUseItem(string itemName)
    {
        isZooming = true;

        // アイテムスロットを取得
        GameObject itemSlot = null;
        if (itemSlots.ContainsKey(itemName))
        {
            itemSlot = itemSlots[itemName];
        }

        if (itemSlot != null)
        {
            Vector3 originalScale = itemSlot.transform.localScale;
            Vector3 targetScale = originalScale * zoomScale;
            float elapsed = 0f;

            // 拡大アニメーション
            while (elapsed < zoomDuration / 2f)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / (zoomDuration / 2f);
                itemSlot.transform.localScale = Vector3.Lerp(originalScale, targetScale, t);
                yield return null;
            }

            itemSlot.transform.localScale = targetScale;

            // 少し待機
            yield return new WaitForSeconds(0.1f);

            // 縮小アニメーション
            elapsed = 0f;
            while (elapsed < zoomDuration / 2f)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / (zoomDuration / 2f);
                itemSlot.transform.localScale = Vector3.Lerp(targetScale, originalScale, t);
                yield return null;
            }

            if (itemSlot != null) // アイテムがまだ存在する場合
            {
                itemSlot.transform.localScale = originalScale;
            }
        }

        // アイテム使用処理
        TryUseItem(itemName);

        isZooming = false;
    }

    void TryUseItem(string itemName)
    {
        // まず合成できるかチェック
        if (ItemCombination.Instance != null)
        {
            CombinationRecipe recipe;
            if (ItemCombination.Instance.CanCombine(itemName, out recipe))
            {
                ItemCombination.Instance.CombineItems(itemName);
                return;
            }
        }

        // 次に変換できるかチェック
        if (CheckCanConvert(itemName))
        {
            ExecuteConversion(itemName);
            return;
        }

        // 合成も変換もできない場合は通常のアイテム使用
        if (!CheckCanUseItem(itemName))
        {
            if (MessageDisplay.Instance != null)
                MessageDisplay.Instance.ShowMessage("ここではつかえない");
            return;
        }

        if (GameManager.Instance != null)
            GameManager.Instance.UseItem(itemName);

        RemoveItemFromUI(itemName);
        ExecuteItemEffect(itemName);

        if (MessageDisplay.Instance != null)
            MessageDisplay.Instance.ShowMessage(itemName + " をつかった");
    }

    bool CheckCanUseItem(string itemName)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return false;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.transform.position, 2f);
        foreach (Collider2D col in colliders)
        {
            // BlockWithItem（破壊用）をチェック
            BlockWithItem block = col.GetComponent<BlockWithItem>();
            if (block != null && block.requiredItem == itemName)
                return true;
            
            // BlockTransform（変換用）をチェック
            BlockTransform transform = col.GetComponent<BlockTransform>();
            if (transform != null && transform.requiredItem == itemName)
                return true;
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
            // BlockWithItem（破壊）をチェック
            BlockWithItem block = col.GetComponent<BlockWithItem>();
            if (block != null && block.requiredItem == itemName)
            {
                block.DestroyBlock();
                Debug.Log("ブロックを破壊しました");
                return;
            }
            
            // BlockTransform（変換）をチェック
            BlockTransform transformBlock = col.GetComponent<BlockTransform>();
            if (transformBlock != null && transformBlock.requiredItem == itemName)
            {
                transformBlock.TransformBlock();
                Debug.Log("ブロックを変換しました");
                return;
            }
        }
    }

    bool CheckCanConvert(string itemName)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return false;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.transform.position, 2f);
        foreach (Collider2D col in colliders)
        {
            ItemConverter converter = col.GetComponent<ItemConverter>();
            if (converter != null)
            {
                ConversionRecipe recipe;
                if (converter.CanConvert(itemName, out recipe))
                    return true;
            }
        }
        return false;
    }

    void ExecuteConversion(string itemName)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.transform.position, 2f);
        foreach (Collider2D col in colliders)
        {
            ItemConverter converter = col.GetComponent<ItemConverter>();
            if (converter != null)
            {
                ConversionRecipe recipe;
                if (converter.CanConvert(itemName, out recipe))
                {
                    converter.ConvertItem(itemName);
                    Debug.Log("アイテムを変換しました");
                    return;
                }
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

        Image icon = iconTransform != null ? iconTransform.GetComponent<Image>() : null;
        Text text = textTransform != null ? textTransform.GetComponent<Text>() : null;

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
            text.text = itemNumber.ToString();
            text.color = Color.white;
            text.fontSize = 18;
            text.alignment = TextAnchor.LowerCenter;
            text.enabled = true;

            RectTransform rt = text.GetComponent<RectTransform>();
            rt.localScale = Vector3.one;
        }
        else
        {
            Debug.LogError("ItemTextが見つかりません");
        }

        itemSlots[itemName] = newSlot;
        itemOrder.Add(itemName);
    }

    public void RemoveItemFromUI(string itemName)
    {
        if (itemSlots.ContainsKey(itemName))
        {
            Destroy(itemSlots[itemName]);
            itemSlots.Remove(itemName);
            itemOrder.Remove(itemName);
            UpdateItemNumbers();
        }
    }

    void UpdateItemNumbers()
    {
        for (int i = 0; i < itemOrder.Count; i++)
        {
            string itemName = itemOrder[i];
            if (itemSlots.ContainsKey(itemName))
            {
                Transform textTransform = itemSlots[itemName].transform.Find("ItemText");
                if (textTransform != null)
                {
                    Text text = textTransform.GetComponent<Text>();
                    if (text != null)
                        text.text = (i + 1).ToString();
                }
            }
        }
    }

    public void ClearInventory()
    {
        foreach (var slot in itemSlots.Values)
            Destroy(slot);

        itemSlots.Clear();
        itemOrder.Clear();
    }
}