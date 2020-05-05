using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Peixi
{
    public class Utility : MonoBehaviour
    {
        /// <summary>
        /// 将m_transform下所有子物体状态设置为active
        /// </summary>
        /// <param name="m_transform"></param>
        /// <param name="active"></param>
        public static void AcitveAllChildren(Transform m_transform,bool active = true)
        {
            foreach (Transform child in m_transform)
            {
                child.gameObject.SetActive(active);
            }
        }
    }
}

