using UnityEngine;

[System.Serializable]
public class ConversionRecipe
{
    public string inputItem;      // 使用するアイテム
    public string outputItem;     // 生成されるアイテム
    public Sprite outputSprite;   // 生成されるアイテムのスプライト
}

public class ItemConverter : MonoBehaviour
{
    [Header("変換レシピ")]
    public ConversionRecipe[] recipes;

    [Header("検知範囲")]
    public float detectionRange = 2f;

    [Header("一意のID")]
    public string converterID;

    [Header("使用制限")]
    public bool singleUseOnly = false;  // trueの場合1回しか使えない

    private Transform player;
    private bool isNearby = false;
    private bool hasBeenUsed = false;

    void OnValidate()
    {
        if (string.IsNullOrEmpty(converterID))
        {
            converterID = gameObject.name + "_" +
                          transform.position.x.ToString("F2") + "_" +
                          transform.position.y.ToString("F2");
        }
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        // 1回限りの場合、使用済みかチェック
        if (singleUseOnly && GameManager.Instance != null)
        {
            hasBeenUsed = GameManager.Instance.IsTransformed(converterID);
        }
    }

    void Update()
    {
        if (player == null || (singleUseOnly && hasBeenUsed)) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= detectionRange && !isNearby)
        {
            isNearby = true;
            Debug.Log("変換装置の近くにいます");
        }
        else if (distance > detectionRange && isNearby)
        {
            isNearby = false;
        }
    }

    // 指定したアイテムで変換可能かチェック
    public bool CanConvert(string itemName, out ConversionRecipe matchedRecipe)
    {
        matchedRecipe = null;

        // 1回限りで使用済みの場合は変換不可
        if (singleUseOnly && hasBeenUsed)
            return false;

        foreach (var recipe in recipes)
        {
            if (recipe.inputItem == itemName)
            {
                matchedRecipe = recipe;
                return true;
            }
        }

        return false;
    }

    // 変換を実行
    public void ConvertItem(string usedItemName)
    {
        // 1回限りで使用済みの場合は処理しない
        if (singleUseOnly && hasBeenUsed)
        {
            if (MessageDisplay.Instance != null)
                MessageDisplay.Instance.ShowMessage("もうつかえない");
            return;
        }

        ConversionRecipe recipe;
        if (!CanConvert(usedItemName, out recipe))
        {
            Debug.Log("変換できません");
            return;
        }

        // 使用したアイテムを削除
        if (GameManager.Instance != null)
        {
            GameManager.Instance.UseItem(recipe.inputItem);
        }

        if (InventoryUI.Instance != null)
        {
            InventoryUI.Instance.RemoveItemFromUI(recipe.inputItem);
        }

        // 新しいアイテムを追加
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddItem(recipe.outputItem, recipe.outputSprite);
        }

        if (InventoryUI.Instance != null)
        {
            InventoryUI.Instance.AddItemToUI(recipe.outputItem, recipe.outputSprite);
        }

        // メッセージ表示
        if (MessageDisplay.Instance != null)
        {
            MessageDisplay.Instance.ShowMessage(recipe.outputItem + " にかわった！");
        }

        Debug.Log($"{recipe.inputItem} → {recipe.outputItem}");

        // 1回限りの場合、使用済みとしてマーク
        if (singleUseOnly)
        {
            hasBeenUsed = true;
            if (GameManager.Instance != null)
            {
                GameManager.Instance.MarkAsTransformed(converterID);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}