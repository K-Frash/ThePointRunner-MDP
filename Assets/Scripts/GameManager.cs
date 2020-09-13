using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public BoardManager Board;

    public MDPModel Model;

    //Load in map data via a matric of strings where;
    // - EntityType.obstacle is an obstacle
    // - "A" is the agent
    // - "G" is a goal state
    // - EntityType.empty is an empty state
    public BaseState[,] sceneSetup;

    void Start()
    {
        //Demo Scene on any first load
        sceneSetup = new BaseState[,]
        {
            {new GoalState("-1"), new ObstacleState("X"), new ObstacleState("X"), new ObstacleState("X"), new ObstacleState("X"), new ObstacleState("X"), new EmptyState("0")   , new GoalState("5")    },
            {new EmptyState("0"), new ObstacleState("X"), new EmptyState("0")   , new EmptyState("0")   , new EmptyState("0")   , new EmptyState("0")   , new EmptyState("0")   , new ObstacleState("X")},
            {new EmptyState("0"), new EmptyState("0")   , new EmptyState("0")   , new ObstacleState("X"), new ObstacleState("X"), new EmptyState("0")   , new EmptyState("0")   , new ObstacleState("X")},
            {new EmptyState("0"), new EmptyState("0")   , new EmptyState("0")   , new EmptyState("0")   , new EmptyState("0")   , new EmptyState("0")   , new EmptyState("0")   , new ObstacleState("X")},
            {new EmptyState("0"), new ObstacleState("X"), new ObstacleState("X"), new EmptyState("0")   , new EmptyState("0")   , new EmptyState("0")   , new ObstacleState("X"), new ObstacleState("X")},
            {new EmptyState("0"), new ObstacleState("X"), new EmptyState("0")   , new EmptyState("0")   , new EmptyState("0")   , new EmptyState("0")   , new EmptyState("0")   , new EmptyState("0")   },
            {new EmptyState("0"), new EmptyState("0")   , new EmptyState("0")   , new EmptyState("0")   , new ObstacleState("X"), new EmptyState("0")   , new GoalState("1")    , new ObstacleState("X")},
            {new Agent("0")     , new EmptyState("0")   , new ObstacleState("X"), new ObstacleState("X"), new ObstacleState("X"), new ObstacleState("X"), new ObstacleState("X"), new ObstacleState("X")}
        };

        // Generates empty cells
        Board.GenerateGrid();

        Model.SetUp(sceneSetup);
    }


    // Update is called once per frame
    void Update()
    {
        //Model.Step();
    }
}
