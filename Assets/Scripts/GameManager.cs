using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public BoardManager Board;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(Board.Rows);
        Board.GenerateGrid();

        /*
        ///Setting up the initial board for a demo run
        //setup Initial Agent
        SetEntity(new Vector2Int(0, 0), EntityType.agent);

        //Setup Goal States
        SetEntity(new Vector2Int(7, 7), EntityType.goal);
        SetEntity(new Vector2Int(0, 7), EntityType.goal);
        SetEntity(new Vector2Int(6, 1), EntityType.goal);

        //Setup Spikes
        SetEntity(new Vector2Int(2, 0), EntityType.obstacle);
        SetEntity(new Vector2Int(3, 0), EntityType.obstacle);
        SetEntity(new Vector2Int(4, 0), EntityType.obstacle);
        SetEntity(new Vector2Int(5, 0), EntityType.obstacle);
        SetEntity(new Vector2Int(6, 0), EntityType.obstacle);
        SetEntity(new Vector2Int(7, 0), EntityType.obstacle);
        SetEntity(new Vector2Int(1, 2), EntityType.obstacle);
        SetEntity(new Vector2Int(1, 3), EntityType.obstacle);
        SetEntity(new Vector2Int(2, 3), EntityType.obstacle);
        SetEntity(new Vector2Int(3, 5), EntityType.obstacle);
        SetEntity(new Vector2Int(4, 5), EntityType.obstacle);
        SetEntity(new Vector2Int(4, 1), EntityType.obstacle);
        SetEntity(new Vector2Int(7, 1), EntityType.obstacle);
        SetEntity(new Vector2Int(1, 6), EntityType.obstacle);
        SetEntity(new Vector2Int(1, 7), EntityType.obstacle);
        SetEntity(new Vector2Int(2, 7), EntityType.obstacle);
        SetEntity(new Vector2Int(3, 7), EntityType.obstacle);
        SetEntity(new Vector2Int(4, 7), EntityType.obstacle);
        SetEntity(new Vector2Int(5, 7), EntityType.obstacle);
        SetEntity(new Vector2Int(6, 3), EntityType.obstacle);
        SetEntity(new Vector2Int(7, 3), EntityType.obstacle);
        SetEntity(new Vector2Int(7, 4), EntityType.obstacle);
        SetEntity(new Vector2Int(7, 5), EntityType.obstacle);
        SetEntity(new Vector2Int(7, 6), EntityType.obstacle);
        */
    }


    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Up");
    }

    public void SetEntity(Vector2Int tileCoords, EntityType entityType)
    {
        Cell subjectCell = Board.BoardCells[tileCoords.x, tileCoords.y];

        switch (entityType)
        {
            case EntityType.agent:
                GameObject prefabAgent = (GameObject)Instantiate(Resources.Load("agent_idle"));
                Agent agent = prefabAgent.GetComponent<Agent>();
                agent.SetupAgent(subjectCell);
                break;
            case EntityType.goal:
                GameObject prefabGoal = (GameObject)Instantiate(Resources.Load("goal_idle"));
                GoalState goal = prefabGoal.GetComponent<GoalState>();
                goal.SetupAgent(subjectCell);
                break;
            case EntityType.obstacle:
                GameObject prefabObstacle = (GameObject)Instantiate(Resources.Load("obstacle_idle"));
                ObstacleState obstacle = prefabObstacle.GetComponent<ObstacleState>();
                obstacle.SetupAgent(subjectCell);
                break;
        }
    }

    public enum EntityType
    {
        agent,
        goal,
        obstacle,
        empty
    }
}
