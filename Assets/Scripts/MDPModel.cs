using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Data;
using System.IO;
using System.Linq;
using UnityEditor;
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
    private List<Vector2Int> GoalStates = new List<Vector2Int>();

    // An array of all Obstacle positions within the current simulation
    private List<Vector2Int> Obstacles = new List<Vector2Int>();

    //For simplicity we start with only one agent ~ Location of the agent on the board
    private Vector2Int AgentPosn;

    /// <summary>
    /// This is the set of information needed by the Bellman Equations to perform
    /// the Markov Decision Process and allow our agent to derive the optimal policy on any board!
    /// </summary>

    //Matricies of cell rewards ~ One for visualization and one for reward weight updates
    private string[,] VisualizedMatrix;
    private float[,] RewardMatrix;

    //Threshold for termination
    private float TerminationThreshold = 000.1f;

    //Discount factor to adjust reward weights
    private float DiscountFactor = 0.9f;

    // Probability of the agent going it's intended direction
    float IntendedProb = 0.8f;

    // Probability of the agent going an unintended direction
    float UnintendedProb = 0.1f;

    // Reward per iteration
    float RewardStep = -0.05f;

    public void SetUp(BaseState[,] sceneMap)
    {
        int rows = sceneMap.GetLength(0);
        int cols = sceneMap.GetLength(1);

        // Initialize Reward Matrix
        VisualizedMatrix = new string[rows, cols];
        RewardMatrix = new float[rows, cols];

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
                else if (currentEntityType == typeof(GoalState)) GoalStates.Add(new Vector2Int(i, j));
                else if (currentEntityType == typeof(ObstacleState)) Obstacles.Add(new Vector2Int(i, j));

                // Set's origional value for V_0 of the reward matrix
                VisualizedMatrix[rows - i - 1, j] = currentOrigionalReward;

                // Setup the V0 reward matrix for weight transitions per step
                float translation;
                if (currentEntityType == typeof(ObstacleState)) translation = 0f;
                else if (currentEntityType == typeof(EmptyState) || currentEntityType == typeof(Agent)) translation = RewardStep; //since our agent should get rewarded per step
                else translation = float.Parse(currentOrigionalReward); //Goal state or Agent state
                RewardMatrix[rows - i - 1, j] = translation;
            }
        }

        // A sanity check used in testing to ensure the backend and front end match
        string resStr = "";
        for(int i = 0; i < rows; i++)
        {
            string rr = "";
            for(int j = 0; j < cols; j++)
            {
                rr += VisualizedMatrix[i, j] + " ";
            }
            resStr += rr + "\n";
        }

        string resStr2 = "";
        for (int i = 0; i < rows; i++)
        {
            string rr2 = "";
            for (int j = 0; j < cols; j++)
            {
                rr2 += RewardMatrix[i, j].ToString() + " ";
            }
            resStr2 += rr2 + "\n";
        }

        Debug.Log(resStr);
        Debug.Log(resStr2);
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

            UpdateEverySecond();
        }

    }

    // Update is called once per second
    void UpdateEverySecond()
    {
        //MoveAgent(Vector2Int.up + Vector2Int.right);

        //Let's calculate V*(s)!

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

    #region BellMan Equations

    /// <summary>
    /// Determine if the maximum difference between any two cells between transition is < TerminationThreshold
    /// </summary>
    /// <param name="pastV"></param>
    /// <param name="curV"></param>
    /// <returns></returns>
    public bool TerminateCondition(string[,] pastV, string[,] curV)
    {
        // Initial iteration safeguard: TODO there can be a cleaner solution to this
        if (curV == null) return false;

        // Searching to determine if the value transitions between the two states have stopped
        // changing up to Threshold decimal places
        float maxS = float.MinValue;
        for(int i = 0; i < Board.Rows; i++)
        {
            for (int j = 0; j < Board.Cols; j++)
            {
                Vector2Int coord = new Vector2Int(i, j);
                //These states never change
                if (GoalStates.Contains(coord) || Obstacles.Contains(coord)) continue;

                float diffS = Math.Abs( float.Parse(pastV[i, j]) - float.Parse(curV[i, j])); //TODO: Check parse authenticity
                maxS = (diffS > maxS) ? diffS : maxS;
            }
        }

        return maxS > TerminationThreshold;
    }

    /// <summary>
    /// Implements the Q function in the Bellman Equations
    /// 
    /// This Determines which direction grants the most benefit to the agent considering the
    /// weighted probability of the agent moving to an unintended tile
    /// </summary>
    /// <param name="Vs"></param>
    /// <param name="Vmatrix"></param>
    /// <returns></returns>
    public float Q(Vector2Int Vs, string[,] Vmatrix)
    {
        //Coordinates:
        // Note: The agent can either move in the intended direction or unintended dependant of probabilities
        //       set before the emulation (denoted by IntendedDirection and UnintendedDirection)
        Vector2Int north = Vector2Int.up;
        Vector2Int east = Vector2Int.right;
        Vector2Int south = Vector2Int.down;
        Vector2Int west = Vector2Int.left;

        List<Vector2Int> dirList = new List<Vector2Int> { north, east, south, west};

        //Current best reward agent can get moving in a given direction
        float curMax = float.MinValue;

        //List of directions for the agent to move in, used in the value iteration calculation
        foreach(Vector2Int intendedDir in dirList)
        {
            float totalBenefit;
            //dirs = new List<Vector2Int> { intended };
            //dirs.Concat( dirList.FindAll(e => (e != intendedDir)).ToList() );

            Vector2Int intendedCoor = Vs + intendedDir;

            if (intendedCoor.x >= 0 &&
                intendedCoor.x < Board.Rows &&
                intendedCoor.y >= 0 &&
                intendedCoor.y < Board.Cols &&
                !Obstacles.Contains(intendedCoor))
            {
                totalBenefit = IntendedProb * float.Parse(Vmatrix[intendedCoor.x,intendedCoor.y]);
            }
            else
            {
                totalBenefit = IntendedProb * float.Parse(Vmatrix[Vs.x, Vs.y]);
            }

            
            foreach(Vector2Int unintendedDir in (dirList.FindAll(e => (e != intendedDir)).ToList()))
            {
                Vector2Int unintendedCoor = Vs + unintendedDir;

                //We are not going outside the board or hitting an obstacle, if we are then we take our current position
                if (unintendedCoor.x >= 0 &&
                    unintendedCoor.x < Board.Rows &&
                    unintendedCoor.y >= 0 &&
                    unintendedCoor.y < Board.Cols &&
                    !Obstacles.Contains(unintendedCoor))
                {
                    totalBenefit += UnintendedProb * float.Parse(Vmatrix[unintendedCoor.x, unintendedCoor.y]);
                }
                else
                {
                    totalBenefit += UnintendedProb * float.Parse(Vmatrix[Vs.x, Vs.y]);
                }
            }
            // Finding the max benefit at a given tile
            curMax = totalBenefit > curMax ? totalBenefit : curMax;
        }

        return curMax;
    }

    public string nextV(float Rs, float phi, Vector2Int coords, string[,] Vmap)
    {
        if (Obstacles.Contains(coords)) return "X";
        float res = Rs + (phi * Q(coords, Vmap));
        return res.ToString();
    }

    public string[,] MDP(float[,] Rmap, float phi, bool showSteps)
    {
        int rows = VisualizedMatrix.GetLength(0);
        int cols = VisualizedMatrix.GetLength(1);

        string[,] curMap = VisualizedMatrix;
        string[,] nextMap = null;

        int pos = 0;
        while(!TerminateCondition(curMap, nextMap))
        {
            if(nextMap == null)
            {
                //Need to deepcopy curMap
                nextMap = curMap.Clone() as string[,];
            }

            curMap = nextMap.Clone() as string[,];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (Obstacles.Contains(new Vector2Int(i, j)) || GoalStates.Contains(new Vector2Int(i, j))) continue;
                    nextMap[i, j] = nextV(Rmap[i, j], phi, new Vector2Int(i, j), curMap);
                }
            }

            pos += 1; //Used for V iteration tracking
        }

        return nextMap;
    }

    #endregion
}
