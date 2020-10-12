using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public static GameData instance;
    public int score = 5;
    public int highscore = 5;
    public int mapWidth = 7;
    public int mapHeight = 8;
    public float waveInterfal = 15;
    public Material[] numberMaterials;
    public Vector2 cubeSpacing = new Vector2(0.1f, 0.1f);
    public GameObject cube;
    public Dictionary<Vector2Int, CubeData> cubes = new Dictionary<Vector2Int, CubeData>();
    public Transform cubesParent;
    public Collider rayField;

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
}
