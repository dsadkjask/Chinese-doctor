using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
    public class InventoryManager : Singleton<InventoryManager>
    {
        [Header("物品数据")]
        public ItemDataList_SO itemDataList_SO;
        //[Header("建造蓝图")]
        //public BluePrintDataList_SO bluePrintData;
        [Header("背包数据")]
        //public InventoryBag_SO playerBagTemp;
        public InventoryBag_SO playerBag;
        //private InventoryBag_SO currentBoxBag;

        private void Start()
        {
            // ISaveable saveable = this;
            // saveable.RegisterSaveable();
             EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, playerBag.itemList);
        }

        private void OnEnable()
        {
            EventHandler.DropItemEvent += OnDropItemEvent;
            // EventHandler.HarvestAtPlayerPosition += OnHarvestAtPlayerPosition;
            // //建造
            // EventHandler.BuildFurnitureEvent += OnBuildFurnitureEvent;
            // EventHandler.BaseBagOpenEvent += OnBaseBagOpenEvent;
            // EventHandler.StartNewGameEvent += OnStartNewGameEvent;
        }

        private void OnDisable()
        {
            EventHandler.DropItemEvent -= OnDropItemEvent;
            // EventHandler.HarvestAtPlayerPosition -= OnHarvestAtPlayerPosition;
            // EventHandler.BuildFurnitureEvent -= OnBuildFurnitureEvent;
            // EventHandler.BaseBagOpenEvent -= OnBaseBagOpenEvent;
            // EventHandler.StartNewGameEvent -= OnStartNewGameEvent;
        }

        private void OnDropItemEvent(int ID, Vector3 pos, ItemType itemType)//扔东西事件，每次只扔一个
        {
            RemoveItem(ID, 1);
        }

        /// <summary>
        /// 移除指定数量的背包物品
        /// </summary>
        /// <param name="ID">物品ID</param>
        /// <param name="removeAmount">数量</param>
        private void RemoveItem(int ID, int removeAmount)//移除物品的方法，要移除的ID，个数
        {
            var index = GetItemIndexInBag(ID);//拿到ID

            if (playerBag.itemList[index].itemAmount > removeAmount)//判断数量是否大于要移除的个数
            {
                var amount = playerBag.itemList[index].itemAmount - removeAmount;//等于当前减去要移除的个数
                var item = new InventoryItem { itemID = ID, itemAmount = amount };//更新信息
                playerBag.itemList[index] = item;
            }
            else if (playerBag.itemList[index].itemAmount == removeAmount)//如果等于
            {
                var item = new InventoryItem();
                playerBag.itemList[index] = item;
            }

            EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, playerBag.itemList);//刷新玩家身上的UI
        }
        
        /// 通过ID返回物品信息
        public ItemDetails GetItemDetails(int ID)
        {
            return itemDataList_SO.itemDetailsList.Find(i => i.itemID == ID);
        }

        /// 添加物品到Player背包里
        public void AddItem(Item item, bool toDestory)
        {
            //是否已经有该物品
             var index = GetItemIndexInBag(item.itemID);

             AddItemAtIndex(item.itemID, index, 1);

            Debug.Log(GetItemDetails(item.itemID).itemID + "Name: " + GetItemDetails(item.itemID).itemName);
            if (toDestory)//是否要销毁，就是比如你是捡东西，就要将物品收进来，然后销毁在地面上的
            {
                Destroy(item.gameObject);
            }

            //更新UI，呼叫更新背包ui事件
            EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, playerBag.itemList);
        }

        /// 检查背包是否有空位
        private bool CheckBagCapacity()
        {
            for (int i = 0; i < playerBag.itemList.Count; i++)
            {
                if (playerBag.itemList[i].itemID == 0)
                    return true;
            }
            return false;
        }

        /// 通过物品ID找到背包已有物品位置
        private int GetItemIndexInBag(int ID)
        {
            for (int i = 0; i < playerBag.itemList.Count; i++)
            {
                if (playerBag.itemList[i].itemID == ID)
                    return i;
            }
            return -1;
        }

        /// 在指定背包序号位置添加物品
        private void AddItemAtIndex(int ID, int index, int amount)
        {
            if (index == -1 && CheckBagCapacity())    //背包没有这个物品 同时背包有空位
            {
                var item = new InventoryItem { itemID = ID, itemAmount = amount };
                for (int i = 0; i < playerBag.itemList.Count; i++)
                {
                    if (playerBag.itemList[i].itemID == 0)
                    {
                        playerBag.itemList[i] = item;
                        break;
                    }
                }
            }
            else    //背包有这个物品
            {
                int currentAmount = playerBag.itemList[index].itemAmount + amount;
                var item = new InventoryItem { itemID = ID, itemAmount = currentAmount };

                playerBag.itemList[index] = item;
            }
        }

        // Player背包范围内交换物品，"fromIndex"起始序号，"targetIndex"目标数据序号
        public void SwapItem(int fromIndex, int targetIndex)
        {
            InventoryItem currentItem = playerBag.itemList[fromIndex];//当前的数据
            InventoryItem targetItem = playerBag.itemList[targetIndex];//目标数据

            if (targetItem.itemID != 0)//如果目标的ID不为0
            {
                playerBag.itemList[fromIndex] = targetItem;//将目标物品赋给当前物品的格子
                playerBag.itemList[targetIndex] = currentItem;//将当前目标赋给目标物品的格子
            }
            else//如果目标格子为0，也就是没有物品
            {
                playerBag.itemList[targetIndex] = currentItem;//那就直接将当前目标赋给目标物品的格子
                playerBag.itemList[fromIndex] = new InventoryItem();//然后之前的格子就变成新的空格
            }

            EventHandler.CallUpdateInventoryUI(InventoryLocation.Player, playerBag.itemList);
        }
    }
}
