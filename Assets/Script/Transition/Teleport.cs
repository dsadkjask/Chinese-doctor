using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Transition
{
    public class Teleport : MonoBehaviour
    {
        //[SceneName]
        public string sceneToGo;//要去的场景
        public Vector3 positionToGo;//要去的坐标

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                EventHandler.CallTransitionEvent(sceneToGo, positionToGo);//调用事件
            }
        }
    }
}