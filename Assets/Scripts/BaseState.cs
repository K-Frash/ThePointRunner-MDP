using UnityEngine;
using UnityEngine.UI;

public class BaseState : MonoBehaviour
{
    protected Cell Parent;
    protected SpriteRenderer ObjectRender;
    protected Animator ObjectAnimation;
    protected float Reward = 0;
    public virtual void SetupAgent(Cell parent)
    {
        Parent = parent;
        ObjectRender = this.GetComponent<SpriteRenderer>();
        ObjectAnimation = this.GetComponent<Animator>();

        Vector2 parentPosition = Parent.GetTileCenter();
        transform.position = new Vector3(parentPosition.x, parentPosition.y, -1);
    }
}