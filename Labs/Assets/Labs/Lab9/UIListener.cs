using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIListener : MonoBehaviour
{
    public GameObject ballPrefab;

    private Color red = Color.red;
    private Color blue = Color.blue;
    private Color green = Color.green;
    private Color _selectedColor;

    private string _label;
    //private
    
    // Start is called before the first frame update
    void Start()
    {
        _selectedColor = red;
        _label = "";
    }

    public void setColorRed(bool b)
    {
        if (b)
        {
            _selectedColor = red;
        }

    }

    public void setColorBlue(bool b)
    {
        if (b)
        {
            _selectedColor = blue;
        }
    }
    public void setColorGreen(bool b)
    {
        if (b)
        {
            _selectedColor = green;
        }
    }

    public void printColor()
    {
        Debug.Log("Selected Color: " + _selectedColor);
    }

   
    public void updateLabel(Text label)
    {
        _label = label.text;
    }

    // When button is clicked
    public void SpawnBall()
    {
        GameObject newBall = Instantiate(ballPrefab);
        float randX = Random.Range(-4, 4);
        newBall.transform.position = new Vector3(randX, 4, 0);
        newBall.GetComponent<SpriteRenderer>().color = _selectedColor;
        TextMesh label = newBall.GetComponentInChildren<TextMesh>();
        label.text = _label;
    }
}
