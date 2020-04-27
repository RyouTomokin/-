using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Peixi
{
    public class Utility : MonoBehaviour
    {
        public static void AcitveAllChildren(Transform m_transform,bool active = true)
        {
            foreach (Transform child in m_transform)
            {
                child.gameObject.SetActive(active);
            }
        }
    }
}

