using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardsManager : MonoBehaviour
{
    [SerializeField] GridLayoutGroup gridRoot;
    [SerializeField] GameObject card;

    [SerializeField] int rows;
    [SerializeField] int columns;
    // Start is called before the first frame update
    void Start()
    {
        RectTransform gridRectTransform = gridRoot.transform as RectTransform;
        if(rows > columns) 
        {
            gridRoot.cellSize = new Vector3(gridRectTransform.rect.width / columns, gridRectTransform.rect.height / rows, 0);
            gridRoot.constraintCount = columns;
        }
        else
        {
            gridRoot.cellSize = new Vector3(gridRectTransform.rect.width / rows, gridRectTransform.rect.height / columns, 0);
            gridRoot.constraintCount = rows;
        }


        for (int i = 0; i < rows * columns; i++)
        {
            GameObject cardObjects = Instantiate(card, gridRoot.transform, false);
            cardObjects.GetComponent<Image>().color = Random.ColorHSV();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
