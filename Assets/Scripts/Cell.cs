using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    #region Fields
    public BaseState ContentObject;
    public Vector2Int BoardPosition = Vector2Int.zero;
    public BoardManager Board = null;
    public GameObject Tile;
    #endregion
    
    public void Setup(Vector2Int newBoardPosition, BoardManager newBoard, GameObject tile){
        BoardPosition = newBoardPosition;
        Board = newBoard;
        Tile = tile;
    }

    public void SetTileEntity(BaseState entity)
    {
        ContentObject = entity;
    }

    public Vector2 GetTileCenter()
    {
        float tileSize = Board.TileSize;
        //float tileOffset = Board.TileSize / 2;
        Vector3 cellCenter = Vector2.zero;
        cellCenter.x += (tileSize * BoardPosition.x);
        cellCenter.y += -(tileSize * BoardPosition.y);

        Debug.Log(string.Format("-->Center {0},{1}",cellCenter.x, cellCenter.y));
        return cellCenter;
    }
}
