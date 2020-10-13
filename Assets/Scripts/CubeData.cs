using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CubeData
{
    public CubeData(int number, GameObject gameObject, List<CubeData> lockTo, Vector2Int gridPosition)
    {
        this.gridPosition = gridPosition;
        this.number = number;
        this.gameObject = gameObject;
        this.lockTo = lockTo;
        meshRenderer = gameObject.GetComponent<MeshRenderer>();
        textMeshes = gameObject.GetComponentsInChildren<TextMesh>();
        rigidbody = gameObject.GetComponent<Rigidbody>();
        rigidbody.isKinematic = true;
        SetMaterial();
        moving = false;
    }

    public int number;
    public GameObject gameObject;
    public List<CubeData> lockTo;
    public MeshRenderer meshRenderer;
    public TextMesh[] textMeshes;
    public Rigidbody rigidbody;
    public Vector2Int gridPosition;
    public bool moving;

    public void SetMaterial()
    {
        meshRenderer.material = GameData.instance.numberMaterials[number - 1];
        foreach (var textMesh in textMeshes)
        {
            textMesh.text = number.ToString();
        }
    }

    public void IncreaseNumber()
    {
        
        number = Mathf.Clamp(number + 1, 0, GameData.instance.numberMaterials.Length);
        SetMaterial();
        if(number > GameData.instance.score)
        {
            GameData.instance.score = number;
        }
    }

    public void MoveCube(int x, int y)
    {
        if (gameObject)
        {
            gameObject.transform.localPosition = new Vector3(x - Mathf.RoundToInt(GameData.instance.mapWidth / 2f) + x * GameData.instance.cubeSpacing.x, y + y * GameData.instance.cubeSpacing.y, 0);
            gridPosition = new Vector2Int(x, y);
        }
        else
        {
            GameData.instance.cubes.Remove(gridPosition);
        }
    }
}
