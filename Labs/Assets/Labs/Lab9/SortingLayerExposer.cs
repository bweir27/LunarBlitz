using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortingLayerExposer : MonoBehaviour
{
    // Source: http://www.valentinourbano.com/unity-text-in-front-of-sprite.html
    public string SortingLayerName = "Default";
    public int SortingOrder = 0;

    private void Awake()
    {
        gameObject.GetComponent<MeshRenderer>().sortingLayerName = SortingLayerName;
        gameObject.GetComponent<MeshRenderer>().sortingOrder = SortingOrder;
    }
}
