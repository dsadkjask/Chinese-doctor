using UnityEngine;

[System.Serializable]
public class ItemDetails
{
    public int itemID;//物品ID,命名方式为1001开始

    public string itemName;//物品名

    public ItemType itemType;//物品类型枚举（在Enums中）

    public Sprite itemIcon;//物品在背包之类的图标

    public Sprite itemOnWorldSprite;//物品在游戏中的样子

    public string itemDescription;//物品描述

    public int itemUseRaidus;//到时物品如果是可以种或者建筑就有一个建造的范围

    public bool canPickedup;//是否可以捡起

    public bool canDropped;//是否可以扔

    public bool canCarried;//是否可以拿在手

    public int itemPrice;//物品价值

    [Range(0,1)]
    public float sellPercentage;//就是如果你在商人里买一个价格，然后卖就会减少原价的百分之多少（sellPercentage）
}

[System.Serializable]
public struct InventoryItem
{
    public int itemID;
    public int itemAmount;
}

[System.Serializable]
public class SerializableVector3
{
    public float x, y, z;

    public SerializableVector3(Vector3 pos)//构造
    {
        this.x = pos.x;
        this.y = pos.y;
        this.z = pos.z;
    }

    public Vector3 ToVector3()
    {
        return new Vector3(x, y, z);
    }

    public Vector2Int ToVector2Int()
    {
        return new Vector2Int((int)x, (int)y);
    }
}

[System.Serializable]
public class SceneItem//场景物品
{
    public int itemID;//物品ID
    public SerializableVector3 position;//位置
}

[System.Serializable]
public class TileProperty
{
    public Vector2Int tileCoordinate;//瓦片坐标
    public GridType gridType;//瓦片类型
    public bool boolTypeValue;//判断是否有类型
}

[System.Serializable]
public class TileDetails
{
    public int girdX, gridY;
    public bool canDig;
    public bool canDropItem;
    public bool canPlaceFurniture;
    public bool isNPCObstacle;
    public int daysSinceDug = -1;//已经被挖了多少天.-1代表还没挖，0代表挖了，然后12。。就是天数
    public int daysSinceWatered = -1;//同理浇水的天数
    public int seedItemID = -1;//种子信息，就是种下了这个格就会变成种子的iD
    public int growthDays = -1;//生长的天数
    public int daysSinceLastHarvest = -1;//反复收割的东西离上一次收割的天数
}