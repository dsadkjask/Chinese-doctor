//用于储存所有枚举型的东西
public enum ItemType
{
    //种子，日用品，家具
    Seed,Commodity,Furniture,
    //锄头,砍树，砸石头，割草，浇水，收集
    HoeTool,ChopTool,BreakTool,ReapTool,WaterTool,CollectTool,
    //可以被割的杂草
    ReapableSencery
}

//格子的类型
public enum SlotType
{
    //背包，箱子，商店
    Bag, Box, Shop
}

//存储容器
public enum InventoryLocation
{
    //玩家的，箱子的
    Player, Box
}

//季节
public enum Season
{
    春天, 夏天, 秋天, 冬天
}

public enum GridType//瓦片类型
{
    //可以挖，可以扔东西，可以添加家具，npc障碍物
    Diggable, DropItem, PlaceFurniture, NPCObstacle
}