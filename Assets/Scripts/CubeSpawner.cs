using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GameData))]
public class CubeSpawner : MonoBehaviour
{
    GameData gameData;
    bool IsfirstRow = true;
    float timeToSpawnNextRow = 0;

    // Start is called before the first frame update
    void Start()
    {
        gameData = GameData.instance;
        SpawnRow();
        SpawnRow();
        timeToSpawnNextRow = Time.time + gameData.waveInterfal;
        gameData.rayField.transform.localScale = new Vector3(gameData.mapWidth + gameData.mapWidth * gameData.cubeSpacing.x, gameData.mapHeight + gameData.mapHeight * gameData.cubeSpacing.y, 1);
        gameData.rayField.transform.localPosition = new Vector3(-Mathf.RoundToInt(gameData.mapWidth % 2f) + Mathf.FloorToInt(gameData.mapWidth / 2f) * gameData.cubeSpacing.x, gameData.rayField.transform.localScale.y / 2f - (Mathf.FloorToInt(gameData.mapHeight / 2f) + 2) * gameData.cubeSpacing.y, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= timeToSpawnNextRow || gameData.cubes.Count <= 3)
            SpawnRow();
    }

    public void SpawnRow()
    {
        if (!IsfirstRow)
        {
            MoveRowUP();
        }
        else
            IsfirstRow = false;
        for (int x = 0; x < gameData.mapWidth; x++)
        {
            SpawnCube(x);
        }

        Gravity.instance.ApplyGravity();
        timeToSpawnNextRow = Time.time + gameData.waveInterfal;
    }

    public void SpawnCube(int x)
    {
        GameObject spawnedCube = Instantiate(gameData.cube, new Vector3(x - Mathf.RoundToInt(gameData.mapWidth / 2f) + x * gameData.cubeSpacing.x, 0,0), new Quaternion(0,0,0,0), gameData.cubesParent);
        spawnedCube.transform.localPosition = new Vector3(x - Mathf.RoundToInt(gameData.mapWidth / 2f) + x * gameData.cubeSpacing.x, 0, 0);
        gameData.cubes.Add(new Vector2Int(x,0), new CubeData(Random.Range(1,gameData.score + 1), spawnedCube, new List<CubeData>(), new Vector2Int(x, 0)));
    }

    private void MoveRowUP()
    {
        for (int y = gameData.mapHeight; y >= 0; y--)
        {
            for (int x = 0; x < gameData.mapWidth; x++)
            {
                if (gameData.cubes.ContainsKey(new Vector2Int(x, y)))
                {
                    if (y == gameData.mapHeight)
                    {
                        Destroy(gameData.cubes[new Vector2Int(x, y)].gameObject);
                        gameData.cubes.Remove(new Vector2Int(x, y));
                    }
                    else
                    {
                        CubeData cube = gameData.cubes[new Vector2Int(x, y)];
                        cube.MoveCube(x,y + 1);
                        gameData.cubes.Add(new Vector2Int(x, y + 1), cube);
                        gameData.cubes.Remove(new Vector2Int(x, y));
                    }
                }
            }
        }
    }
}
