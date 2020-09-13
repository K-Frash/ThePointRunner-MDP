using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    #region Fields
    public Image OutlineImage;
    public Text RewardDisplay;

    // Entity Graphics holder
    public BaseState Entity;

    //Cell's Position on the Board
    public Vector2Int BoardPosition = Vector2Int.zero;

    //The Board
    public BoardManager Board = null;

    //RectTransform of the Cell object
    public RectTransform CellRectTransform;

    public GameObject Tile;
    #endregion
    
    public void GenerateNewCellSetup(Vector2Int newBoardPosition, BoardManager newBoard){
        BoardPosition = newBoardPosition;
        Board = newBoard;
        CellRectTransform = GetComponent<RectTransform>();
        CellRectTransform.sizeDelta =  new Vector2(Board.TileWidth, Board.TileHeight);
    }

    public void PlacePiece(GameObject entityInit, Type entityType, string reward)
    {
        BaseState newState = null;
        if (entityInit)
        {
            entityInit.transform.parent = this.transform;
            entityInit.transform.position = new Vector2(this.transform.position.x, this.transform.position.y + 20); //slight offset of 10

            //Generate the new Piece
            newState = (BaseState)entityInit.AddComponent(entityType);
        }

        //RewardDisplay will still be provided regardless of the EntityType
        RewardDisplay.text = reward;

        // Set's the entity on the current Cell
        Entity = newState;
    }

    public Vector2 GetTileCenter()
    {
        float tileSize = Board.TileWidth;
        //float tileOffset = Board.TileSize / 2;
        Vector3 cellCenter = Vector2.zero;
        cellCenter.x += (tileSize * BoardPosition.x);
        cellCenter.y += (tileSize * BoardPosition.y);

        //Debug.Log(string.Format("-->Center {0},{1}",cellCenter.x, cellCenter.y));
        return cellCenter;
    }
}
