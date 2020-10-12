using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeData
{
    public CubeData(int number, GameObject gameObject, List<CubeData> lockTo)
    {
        this.number = number;
        this.gameObject = gameObject;
        this.lockTo = lockTo;
        meshRenderer = gameObject.GetComponent<MeshRenderer>();
        textMeshes = gameObject.GetComponentsInChildren<TextMesh>();
        rigidbody = gameObject.GetComponent<Rigidbody>();
        rigidbody.isKinematic = true;
        SetMaterial();
    }

    public int number;
    public GameObject gameObject;
    public List<CubeData> lockTo;
    public MeshRenderer meshRenderer;
    public TextMesh[] textMeshes;
    public Rigidbody rigidbody;

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
        number++;
        SetMaterial();
        if(number > GameData.instance.score)
        {
            GameData.instance.score = number;
        }
    }
}
