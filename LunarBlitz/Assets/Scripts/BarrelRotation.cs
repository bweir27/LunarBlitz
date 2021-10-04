using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelRotation : MonoBehaviour
{
    public Transform pivot;
    public Transform barrel;

    public Tower tower;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(tower != null)
        {
            if(tower.currentTarget != null)
            {
                //Debug.Log(tower.currentTarget);
                Vector2 relative = tower.currentTarget.transform.position - pivot.position;

                float angle = Mathf.Atan2(relative.y, relative.x) * Mathf.Rad2Deg;

                Vector3 newRotation = new Vector3(0, 0, angle);

                pivot.localRotation = Quaternion.Euler(newRotation);
            }
        }
    }

    
}
