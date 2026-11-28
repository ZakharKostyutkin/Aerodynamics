using System;
using System.Collections.Generic;
using UnityEngine;

public class WindField : MonoBehaviour
{
    [SerializeField] private WindFieldRenderer _renderer;
    [SerializeField] private Vector3 _firstHexPosition;
    [SerializeField] private int _generationRadius = 2;
    [SerializeField] private float _radius = 2;
    
    private readonly Dictionary<Vector2Int, AirCellData> _airCellDatas = new Dictionary<Vector2Int, AirCellData>();
    
    static readonly Vector2Int[] NeighborDirections =
    {
        new Vector2Int(1, 0),
        new Vector2Int(1, -1),
        new Vector2Int(0, -1),
        new Vector2Int(-1, 0),
        new Vector2Int(-1, 1),
        new Vector2Int(0, 1),
    };


    private void Awake()
    {
        Vector2Int startCoord = Vector2Int.zero;

        GenerateAir(startCoord);
        EditAir(startCoord, Vector3.right * 2); 
        
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
            
                    GenerateAir(nextCoord);
                }
            }
        }
        
        _renderer.SetRadius(_radius);
        _renderer.SetFieldOrigin(_firstHexPosition);
        _renderer.DrawAirField(_airCellDatas);
        
        _renderer.InitializeMesh();
    }

    

    private void EditAir(Vector2Int coord, Vector3 direction)
    {
        if (!_airCellDatas.TryGetValue(coord, out AirCellData airCellData))
            return;
        
        airCellData.Velocity += direction;
        _airCellDatas[coord] = airCellData;
    }

    private void GenerateAir(Vector2Int coordinates)
    {
        AirCellData airCellData;
        airCellData.Velocity = Vector3.forward;
        _airCellDatas.TryAdd(coordinates, airCellData);
    }
}
