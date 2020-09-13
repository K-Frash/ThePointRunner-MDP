using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardManager : MonoBehaviour
{
    //Board dimension presents
    public int Rows = 8;
    public int Cols = 8;

    //Used in calculating Tile Dimensions
    [HideInInspector]
    public float TileWidth;
    [HideInInspector]
    public float TileHeight;

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
        TileWidth = parentRect.rect.width / Rows;
        TileHeight = parentRect.rect.height / Cols;

        Debug.Log(string.Format("--> Board Dim; {0} {1}", parentRect.rect.width, parentRect.rect.height));

        for (int row = 0; row < Rows; row++)
        {
            for(int col = 0; col < Cols; col++)
            {
                GameObject tilePrefab = Instantiate(CellPrefab, transform);
                Color32 curColor = ((col-row) % 2 == 0) ? evenColors : oddColors;

                //Position
                RectTransform rectTransform = tilePrefab.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = new Vector2((TileWidth * row) + 50, (TileHeight * col) + 50);

                //Setup Cell
                BoardCells[row, col] = tilePrefab.GetComponent<Cell>();
                BoardCells[row, col].GenerateNewCellSetup(new Vector2Int(row, col), this);
                BoardCells[row, col].GetComponent<Image>().color = curColor;
            }
        }
    }
}
