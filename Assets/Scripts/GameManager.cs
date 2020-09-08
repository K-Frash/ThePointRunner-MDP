using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public BoardManager Board;

    //Load in map data via a matric of strings where;
    // - EntityType.obstacle is an obstacle
    // - "A" is the agent
    // - "G" is a goal state
    // - EntityType.empty is an empty state
    public EntityType[,] sceneSetup;

    public string[,] sceneRewardMatrix;

    void Start()
    {
        Debug.Log(Board.Rows);
        Board.GenerateGrid();

        //Demo Scene on any first load
        sceneSetup = new EntityType[,]
        {
            {EntityType.goal , EntityType.obstacle, EntityType.obstacle, EntityType.obstacle, EntityType.obstacle, EntityType.obstacle, EntityType.empty   , EntityType.goal    },
            {EntityType.empty, EntityType.obstacle, EntityType.empty   , EntityType.empty   , EntityType.empty   , EntityType.empty   , EntityType.empty   , EntityType.obstacle},
            {EntityType.empty, EntityType.empty   , EntityType.empty   , EntityType.obstacle, EntityType.obstacle, EntityType.empty   , EntityType.empty   , EntityType.obstacle},
            {EntityType.empty, EntityType.empty   , EntityType.empty   , EntityType.empty   , EntityType.empty   , EntityType.empty   , EntityType.empty   , EntityType.obstacle},
            {EntityType.empty, EntityType.obstacle, EntityType.obstacle, EntityType.empty   , EntityType.empty   , EntityType.empty   , EntityType.obstacle, EntityType.obstacle},
            {EntityType.empty, EntityType.obstacle, EntityType.empty   , EntityType.empty   , EntityType.empty   , EntityType.empty   , EntityType.empty   , EntityType.empty   },
            {EntityType.empty, EntityType.empty   , EntityType.empty   , EntityType.empty   , EntityType.obstacle, EntityType.empty   , EntityType.goal    , EntityType.obstacle},
            {EntityType.agent, EntityType.empty   , EntityType.obstacle, EntityType.obstacle, EntityType.obstacle, EntityType.obstacle, EntityType.obstacle, EntityType.obstacle}
        };

        // Sets up whatever configuration is present in sceneSetup
        SetUpMap();
    }


    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Up");
    }

    public void SetUpMap()
    {
        int rows = sceneSetup.GetLength(0);
        int cols = sceneSetup.GetLength(1);
        for (int i = 0; i < rows; i++)
        {
            for(int j = 0; j < cols; j++)
            {
                // Need to flip the x coord due to cartesian coordinate system for the canvas
                SetEntity(new Vector2Int(i, j), sceneSetup[rows - i - 1, j]);
            }
        }
    }

    public void SetEntity(Vector2Int tileCoords, EntityType entityType)
    {
        Cell subjectCell = Board.BoardCells[tileCoords.y, tileCoords.x];
        string entityID = entityIDCatalouge[entityType];
        if(entityID != "") //TODO: there can be a cleaner condition check here
        {
            subjectCell.PlacePiece(entityID);
        }
    }

    public enum EntityType
    {
        agent,
        goal,
        obstacle,
        empty
    }

    public Dictionary<EntityType, string> entityIDCatalouge = new Dictionary<EntityType, string>
    {
        [EntityType.agent] = "agent_idle",
        [EntityType.goal] = "goal_idle",
        [EntityType.obstacle] = "obstacle_idle",
        [EntityType.empty] = ""
    };
}
