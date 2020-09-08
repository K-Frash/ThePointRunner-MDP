using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardManager : MonoBehaviour
{
    //Board dimension presents
    [HideInInspector]
    public int Rows = 8;
    [HideInInspector]
    public int Cols = 8;
    [HideInInspector]
    public float TileSize;

    //Reference to the Cell's prefab
    public GameObject CellPrefab;

    //Array holding cells
    [HideInInspector]
    public Cell[,] BoardCells;

    public void GenerateGrid()
    {
        //Generate Holder's for the board based on provided dimensions
        BoardCells = new Cell[Rows, Cols];

        //Grab Tile references
        Color32 evenColors = new Color32(230, 220, 187, 255);
        Color32 oddColors = new Color32(202, 167, 132, 255);

        //Get Dimensions of board from parent canvas to scale tiles
        RectTransform parentRect = this.GetComponentInParent<RectTransform>();
        float gridW = parentRect.rect.width / Rows;
        float gridH = parentRect.rect.height / Cols;
        TileSize = gridW; //The Grid should always be a square shape

        Debug.Log(string.Format("--> Board Dim; {0} {1}", parentRect.rect.width, parentRect.rect.height));

        for (int row = 0; row < Rows; row++)
        {
            for(int col = 0; col < Cols; col++)
            {
                GameObject tilePrefab = Instantiate(CellPrefab, transform);
                Color32 curColor = ((col-row) % 2 == 0) ? evenColors : oddColors;

                //Position
                RectTransform rectTransform = tilePrefab.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = new Vector2((gridW * row) + 50, (gridH * col) + 50);

                //Setup Cell
                BoardCells[row, col] = tilePrefab.GetComponent<Cell>();
                BoardCells[row, col].Setup(new Vector2Int(row, col), this);
                BoardCells[row, col].GetComponent<Image>().color = curColor;
            }
        }
    }
}
