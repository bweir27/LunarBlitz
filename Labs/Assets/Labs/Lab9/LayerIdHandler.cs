using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LayerIdHandler : MonoBehaviour
{
    // Source: http://www.valentinourbano.com/unity-text-in-front-of-sprite.html
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<Renderer>().sortingLayerID = this.transform.parent.GetComponent<Renderer>().sortingLayerID;
    }
}
