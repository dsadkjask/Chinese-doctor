using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Inventory
{
    public class SlotUI : MonoBehaviour ,IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [Header("组件获取")]
        [SerializeField] private Image slotImage;//获取格子图片，[SerializeField]的意思是为了这里我们不用Awake去获取，而是在Inspector里提前获取
        [SerializeField] private TextMeshProUGUI amountText;//显示数量
        public Image slotHightlight;//高亮显示
        [SerializeField] private Button button;//获得按钮，因为我们不想在没有物品的情况下，格子还能被按下

        [Header("格子类型")]
        public SlotType slotType;//用于判断当前格子的类型
        public bool isSelected;//判断是否被选中
        public int slotIndex;//格子的序号

         //物品信息
         public ItemDetails itemDetails;//获取物品信息
         public int itemAmount;//物体的数量

        // public InventoryLocation Location
        // {
        //     get
        //     {
        //         return slotType switch
        //         {
        //             SlotType.Bag => InventoryLocation.Player,
        //             SlotType.Box => InventoryLocation.Box,
        //             _ => InventoryLocation.Player
        //         };
        //     }
        // }

        public InventoryUI inventoryUI => GetComponentInParent<InventoryUI>();

        private void Start()
        {
            isSelected = false;//让一开始没有东西被选中

            if (itemDetails.itemID == 0)//让游戏一开始将所有的没有物品的格子都调用UpdateEmptySlot()函数,这里不能用等于null，会报错
            {
                UpdateEmptySlot();
            }
        }

        // 更新格子UI和信息
        public void UpdateSlot(ItemDetails item, int amount)
        {
            itemDetails = item;//获取物品信息
            slotImage.sprite = item.itemIcon;//让格子的图片等于物品的icon图标
            itemAmount = amount;//让格子的数量等于物品的数量
            amountText.text = amount.ToString();//更新物品数量显示
            slotImage.enabled = true;//打开格子图片的显示
            button.interactable = true;//打开格子点击功能
        }

        // 将Slot更新为空，（当格子中没有东西了就应该调用这个函数）
        public void UpdateEmptySlot()
        {
            if (isSelected)//当前如果被选中
            {
                isSelected = false;//就不能让它被选中

                //inventoryUI.UpdateSlotHightlight(-1);
                //EventHandler.CallItemSelectedEvent(itemDetails, isSelected);
            }
            itemDetails = null;//
            slotImage.enabled = false;//将图片关掉
            amountText.text = string.Empty;//文本也清空
            button.interactable = false;//设置按钮也不能被点按
        }
        
        //点击功能，显示高亮选中
        public void OnPointerClick(PointerEventData eventData)
        {
            if (itemDetails == null) return;//如果没东西就返回不执行
            isSelected = !isSelected;//选中变不选中，不选中变选中

            inventoryUI.UpdateSlotHightlight(slotIndex);//在点中后调用UpdateSlotHightlight

            // if (slotType == SlotType.Bag)//如果点击的类型是背包的，还可以显示详情
            // {
            //     //通知物品被选中的状态和信息
            //     EventHandler.CallItemSelectedEvent(itemDetails, isSelected);
            // }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (itemAmount != 0)//点击拖拽时如果数量不为0
            {
                inventoryUI.dragItem.enabled = true;//拖拽图片就会启用
                inventoryUI.dragItem.sprite = slotImage.sprite;//拖拽图片也会换成格子里的那个图片
                inventoryUI.dragItem.SetNativeSize();//设置为图片原来的尺寸（其实可有可无）

                isSelected = true;//同时格子是被选中状态
                inventoryUI.UpdateSlotHightlight(slotIndex); //并且会更新高光
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            //在拖拽的过程中，我们要让这个图片的位置一直是等于我们鼠标的位置的
            inventoryUI.dragItem.transform.position = Input.mousePosition;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
             inventoryUI.dragItem.enabled = false;//结束的时候要将这个图片关闭
             Debug.Log(eventData.pointerCurrentRaycast.gameObject);//这里用于测试，打印出当前射线所碰到的物体是什么

            if (eventData.pointerCurrentRaycast.gameObject != null)//判断射线检测到的是否是null，如果不为空则
            {
                if (eventData.pointerCurrentRaycast.gameObject.GetComponent<SlotUI>() == null)//判断检测到的这个物体是否带有SlotUI代码组件，如果为空，则返回
                    return;

                var targetSlot = eventData.pointerCurrentRaycast.gameObject.GetComponent<SlotUI>();//如果不为空则让目标格子等于这个当前拖拽的组件
                int targetIndex = targetSlot.slotIndex;//当前拖拽格子的索引就等于目标格子索引

                //在Player自身背包范围内交换
                if (slotType == SlotType.Bag && targetSlot.slotType == SlotType.Bag)//如果 被拖拽格子的类型 和 拖拽过去的目标的格子 类型都为背包
                {
                     InventoryManager.Instance.SwapItem(slotIndex, targetIndex);
                }
            //     else if (slotType == SlotType.Shop && targetSlot.slotType == SlotType.Bag)  //买，如果被拖拽格子类型是商店的，然后目标格是自己的背包的
            //     {
            //         EventHandler.CallShowTradeUI(itemDetails, false);
            //     }
            //     else if (slotType == SlotType.Bag && targetSlot.slotType == SlotType.Shop)  //卖，如果被拖拽格子是自己背包的，然后目标格是商店的
            //     {
            //         EventHandler.CallShowTradeUI(itemDetails, true);
            //     }
            //     else if (slotType != SlotType.Shop && targetSlot.slotType != SlotType.Shop && slotType != targetSlot.slotType)//如果被拖拽格子的类型不是商店，且目标格子不是
            //     {
            //         //跨背包数据交换物品
            //         InventoryManager.Instance.SwapItem(Location, slotIndex, targetSlot.Location, targetSlot.slotIndex);
            //     }
                 //清空所有高亮显示
                 inventoryUI.UpdateSlotHightlight(-1);
            // }
            // else    //测试扔在地上
            // {
            //     if (itemDetails.canDropped)
            //     {
            //         //鼠标对应世界地图坐标
            //         var pos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));

            //         EventHandler.CallInstantiateItemInScene(itemDetails.itemID, pos);
            //     }
              }
        }
    }
}