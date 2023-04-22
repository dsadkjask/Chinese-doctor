using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Horse : MonoBehaviour
{
     public GameObject player;
     public GameObject sitPoint;//假人
     public Animator anim;
     public bool isWalk;
    //public Transform playertransform; // 角色的Transform组件
    //public float mountOffset = 1.0f; // 坐骑距离角色的偏移量
    public bool isHorse;
    //private Vector3 targetPos; // 坐骑的目标位置

    void start()
    {
        anim = gameObject.GetComponent<Animator>();
        isHorse = false;
    }
    void Update()
    {
        //UpdateAnim();
        if(isHorse)
        {
            transform.SetParent(player.transform);
        }
        else if(!isHorse)
        {
            transform.SetParent(null);
        }
        // if(Input.GetKeyDown(KeyCode.F))
        //     {
        //         isHorse = true;
        //         print("ok");
        //     }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.Space))
        {
                // player.SetActive(false);
                // fake.SetActive(true);
                player.transform.position = sitPoint.transform.position;
                isHorse = true;
                print("ok");
        }
        
    }

    // void UpdateAnim()
    // {
    //     anim.SetBool("isWalk",isWalk);
    // }

    // void OnTriggerExit2D(Collider2D other)
    // {
    //     if (other.CompareTag("Player"))
    //     {
    //         player.SetParent(null); // 将角色从坐骑的子对象中移除
    //     }
    // }

    
}
