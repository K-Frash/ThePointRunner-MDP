using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Assertions.Must;
using static GameManager;

/// <summary>
/// This class implements the Markov Decision Process algorithm.
/// Due to it's tight coupling with the board pieces, it acts as
/// an entity manager to move the agent through it's environment
/// </summary>
public class MDPModel : MonoBehaviour
{
    public BoardManager Board;

    // We generate states dynamically!
    public GameObject StatePrefab;

    //A Map of EntityType to their respective resource ID in our Resources Directory for quick lookup
    public Dictionary<Type, string> entityIDCatalouge = new Dictionary<Type, string>
    {
        [typeof(Agent)]         = "agent_idle",
        [typeof(GoalState)]     = "goal_idle",
        [typeof(ObstacleState)] = "obstacle_idle",
        [typeof(EmptyState)]    = "empty_idle"
    };

    // An array of all Goal States within the current simulation
    private List<(int, int)> GoalStates = new List<(int, int)>();

    // An array of all Obstacle positions within the current simulation
    private List<(int, int)> Obstacles = new List<(int, int)>();

    //For simplicity we start with only one agent ~ Location of the agent on the board
    private Vector2Int AgentPosn;
    float intendedProb = 0.8f;
    float unintendedProb = 0.1f;

    //Matrix of cell rewards
    private string[,] RewardMatrix;

    public void SetUp(BaseState[,] sceneMap)
    {
        int rows = sceneMap.GetLength(0);
        int cols = sceneMap.GetLength(1);

        // Initialize Reward Matrix
        RewardMatrix = new string[rows, cols];

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                Type currentEntityType = sceneMap[rows - i - 1, j].GetType();
                string currentOrigionalReward = sceneMap[rows - i - 1, j].GetReward();

                //Populate the given cell with the entity
                SetEntity(new Vector2Int(i, j), currentEntityType, currentOrigionalReward);

                //Populate fields for the states
                if (currentEntityType == typeof(Agent)) AgentPosn = new Vector2Int(i, j);
                else if (currentEntityType == typeof(GoalState)) GoalStates.Add((i, j));
                else if (currentEntityType == typeof(ObstacleState)) Obstacles.Add((i, j));

                // Set's origional value for V_0 of the reward matrix
                RewardMatrix[rows - i - 1, j] = currentOrigionalReward;
            }
        }

        string resStr = "";
        // Sanity Check
        for(int i = 0; i < rows; i++)
        {
            string rr = "";
            for(int j = 0; j < cols; j++)
            {
                rr += RewardMatrix[i, j].ToString() + " ";
            }
            resStr += rr + "\n";
        }

        Debug.Log(resStr);
    }

    #region Update Cycle
    // Next update in second
    private int nextUpdate = 1;

    // Update is called once per frame
    void Update()
    {

        // If the next update is reached
        if (Time.time >= nextUpdate)
        {
            //Debug.Log(Time.time + ">=" + nextUpdate);
            // Change the next update (current second+1)
            nextUpdate = Mathf.FloorToInt(Time.time) + 1;
            // Call your fonction
            UpdateEverySecond();
        }

    }

    // Update is called once per second
    void UpdateEverySecond()
    {
        MoveAgent(Vector2Int.up + Vector2Int.right);
    }

    #endregion

    private void MoveAgent(Vector2Int dir)
    {
        //TODO: Temp right now, just for testing
        Board.MoveEntities(AgentPosn, dir);
        AgentPosn += dir;
    }

    //Once the Initial Board is set, it is time for us to perform MDP to get the agent's optimal policy
    //In it's environment!
    public void Step()
    {

    }

    public void SetEntity(Vector2Int tileCoords, Type entityType, string origReward)
    {
        Cell subjectCell = Board.BoardCells[tileCoords.y, tileCoords.x];
        string entityID = entityIDCatalouge[entityType];

        // Generates a prefab instance from our resources directory
        GameObject resourceInit = (GameObject)Instantiate(Resources.Load(entityID));

        BaseState newState = (BaseState)resourceInit.AddComponent(entityType);
        newState.SetReward(origReward);

        subjectCell.PlacePiece(newState);
    }

    public void UpdateCellRewards(string[,] rewardMatrix)
    {
        int rows = rewardMatrix.GetLength(0);
        int cols = rewardMatrix.GetLength(1);

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                Board.BoardCells[i, j].RewardDisplay.text = rewardMatrix[i, j];
            }
        }
    }
}
