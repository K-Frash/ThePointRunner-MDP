using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BaseState : EventTrigger
{
    protected Cell parentCell;
    public virtual void DisplayEntity(Cell parent)
    {
        parentCell = parent;

        Vector2 parentPosition = parentCell.GetTileCenter();
        transform.position = new Vector3(parentPosition.x, parentPosition.y, -1);
    }
}