using UnityEngine;

[System.Serializable]
public class CombinationRecipe
{
    public string item1;
    public string item2;
    public string resultItem;
    public Sprite resultSprite;
}

public class ItemCombination : MonoBehaviour
{
    public static ItemCombination Instance;

    [Header("合成レシピ")]
    public CombinationRecipe[] recipes;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    // 指定したアイテムで合成可能かチェック
    public bool CanCombine(string itemName, out CombinationRecipe matchedRecipe)
    {
        matchedRecipe = null;

        if (GameManager.Instance == null) return false;

        foreach (var recipe in recipes)
        {
            // item1がitemNameと一致し、item2を持っているかチェック
            if (recipe.item1 == itemName && GameManager.Instance.HasItem(recipe.item2))
            {
                matchedRecipe = recipe;
                return true;
            }
            // item2がitemNameと一致し、item1を持っているかチェック
            if (recipe.item2 == itemName && GameManager.Instance.HasItem(recipe.item1))
            {
                matchedRecipe = recipe;
                return true;
            }
        }

        return false;
    }

    // 合成を実行
    public void CombineItems(string usedItemName)
    {
        CombinationRecipe recipe;
        if (!CanCombine(usedItemName, out recipe))
        {
            Debug.Log("合成できません");
            return;
        }

        // 両方のアイテムを削除
        if (GameManager.Instance != null)
        {
            GameManager.Instance.UseItem(recipe.item1);
            GameManager.Instance.UseItem(recipe.item2);
        }

        // UIから削除
        if (InventoryUI.Instance != null)
        {
            InventoryUI.Instance.RemoveItemFromUI(recipe.item1);
            InventoryUI.Instance.RemoveItemFromUI(recipe.item2);
        }

        // 新しいアイテムを追加
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddItem(recipe.resultItem, recipe.resultSprite);
        }

        if (InventoryUI.Instance != null)
        {
            InventoryUI.Instance.AddItemToUI(recipe.resultItem, recipe.resultSprite);
        }

        // メッセージ表示
        if (MessageDisplay.Instance != null)
        {
            MessageDisplay.Instance.ShowMessage(recipe.resultItem + " をつくった");
        }

        Debug.Log($"{recipe.item1} + {recipe.item2} = {recipe.resultItem}");
    }
}