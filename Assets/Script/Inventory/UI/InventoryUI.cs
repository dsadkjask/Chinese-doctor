using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory
{
    public class InventoryUI : MonoBehaviour
    {
         public ItemTooltip itemTooltip;//获得物品提示脚本

         [Header("拖拽图片")]
         public Image dragItem;

         [Header("玩家背包UI")]
         [SerializeField] private GameObject bagUI;//[SerializeField] private 跟 public的区别就是私有然后可以在窗口中调用但是别的代码中调用不了它，public就是公开
         private bool bagOpened;//判断背包是否被打开

        // [Header("通用背包")]
        // [SerializeField] private GameObject baseBag;
        // public GameObject shopSlotPrefab;
        // public GameObject boxSlotPrefab;

        // [Header("交易UI")]
        // public TradeUI tradeUI;
        // public TextMeshProUGUI playerMoneyText;

        [SerializeField] private SlotUI[] playerSlots;
        // [SerializeField] private List<SlotUI> baseBagSlots;

        private void OnEnable()
        {
            EventHandler.UpdateInventoryUI += OnUpdateInventoryUI;//注册背包事件
            // EventHandler.BeforeSceneUnloadEvent += OnBeforeSceneUnloadedEvent;
            // EventHandler.BaseBagOpenEvent += OnBaseBagOpenEvent;
            // EventHandler.BaseBagCloseEvent += OnBaseBagCloseEvent;
            // EventHandler.ShowTradeUI += OnShowTradeUI;
        }

        private void OnDisable()
        {
            EventHandler.UpdateInventoryUI -= OnUpdateInventoryUI;//关闭背包事件
            // EventHandler.BeforeSceneUnloadEvent -= OnBeforeSceneUnloadedEvent;
            // EventHandler.BaseBagOpenEvent -= OnBaseBagOpenEvent;
            // EventHandler.BaseBagCloseEvent -= OnBaseBagCloseEvent;
            // EventHandler.ShowTradeUI -= OnShowTradeUI;
        }


        private void Start()
        {
            //给每一个格子序号
            for (int i = 0; i < playerSlots.Length; i++)//循环格子长度
            {
                playerSlots[i].slotIndex = i;//然后赋值给slotIndex格子的序列
            }
            bagOpened = bagUI.activeInHierarchy;//然后判断背包是否开启就是直接判断bagUI，是否在Hierarchy窗口中激活
            //playerMoneyText.text = InventoryManager.Instance.playerMoney.ToString();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                OpenBagUI();
            }
        }

        // private void OnShowTradeUI(ItemDetails item, bool isSell)
        // {
        //     tradeUI.gameObject.SetActive(true);
        //     tradeUI.SetupTradeUI(item, isSell);
        // }

        // /// <summary>
        // /// 打开通用包裹UI事件
        // /// </summary>
        // /// <param name="slotType"></param>
        // /// <param name="bagData"></param>
        // private void OnBaseBagOpenEvent(SlotType slotType, InventoryBag_SO bagData)
        // {
        //     GameObject prefab = slotType switch
        //     {
        //         SlotType.Shop => shopSlotPrefab,
        //         SlotType.Box => boxSlotPrefab,
        //         _ => null,
        //     };

        //     //生成背包UI
        //     baseBag.SetActive(true);

        //     baseBagSlots = new List<SlotUI>();

        //     for (int i = 0; i < bagData.itemList.Count; i++)
        //     {
        //         var slot = Instantiate(prefab, baseBag.transform.GetChild(0)).GetComponent<SlotUI>();
        //         slot.slotIndex = i;
        //         baseBagSlots.Add(slot);
        //     }
        //     LayoutRebuilder.ForceRebuildLayoutImmediate(baseBag.GetComponent<RectTransform>());

        //     if (slotType == SlotType.Shop)
        //     {
        //         bagUI.GetComponent<RectTransform>().pivot = new Vector2(-1, 0.5f);
        //         bagUI.SetActive(true);
        //         bagOpened = true;
        //     }
        //     //更新UI显示
        //     OnUpdateInventoryUI(InventoryLocation.Box, bagData.itemList);
        // }

        // /// <summary>
        // /// 关闭通用包裹UI事件
        // /// </summary>
        // /// <param name="slotType"></param>
        // /// <param name="bagData"></param>
        // private void OnBaseBagCloseEvent(SlotType slotType, InventoryBag_SO bagData)
        // {
        //     baseBag.SetActive(false);
        //     itemTooltip.gameObject.SetActive(false);
        //     UpdateSlotHightlight(-1);

        //     foreach (var slot in baseBagSlots)
        //     {
        //         Destroy(slot.gameObject);
        //     }
        //     baseBagSlots.Clear();

        //     if (slotType == SlotType.Shop)
        //     {
        //         bagUI.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
        //         bagUI.SetActive(false);
        //         bagOpened = false;
        //     }
        // }


        // private void OnBeforeSceneUnloadedEvent()
        // {
        //     UpdateSlotHightlight(-1);
        // }


        // 更新指定位置的Slot事件函数，location：库存位置，list：数据列表
        private void OnUpdateInventoryUI(InventoryLocation location, List<InventoryItem> list)
        {
            switch (location)
            {
                //如果库存位置是player时
                case InventoryLocation.Player:
                    for (int i = 0; i < playerSlots.Length; i++)//循环到每一个格
                    {
                        if (list[i].itemAmount > 0)//判断当前列表中的第i个物品数量是否大于0，大于则代表有，则
                        {
                            var item = InventoryManager.Instance.GetItemDetails(list[i].itemID);//则获取这个列表id物体的详情
                            playerSlots[i].UpdateSlot(item, list[i].itemAmount);//然后就更新我们背包里的样子，传入这个物品的信息和列表中的数量
                        }
                        else//小于0则
                        {
                            playerSlots[i].UpdateEmptySlot();//当前的格子为空
                        }
                    }
                    break;
                // //如果库存位置是Box时
                // case InventoryLocation.Box:
                //     for (int i = 0; i < baseBagSlots.Count; i++)
                //     {
                //         if (list[i].itemAmount > 0)
                //         {
                //             var item = InventoryManager.Instance.GetItemDetails(list[i].itemID);
                //             baseBagSlots[i].UpdateSlot(item, list[i].itemAmount);
                //         }
                //         else
                //         {
                //             baseBagSlots[i].UpdateEmptySlot();
                //         }
                //     }
                //     break;
            }

            //playerMoneyText.text = InventoryManager.Instance.playerMoney.ToString();
        }

        // 打开关闭背包UI，Button调用事件
        public void OpenBagUI()
        {
            bagOpened = !bagOpened;

            bagUI.SetActive(bagOpened);
        }


        /// 更新Slot高亮显示
        public void UpdateSlotHightlight(int index)
        {
            foreach (var slot in playerSlots)//循环格子
            {
                if (slot.isSelected && slot.slotIndex == index)//如果格子被选中且传进来的编号等于格子的编号，则
                {
                    slot.slotHightlight.gameObject.SetActive(true);//将这个格子的高光打开
                }
                else
                {
                    slot.isSelected = false;//否则就取消选中
                    slot.slotHightlight.gameObject.SetActive(false);//取消高光
                }
            }
        }
    }
}