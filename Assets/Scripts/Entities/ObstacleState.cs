using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleState : BaseState
{
    public override void DisplayEntity(Cell parent)
    {
        parentCell = parent;

        Vector2 parentPosition = parentCell.GetTileCenter();
        transform.position = new Vector3(parentPosition.x, parentPosition.y, -1);

        parentCell.RewardDisplay.text = "X";
    }
}
