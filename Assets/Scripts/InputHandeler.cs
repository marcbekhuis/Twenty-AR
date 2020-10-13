using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandeler : MonoBehaviour
{
    CubeData movingCube;
    [SerializeField] Camera camera;

    // Update is called once per frame
    void Update()
    {
        if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (movingCube == null)
                {
                    Vector2 position = GetPosition(Input.mousePosition);
                    Vector2Int gridPosition = new Vector2Int(Mathf.RoundToInt(position.x / GameData.instance.cubesParent.transform.localScale.x * 1 + GameData.instance.mapWidth / 2f), Mathf.RoundToInt(position.y / GameData.instance.cubesParent.transform.localScale.y));
                    if (GameData.instance.cubes.ContainsKey(gridPosition))
                    {
                        movingCube = GameData.instance.cubes[gridPosition];
                        movingCube.rigidbody.isKinematic = false;
                        movingCube.moving = true;
                        MoveCube(position);
                    }
                }
            }
            else if (Input.GetKey(KeyCode.Mouse0))
            {
                if (movingCube != null)
                {
                    Vector2 position = GetPosition(Input.mousePosition);
                    MoveCube(position);
                }
            }
            else if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                if (movingCube != null)
                {
                    movingCube.MoveCube(movingCube.gridPosition.x, movingCube.gridPosition.y);
                    movingCube.rigidbody.isKinematic = true;
                    movingCube.moving = false;
                    movingCube = null;
                }
            }
        }
        else
        {
            foreach (var touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Began)
                {
                    if (movingCube == null)
                    {
                        Vector2 position = GetPosition(touch.position);
                        Vector2Int gridPosition = new Vector2Int(Mathf.RoundToInt(position.x / GameData.instance.cubesParent.transform.localScale.x * 1 + GameData.instance.mapWidth / 2f), Mathf.RoundToInt(position.y / GameData.instance.cubesParent.transform.localScale.y));
                        if (GameData.instance.cubes.ContainsKey(gridPosition))
                        {
                            movingCube = GameData.instance.cubes[gridPosition];
                            movingCube.rigidbody.isKinematic = false;
                            movingCube.moving = true;
                            MoveCube(position);
                        }
                    }
                }
                else if (touch.phase == TouchPhase.Moved)
                {
                    if (movingCube != null)
                    {
                        Vector2 position = GetPosition(touch.position);
                        MoveCube(position);
                    }
                }
                else if (touch.phase == TouchPhase.Ended)
                {
                    if (movingCube != null)
                    {
                        movingCube.MoveCube(movingCube.gridPosition.x, movingCube.gridPosition.y);
                        movingCube.rigidbody.isKinematic = true;
                        movingCube.moving = false;
                        movingCube = null;
                    }
                }
                break;
            }
        }
    }

    public void MoveCube(Vector2 position)
    {
        if (movingCube == null)
            return;
        if (!movingCube.rigidbody)
            return;
        movingCube.rigidbody.MovePosition(position);
        Vector2Int gridPosition = new Vector2Int(Mathf.RoundToInt(position.x / GameData.instance.cubesParent.transform.localScale.x * 1 + GameData.instance.mapWidth / 2f), Mathf.RoundToInt(position.y / GameData.instance.cubesParent.transform.localScale.y));
        if (GameData.instance.cubes.ContainsKey(gridPosition))
        {
            if (gridPosition != movingCube.gridPosition)
            {
                if (Vector2.Distance(gridPosition, movingCube.gridPosition) <= 1)
                {
                    if (movingCube.number == GameData.instance.cubes[gridPosition].number)
                    {
                        GameData.instance.cubes[gridPosition].IncreaseNumber();
                        Destroy(movingCube.gameObject);
                        GameData.instance.cubes.Remove(movingCube.gridPosition);
                        movingCube = null;
                    }
                }
            }
        }
        else if(gridPosition.x < GameData.instance.mapWidth && gridPosition.x >= 0 && gridPosition.y >= 0)
        {
            if (Vector2.Distance(gridPosition, movingCube.gridPosition) <= 1)
            {
                Debug.LogError("Moving Cube around");
                GameData.instance.cubes.Add(gridPosition, movingCube);
                GameData.instance.cubes.Remove(movingCube.gridPosition);
            }
        }
        Gravity.instance.ApplyGravity();
    }

    private Vector2 GetPosition(Vector3 position)
    {
        Ray ray = camera.ScreenPointToRay(position);
        RaycastHit hit;
        Debug.DrawRay(ray.origin, ray.direction * 100);
        Physics.Raycast(ray, out hit);
        if (hit.transform)
        {
            return hit.point;
        }
        return default;
    }
}
