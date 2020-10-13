using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    public static Gravity instance;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void ApplyGravity()
    {
        GameData gameData = GameData.instance;
        for (int x = 0; x < gameData.mapWidth; x++)
        {
            for (int y = 1; y < gameData.mapHeight + 1; y++)
            {
                if (gameData.cubes.ContainsKey(new Vector2Int(x,y)))
                {
                    bool RunAgain = true;
                    do
                    {
                        int cubesCount = gameData.cubes.Count;
                        RunAgain = ApplyGravityToCube(new Vector2Int(x, y), gameData);
                        if (RunAgain)
                            y--;
                    } while (RunAgain);
                }
            }
        }
    }

    private bool ApplyGravityToCube(Vector2Int gridPosition, GameData gameData)
    {
        if (!gameData.cubes[gridPosition].moving)
        {
            if (gameData.cubes.ContainsKey(gridPosition - new Vector2Int(0, 1)))
            {
                CubeData cube = gameData.cubes[gridPosition];
                CubeData cubeLower = gameData.cubes[gridPosition - new Vector2Int(0, 1)];
                if (cube.number == cubeLower.number)
                {
                    Destroy(cube.gameObject);
                    gameData.cubes.Remove(gridPosition);
                    cubeLower.IncreaseNumber();
                }
            }
            else
            {
                CubeData cube = gameData.cubes[gridPosition];
                cube.MoveCube(gridPosition.x, gridPosition.y - 1);
                gameData.cubes.Add(gridPosition - new Vector2Int(0, 1), cube);
                gameData.cubes.Remove(gridPosition);
            }
            if (!gameData.cubes.ContainsKey(gridPosition - new Vector2Int(0, 2)) && gridPosition.y - 2 >= 0)
                return true;
            else
                return false;
        }
        return false;
    }
}
