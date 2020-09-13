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
    public (EntityType, string)[,] sceneSetup;

    public string[,] sceneRewardMatrix;

    void Start()
    {
        //Demo Scene on any first load
        sceneSetup = new (EntityType, string)[,]
        {
            {(EntityType.goal,  "-1"), (EntityType.obstacle, "X"), (EntityType.obstacle, "X"), (EntityType.obstacle, "X"), (EntityType.obstacle, "X"), (EntityType.obstacle, "X"), (EntityType.empty,    "0"), (EntityType.goal,     "5")},
            {(EntityType.empty, "0"), (EntityType.obstacle, "X"), (EntityType.empty,    "0"), (EntityType.empty,    "0"), (EntityType.empty,    "0"), (EntityType.empty,    "0"), (EntityType.empty,    "0"), (EntityType.obstacle, "X")},
            {(EntityType.empty, "0"), (EntityType.empty,    "0"), (EntityType.empty,    "0"), (EntityType.obstacle, "X"), (EntityType.obstacle, "X"), (EntityType.empty,    "0"), (EntityType.empty,    "0"), (EntityType.obstacle, "X")},
            {(EntityType.empty, "0"), (EntityType.empty,    "0"), (EntityType.empty,    "0"), (EntityType.empty,    "0"), (EntityType.empty,    "0"), (EntityType.empty,    "0"), (EntityType.empty,    "0"), (EntityType.obstacle, "X")},
            {(EntityType.empty, "0"), (EntityType.obstacle, "X"), (EntityType.obstacle, "X"), (EntityType.empty,    "0"), (EntityType.empty,    "0"), (EntityType.empty,    "0"), (EntityType.obstacle, "X"), (EntityType.obstacle, "X")},
            {(EntityType.empty, "0"), (EntityType.obstacle, "X"), (EntityType.empty,    "0"), (EntityType.empty,    "0"), (EntityType.empty,    "0"), (EntityType.empty,    "0"), (EntityType.empty,    "0"), (EntityType.empty,    "0")},
            {(EntityType.empty, "0"), (EntityType.empty,    "0"), (EntityType.empty,    "0"), (EntityType.empty,    "0"), (EntityType.obstacle, "X"), (EntityType.empty,    "0"), (EntityType.goal,     "1"), (EntityType.obstacle, "X")},
            {(EntityType.agent, "0"), (EntityType.empty,    "0"), (EntityType.obstacle, "X"), (EntityType.obstacle, "X"), (EntityType.obstacle, "X"), (EntityType.obstacle, "X"), (EntityType.obstacle, "X"), (EntityType.obstacle, "X")}
        };

        // Generates empty cells
        Board.GenerateGrid();

        Model.SetUp(sceneSetup, Board);
    }


    // Update is called once per frame
    void Update()
    {
        Model.Step();
    }

    public enum EntityType
    {
        agent,
        goal,
        obstacle,
        empty
    }
}
