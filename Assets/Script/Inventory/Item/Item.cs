using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
    public class Item : MonoBehaviour
    {
        public int itemID;//获取ID

        private SpriteRenderer spriteRenderer;//获取SpriteRenderer组件
        private BoxCollider coll;
        public ItemDetails itemDetails;//拿到信息要临时存储

        void Awake()
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            coll = GetComponent<BoxCollider>();
        }

        private void Start()
        {
            if (itemID != 0)
            {
                Init(itemID);//初始化ID
            }
        }

        public void Init(int ID)//初始化ID
        {
            itemID = ID;//让当前ID等于拿到的ID

            //Inventory获得当前物体数据
            itemDetails = InventoryManager.Instance.GetItemDetails(itemID);

            if (itemDetails != null)
            {
                //如果数据不为空则读取它在地图中的那个图片，如果没有在地图上的就直接用图标
                spriteRenderer.sprite = itemDetails.itemOnWorldSprite != null ? itemDetails.itemOnWorldSprite : itemDetails.itemIcon;

                //修改碰撞体尺寸
                Vector3 newSize = new Vector3(spriteRenderer.sprite.bounds.size.x, spriteRenderer.sprite.bounds.size.y,0.2f);
                coll.size = newSize;
                //coll.offset = new Vector3(0, spriteRenderer.sprite.bounds.center.y,0);
            }

            // if (itemDetails.itemType == ItemType.ReapableScenery)
            // {
            //     gameObject.AddComponent<ReapItem>();
            //     gameObject.GetComponent<ReapItem>().InitCropData(itemDetails.itemID);
            //     gameObject.AddComponent<ItemInteractive>();
            // }
        }
    }
}
