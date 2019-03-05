using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridNumbering : MonoBehaviour
{
    [SerializeField] private GameObject gridCharPrefab;

    void Start()
    {
        //Generate numbers top and bottom of the grid starting from left
        for (int i = 0; i < GameManager.instance.MapHandler.MapWidth; i++)
        {
            GameObject gridCharTop = Instantiate(gridCharPrefab, transform);
            gridCharTop.transform.position = new Vector3(0.5f + i, 0.5f + GameManager.instance.MapHandler.MapHeight, 0);
            gridCharTop.GetComponent<TextMesh>().text = "" + i;

            GameObject gridCharBottom = Instantiate(gridCharPrefab, transform);
            gridCharBottom.transform.position = new Vector3(0.5f + i, -0.5f, 0);
            gridCharBottom.GetComponent<TextMesh>().text = "" + i;
        }

        //Generate letters left and right of the grid starting from top
        for (int i = 0; i < GameManager.instance.MapHandler.MapWidth; i++)
        {
            GameObject gridCharLeft = Instantiate(gridCharPrefab, transform);
            gridCharLeft.transform.position = new Vector3(-0.5f, 0.5f + i, 0);
            gridCharLeft.GetComponent<TextMesh>().text = "" + (char)('a' + GameManager.instance.MapHandler.MapHeight - i - 1);

            GameObject gridCharRight = Instantiate(gridCharPrefab, transform);
            gridCharRight.transform.position = new Vector3(0.5f + GameManager.instance.MapHandler.MapWidth, 0.5f + i, 0);
            gridCharRight.GetComponent<TextMesh>().text = "" + (char)('a' + GameManager.instance.MapHandler.MapHeight - i - 1);
        }
    }
}
