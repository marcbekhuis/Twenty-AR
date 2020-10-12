using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandeler : MonoBehaviour
{
    CubeData movingCube;
    Vector2Int movingCubeGridPosition;
    [SerializeField] Camera camera;

    // Update is called once per frame
    void Update()
    {
        if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && movingCube == null)
            {
                Vector2 position = GetPosition(Input.mousePosition);
                Vector2Int gridPosition = new Vector2Int(Mathf.RoundToInt(position.x + GameData.instance.mapWidth / 2f), Mathf.RoundToInt(position.y));
                if (GameData.instance.cubes.ContainsKey(gridPosition))
                {
                    movingCube = GameData.instance.cubes[gridPosition];
                    movingCubeGridPosition = gridPosition;
                    movingCube.rigidbody.isKinematic = false;
                    MoveCube(position);
                }
            }
            else if (Input.GetKeyUp(KeyCode.Mouse0) && movingCube != null)
            {
                movingCube.gameObject.transform.localPosition = new Vector2(movingCubeGridPosition.x - Mathf.RoundToInt(GameData.instance.mapWidth / 2f) + movingCubeGridPosition.x * GameData.instance.cubeSpacing.x, movingCubeGridPosition.y + movingCubeGridPosition.y * GameData.instance.cubeSpacing.y);
                movingCube.rigidbody.isKinematic = true;
                movingCube = null;
            }
            if (Input.GetKey(KeyCode.Mouse0) && movingCube != null)
            {
                Vector2 position = GetPosition(Input.mousePosition);
                MoveCube(position);
            }
        }
        else
        {
            foreach (var touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Began)
                {
                    Vector2 position = GetPosition(touch.position);
                    Vector2Int gridPosition = new Vector2Int(Mathf.RoundToInt(position.x + GameData.instance.mapWidth / 2f), Mathf.RoundToInt(position.y));
                    if (GameData.instance.cubes.ContainsKey(gridPosition))
                    {
                        movingCube = GameData.instance.cubes[gridPosition];
                        movingCubeGridPosition = gridPosition;
                        movingCube.rigidbody.isKinematic = false;
                        MoveCube(position);
                    }
                }
                else if (touch.phase == TouchPhase.Moved)
                {
                    movingCube.gameObject.transform.localPosition = new Vector2(movingCubeGridPosition.x - Mathf.RoundToInt(GameData.instance.mapWidth / 2f) + movingCubeGridPosition.x * GameData.instance.cubeSpacing.x, movingCubeGridPosition.y + movingCubeGridPosition.y * GameData.instance.cubeSpacing.y);
                    movingCube.rigidbody.isKinematic = true;
                    movingCube = null;
                }
                if (touch.phase == TouchPhase.Moved)
                {
                    Vector2 position = GetPosition(touch.position);
                    MoveCube(position);
                }
                break;
            }
        }
    }

    public void MoveCube(Vector2 position)
    {
        movingCube.rigidbody.MovePosition(position);
        Vector2Int gridPosition = new Vector2Int(Mathf.RoundToInt(position.x + GameData.instance.mapWidth / 2f), Mathf.RoundToInt(position.y));
        if (GameData.instance.cubes.ContainsKey(gridPosition))
        {
            if (gridPosition != movingCubeGridPosition)
            {
                if (movingCube.number == GameData.instance.cubes[gridPosition].number)
                {
                    GameData.instance.cubes[gridPosition].IncreaseNumber();
                    Destroy(movingCube.gameObject);
                    GameData.instance.cubes.Remove(movingCubeGridPosition);
                    movingCube = null;
                }
            }
        }
        else if(gridPosition.x < GameData.instance.mapWidth && gridPosition.x >= 0 && gridPosition.y >= 0)
        {
            GameData.instance.cubes.Add(gridPosition, movingCube);
            GameData.instance.cubes.Remove(movingCubeGridPosition);
            movingCubeGridPosition = gridPosition;
        }
        Gravity.instance.ApplyGravity();
    }

    private Vector2 GetPosition(Vector3 position)
    {
        Ray ray = camera.ScreenPointToRay(position);
        RaycastHit hit;
        Physics.Raycast(ray, out hit);
        if (hit.transform)
        {
            return hit.point;
        }
        return default;
    }
}
