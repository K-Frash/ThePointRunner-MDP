using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    #region Fields
    public Image OutlineImage;
    public Text RewardDisplay;

    //public Image EntityImage;
    public GameObject Entity;

    //Reward for current Cell state
    public int Reward = 0;

    //Cell's Position on the Board
    public Vector2Int BoardPosition = Vector2Int.zero;

    //The Board
    public BoardManager Board = null;

    //RectTransform of the Cell object
    public RectTransform CellRectTransform;

    public GameObject Tile;
    #endregion
    
    public void Setup(Vector2Int newBoardPosition, BoardManager newBoard){
        BoardPosition = newBoardPosition;
        Board = newBoard;
        CellRectTransform = GetComponent<RectTransform>();
        RewardDisplay.text = Reward.ToString();

        //CellRectTransform.ForceUpdateRectTransforms();
        CellRectTransform.sizeDelta =  new Vector2(Board.TileWidth, Board.TileHeight);

        /*
        //Temp: Example of injecting component data for the cell!!!
        GameObject resourceTemp = (GameObject)Instantiate(Resources.Load("agent_idle"));
        resourceTemp.transform.parent = this.transform;
        resourceTemp.transform.position = new Vector2(this.transform.position.x, this.transform.position.y+10);
        //EntityImage.sprite = resourceTemp.GetComponent<SpriteRenderer>().sprite;
        Entity = resourceTemp;
        */
    }

    public void PlacePiece(string resourceID)
    {
        GameObject resourceTemp = (GameObject)Instantiate(Resources.Load(resourceID));
        resourceTemp.transform.parent = this.transform;
        resourceTemp.transform.position = new Vector2(this.transform.position.x, this.transform.position.y + 20); //slight offset of 10
        Entity = resourceTemp;
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
