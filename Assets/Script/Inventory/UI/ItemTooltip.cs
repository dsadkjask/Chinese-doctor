using Inventory;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemTooltip : MonoBehaviour
{
    //名字
    [SerializeField] private TextMeshProUGUI nameText;
    //类型
    [SerializeField] private TextMeshProUGUI typeText;
    //描述
    [SerializeField] private TextMeshProUGUI descriptionText;
    //价格
    [SerializeField] private Text valueText;
    //用于控制bottom那块的开关
    [SerializeField] private GameObject bottomPart;

     public void SetupTooltip(ItemDetails itemDetails, SlotType slotType)
    {
        nameText.text = itemDetails.itemName;//获得物品的名字并赋值给nameText

        //typeText.text = itemDetails.itemType.ToString();
        //我们要把英文显示改成中文
        typeText.text = GetItemType(itemDetails.itemType);//获得物品的类型并赋值给typeText

        descriptionText.text = itemDetails.itemDescription;//获得物品的描述并赋值给descriptionText

        //如果物品的类型是 种子 或 商品 或 家具
        if (itemDetails.itemType == ItemType.Seed || itemDetails.itemType == ItemType.Commodity || itemDetails.itemType == ItemType.Furniture)
        {
            bottomPart.SetActive(true);

            var price = itemDetails.itemPrice;//价格就等于这个物品的价格
            if (slotType == SlotType.Bag)//但如果这个物品是在包里的话
            {
                price = (int)(price * itemDetails.sellPercentage);//价格就等于是原先价格*百分之多少（就是卖出去的钱会比买回来的少）
            }

            valueText.text = price.ToString();//获得物品的价格并赋值给valueText
        }
        else//如果不是以上类型就不显示价格了
        {
            bottomPart.SetActive(false);
        }
        //强制立即显示文字，解决延迟问题
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
    }

    private string GetItemType(ItemType itemType)
    {
        return itemType switch
        {
            ItemType.Seed => "种子",
            ItemType.Commodity => "商品",
            ItemType.Furniture => "家具",
            ItemType.BreakTool => "工具",
            ItemType.ChopTool => "工具",
            ItemType.CollectTool => "工具",
            ItemType.HoeTool => "工具",
            ItemType.ReapTool => "工具",
            ItemType.WaterTool => "工具",
            _ => "无"
        };
    }
}
