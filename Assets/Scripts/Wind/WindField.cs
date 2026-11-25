using System.Collections.Generic;
using UnityEngine;

public class WindField : MonoBehaviour
{
    [SerializeField]
    private WindCell _windCell;

    [SerializeField] private Vector3 _firstHexPosition;
    [SerializeField] private int _generationRadius = 2;
    [SerializeField] private float _radius = 2;
    
    private Dictionary<Vector2Int, WindCell> _windCells = new Dictionary<Vector2Int, WindCell>();
    
    readonly Vector2Int[] NeighborDirections =
    {
        new Vector2Int(1, 0),
        new Vector2Int(1, -1),
        new Vector2Int(0, -1),
        new Vector2Int(-1, 0),
        new Vector2Int(-1, 1),
        new Vector2Int(0, 1),
    };


    private void Start()
    {
        GameObject hexGO = Instantiate(_windCell.gameObject, _firstHexPosition, Quaternion.identity);
        WindCell hex = hexGO.GetComponent<WindCell>();
        hex.SetRadius(_radius);
        
        _windCells.Add(new Vector2Int(0,0), hex);
        
        for (int i = 1; i < _generationRadius + 1; i++)
        {
            Vector2Int currentCoord = new Vector2Int(i, 0);
            for (int j = 0; j < 6; j++)
            {
                for (int k = 0; k < i; k++)
                {
                    Vector2Int direction = new Vector2Int(NeighborDirections[(j + 2) % 6].x, NeighborDirections[(j + 2) % 6].y);
                    
                    Vector2Int nextCoord = currentCoord + direction;
                    currentCoord = nextCoord;

                    if (_windCells.ContainsKey(currentCoord))
                        continue;
            
                    GameObject hexGO1 = Instantiate(_windCell.gameObject, _firstHexPosition, Quaternion.identity);
                    WindCell hex1 = hexGO1.GetComponent<WindCell>();
                    hex1.SetRadius(_radius);
            
                    CalculateHexPosition(hexGO1.transform,hexGO.transform,nextCoord.x,  nextCoord.y);
                }
            }
        }
    }

    private void CalculateHexPosition(Transform hex, Transform parentHex, int q, int r)
    {
        float positionX = Mathf.Sqrt(3) * _radius * (q + (r / 2f)) + parentHex.position.x;
        float positionZ = 3f / 2f * _radius * r + parentHex.position.z;
        
        hex.transform.position = new Vector3(positionX, parentHex.position.y, positionZ);
        _windCells.Add(new Vector2Int(q,r), hex.GetComponent<WindCell>());
    }
}
