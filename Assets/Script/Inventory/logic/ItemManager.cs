using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Inventory
{
public class ItemManager : MonoBehaviour
{
    public Item itemPrefab;//预制体
    public Item bounceItemPrefab;

    private Transform itemParent;//目的是保存整洁，在Start（）获得

    //记录场景Item
    private Dictionary<string, List<SceneItem>> sceneItemDict = new Dictionary<string, List<SceneItem>>();//Dictionary<场景名，场景中物品的列表>

    private void OnEnable()
        {
            EventHandler.InstantiateItemInScene += OnInstantiateItemInScene;//注册这个方法
            EventHandler.DropItemEvent += OnDropItemEvent;

             EventHandler.BeforeSceneUnloadEvent += OnBeforeSceneUnloadEvent;
             EventHandler.AfterSceneLoadedEvent += OnAfterSceneLoadedEvent;

            // //建造
            // EventHandler.BuildFurnitureEvent += OnBuildFurnitureEvent;
            // EventHandler.StartNewGameEvent += OnStartNewGameEvent;
        }

        private void OnDisable()
        {
            EventHandler.InstantiateItemInScene -= OnInstantiateItemInScene;//关闭这个方法
            EventHandler.DropItemEvent -= OnDropItemEvent;
             EventHandler.BeforeSceneUnloadEvent -= OnBeforeSceneUnloadEvent;
             EventHandler.AfterSceneLoadedEvent -= OnAfterSceneLoadedEvent;
            // EventHandler.BuildFurnitureEvent -= OnBuildFurnitureEvent;
            // EventHandler.StartNewGameEvent -= OnStartNewGameEvent;
        }

        private void Start()
        {
            //itemParent = GameObject.FindWithTag("ItemParent").transform;
        }

        private void OnBeforeSceneUnloadEvent()
        {
            GetAllSceneItems();
            //GetAllSceneFurniture();
        }

        private void OnAfterSceneLoadedEvent()
        {
            itemParent = GameObject.FindWithTag("ItemParent").transform;
            RecreateAllItems();
            //RebuildFurniture();
        }
        //在指定坐标位置生成物品，"ID"物品ID，"pos"世界坐标
        private void OnInstantiateItemInScene(int ID, Vector3 pos)
        {
            var item = Instantiate(bounceItemPrefab, pos, Quaternion.identity, itemParent);
            item.itemID = ID;
            //item.GetComponent<ItemBounce>().InitBounceItem(pos, Vector3.up);
        }

        private void OnDropItemEvent(int ID, Vector3 mousePos, ItemType itemType)
        {
            // if (itemType == ItemType.Seed) return;

            // var item = Instantiate(bounceItemPrefab, PlayerTransform.position, Quaternion.identity, itemParent);
            // item.itemID = ID;
            // var dir = (mousePos - PlayerTransform.position).normalized;
            // item.GetComponent<ItemBounce>().InitBounceItem(mousePos, dir);

            var item = Instantiate(bounceItemPrefab, mousePos, Quaternion.identity, itemParent);
            item.itemID = ID;
        }

        // 获得当前场景所有Item
        private void GetAllSceneItems()
        {
            List<SceneItem> currentSceneItems = new List<SceneItem>();

            foreach (var item in FindObjectsOfType<Item>())
            {
                SceneItem sceneItem = new SceneItem
                {
                    itemID = item.itemID,
                    position = new SerializableVector3(item.transform.position)
                };

                currentSceneItems.Add(sceneItem);
            }

            if (sceneItemDict.ContainsKey(SceneManager.GetActiveScene().name))
            {
                //找到数据就更新item数据列表
                sceneItemDict[SceneManager.GetActiveScene().name] = currentSceneItems;
            }
            else    //如果是新场景
            {
                sceneItemDict.Add(SceneManager.GetActiveScene().name, currentSceneItems);
            }
        }

        // 刷新重建当前场景物品.全部删掉，又重新生成
        private void RecreateAllItems()
        {
            List<SceneItem> currentSceneItems = new List<SceneItem>();

            if (sceneItemDict.TryGetValue(SceneManager.GetActiveScene().name, out currentSceneItems))
            {
                if (currentSceneItems != null)
                {
                   // 清场
                    foreach (var item in FindObjectsOfType<Item>())
                    {
                        Destroy(item.gameObject);
                    }

                    foreach (var item in currentSceneItems)
                    {
                        Item newItem = Instantiate(itemPrefab, item.position.ToVector3(), Quaternion.identity, itemParent);
                        newItem.Init(item.itemID);
                    }
                }
            }
        }
}
}
