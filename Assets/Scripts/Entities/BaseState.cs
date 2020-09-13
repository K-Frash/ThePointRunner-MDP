using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BaseState : EventTrigger
{
    protected Cell ParentCell;
    protected string Reward { get; set; }

    public virtual void DisplayEntity(Cell parent)
    {
        ParentCell = parent;

        Vector2 parentPosition = ParentCell.GetTileCenter();
        transform.position = new Vector3(parentPosition.x, parentPosition.y, -1);
    }
    
    public string GetReward()
    {
        return Reward;
    }
    
    public void SetReward(string reward)
    {
        Reward = reward;
    }
}