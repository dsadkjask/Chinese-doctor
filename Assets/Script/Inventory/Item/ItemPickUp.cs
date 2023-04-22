using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
    public class ItemPickUp : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            Item item = other.GetComponent<Item>();

            if (item != null)
            {
                if (item.itemDetails.canPickedup)
                {
                    //拾取物品添加到背包
                    InventoryManager.Instance.AddItem(item, true);//添加该物品并且销毁在地面上的

                    //播放音效
                    //EventHandler.CallPlaySoundEvent(SoundName.Pickup);
                }
            }
        }
    }
}