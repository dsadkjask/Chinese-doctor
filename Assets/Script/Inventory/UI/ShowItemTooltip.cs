using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Inventory
{
    [RequireComponent(typeof(SlotUI))]//确保一定有SlotUI
    public class ShowItemTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private SlotUI slotUI;//获取SlotUI
        private InventoryUI inventoryUI => GetComponentInParent<InventoryUI>();//获取inventoryUI

        private void Awake()
        {
            slotUI = GetComponent<SlotUI>();
        }


        public void OnPointerEnter(PointerEventData eventData)//当鼠标进去
        {
            if (slotUI.itemDetails != null)//如果物品信息不为空
            {
                inventoryUI.itemTooltip.gameObject.SetActive(true);//itemTooltip激活
                inventoryUI.itemTooltip.SetupTooltip(slotUI.itemDetails, slotUI.slotType);//并传入物品信息
                
                //调整位置大小，锚点
                inventoryUI.itemTooltip.GetComponent<RectTransform>().pivot = new Vector2(0, 0);
                inventoryUI.itemTooltip.transform.position = transform.position + Vector3.up * 60;

                // if (slotUI.itemDetails.itemType == ItemType.Furniture)//如果类型为家具
                // {
                //     inventoryUI.itemTooltip.resourcePanel.SetActive(true);
                //     inventoryUI.itemTooltip.SetupResourcePanel(slotUI.itemDetails.itemID);
                // }
                // else
                // {
                //     inventoryUI.itemTooltip.resourcePanel.SetActive(false);
                // }
            }
            else//空格子
            {
                inventoryUI.itemTooltip.gameObject.SetActive(false);//itemTooltip关闭
            }
        }

        public void OnPointerExit(PointerEventData eventData)//划出
        {
            inventoryUI.itemTooltip.gameObject.SetActive(false);//划出，也关闭
        }

    }
}