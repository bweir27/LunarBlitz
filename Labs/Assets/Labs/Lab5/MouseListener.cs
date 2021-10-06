using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseListener : MonoBehaviour
{
    public Camera camera;
    public Color hoverColor;
    private Vector2 mousePosInWorldCoords;
    private bool IsColored = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        bool click = Input.GetButtonDown("Fire1");
        Debug.Log(click);
        if (click)
        {
            // get the mouse coordinates (which are in screen coords)
            // and convert them to world coordinates
            mousePosInWorldCoords = camera.ScreenToWorldPoint(Input.mousePosition);

            // get a ray from the mouse coordinates
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);

            //do a raycast into the scene
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            if (hit != null && hit.collider != null)
            {
                //IsColored = !IsColored;
                Debug.Log(hit);
                Debug.Log(gameObject.name);
                if (hit.collider.gameObject.name == gameObject.name)
                {
                    toggleColor();
                }
                //else
                //{
                //    GetComponent<SpriteRenderer>().color = Color.white;
                //}
            }
        }
    }

    void toggleColor()
    {
        if (IsColored)
        {
            GetComponent<SpriteRenderer>().color = Color.white;
        }
        else
        {
            GetComponent<SpriteRenderer>().color = hoverColor;
        }
        IsColored = !IsColored;
    }
}
