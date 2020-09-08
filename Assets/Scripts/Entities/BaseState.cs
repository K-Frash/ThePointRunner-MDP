using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BaseState : EventTrigger
{
    protected Cell parentCell;
    protected float Reward = 0;
    public virtual void SetupAgent(Cell parent)
    {
        parentCell = parent;

        Vector2 parentPosition = parentCell.GetTileCenter();
        transform.position = new Vector3(parentPosition.x, parentPosition.y, -1);
    }
}