using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardManager : MonoBehaviour
{
    public int Rows = 8;

    public int Cols = 8;

    public float TileSize = 1;

    [HideInInspector]
    public Cell[,] BoardCells = new Cell[8, 8];

    void Start()
    {
        GenerateGrid();
    }

    public void GenerateGrid()
    {
        //Grab Tile references
        GameObject tile1Prefab = (GameObject)Instantiate(Resources.Load("floor_1"));
        GameObject tile2Prefab = (GameObject)Instantiate(Resources.Load("floor_2"));
        float gridW = Cols * TileSize;
        float gridH = Rows * TileSize;

        for (int row = 0; row < Rows; row++)
        {
            for(int col = 0; col < Cols; col++)
            {
                float posX = col * TileSize;
                float posY = row * -TileSize;
                GameObject refTile = ((col-row) % 2 == 0) ? tile1Prefab : tile2Prefab;
                GameObject tile = Instantiate(refTile, transform);
                tile.transform.position = new Vector2(posX, posY);

                BoardCells[row, col] = tile.GetComponent<Cell>();
                BoardCells[row, col].Setup(new Vector2Int(row, col), this, tile);
            }
        }

        Destroy(tile1Prefab);
        Destroy(tile2Prefab);

        //Center the Board on the Main Camera
        //transform.position = new Vector2(-(gridW/2) + TileSize/2, gridH/2 - TileSize/2);
    }
}
