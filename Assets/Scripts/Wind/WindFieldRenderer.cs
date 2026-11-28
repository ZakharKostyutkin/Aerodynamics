using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class WindFieldRenderer : MonoBehaviour
{
    [SerializeField] private float _height = 2;
    [SerializeField] private float _radius = 2;
    [SerializeField] private float _airArrowHeadScale = 1;
    
    [SerializeField] private MeshFilter _meshFilter;
    
    [Header("Debugging")]
    [SerializeField] private bool _debugHex = false;
    [SerializeField] private bool _debugWindArrow = false;
    
    private Vector3 _fieldOrigin;
    private Dictionary<Vector2Int, AirCellData> _airCellDatas;
    private Mesh _mesh;

    public void InitializeMesh()
    {
        _mesh = new Mesh();
        _mesh.name = "WindField Mesh";
        _mesh.indexFormat = IndexFormat.UInt32;

        BuildMesh();
    }

    private void OnDrawGizmos()
    {
        if (!_debugHex && !_debugWindArrow)
            return;
        
        if (_airCellDatas == null)
            return;
        
        foreach (var cell in _airCellDatas)
        {
            Vector2Int coord = cell.Key; 
            AirCellData airCellData = cell.Value; 
            
            Vector3 direction = airCellData.Velocity; 
            
            float positionX = Mathf.Sqrt(3) * _radius * (coord.x + (coord.y / 2f));
            float positionZ = 3f / 2f * _radius * coord.y;
            
            Vector3 position = new Vector3(positionX, 0, positionZ) + _fieldOrigin; 
            Vector3 endPosition = position + direction;
            
            
            DrawVelocity(position, endPosition, direction);
            
            
            DrawHex(position);
        }
    }

    private void DrawVelocity(Vector3 position, Vector3 endPosition, Vector3 direction)
    {
        if (!_debugWindArrow)
            return;
        
        if (direction.sqrMagnitude < 0.0001f)
            return;
        
        Gizmos.color = Color.blue;
            
        Gizmos.DrawLine(position, endPosition);

        Vector3 backward = -direction.normalized;
            
        float length = direction.magnitude * _airArrowHeadScale;
            
        Vector3 arrowRight = Quaternion.AngleAxis(33, Vector3.up) * backward;
        Vector3 arrowLeft = Quaternion.AngleAxis(-33f, Vector3.up) * backward;

        Vector3 right = endPosition + arrowRight * length;
        Vector3 left = endPosition + arrowLeft * length;

        Gizmos.DrawLine(endPosition, right);
        Gizmos.DrawLine(endPosition, left);
    }

    private void DrawHex(Vector3 center)
    {
        if (!_debugHex)
            return;
        
        for (int i = 0; i < 6; i++)
        {
            
            float angle = 60 * i - 30;
            float radians = angle * Mathf.Deg2Rad;
            
            Vector3 posTop = center + new Vector3(
                _radius * Mathf.Cos(radians),
                _height / 2,
                _radius * Mathf.Sin(radians));
            
            Vector3 posBot = center + new Vector3(
                _radius * Mathf.Cos(radians),
                -_height / 2,
                _radius * Mathf.Sin(radians));
            
            Vector3 newPosTop = center + new Vector3(
                _radius * Mathf.Cos((angle + 60) * Mathf.Deg2Rad),
                _height / 2,
                _radius * Mathf.Sin((angle + 60) * Mathf.Deg2Rad));
            
            Vector3 newPosBot = center + new Vector3(
                _radius * Mathf.Cos((angle + 60) * Mathf.Deg2Rad),
                -_height / 2,
                _radius * Mathf.Sin((angle + 60) * Mathf.Deg2Rad));
            
            Gizmos.color = Color.red;
            
            Gizmos.DrawLine(posTop, posBot);
            Gizmos.DrawLine(posTop, newPosTop);
            Gizmos.DrawLine(posBot, newPosBot);
        }
    }

    public void DrawAirField(
        Dictionary<Vector2Int, AirCellData> airCellDatas)
    {
        _airCellDatas = airCellDatas;
    }

    public void SetRadius(float radius)
    {
        _radius = radius;
    }

    public void SetFieldOrigin(Vector3 origin)
    {
        _fieldOrigin = origin;
    }

    private Vector3[] GetMeshVertices(Vector3 position)
    {
        position += _fieldOrigin;
        Vector3 localPosition = transform.InverseTransformPoint(position);
        Vector3[] vertices = new Vector3[6];
        for (int i = 0; i < 6; i++)
        {

            float angle = 60 * i - 30;
            float radians = angle * Mathf.Deg2Rad;

            Vector3 vertex = localPosition + new Vector3(
                _radius * Mathf.Cos(radians),
                _height / 2,
                _radius * Mathf.Sin(radians));
            
            vertices[i] = vertex;
        }
        return vertices;
    }
    
    private int[] GetMeshTriangles()
    {
        int[] triangles = new int[12];
        for (int i = 0; i < 4; i++)
        {
            triangles[i * 3] = 0;
            triangles[i * 3 + 1] = i + 1;
            triangles[i * 3 + 2] = i + 2;
        }
        return triangles;
    }

    private void BuildMesh()
    {
        _mesh.SetVertices(GetFieldVertices());
        _mesh.SetTriangles(GetFieldTriangles(), 0);
        
        _mesh.RecalculateNormals();
        _meshFilter.mesh = _mesh;
    }

    private List<Vector3> GetFieldVertices()
    {
        List<Vector3> vertices = new List<Vector3>();

        foreach (var cell in _airCellDatas)
        {
            Vector2Int coord = cell.Key; 
            
            float positionX = Mathf.Sqrt(3) * _radius * (coord.x + (coord.y / 2f));
            float positionZ = 3f / 2f * _radius * coord.y;
        
            Vector3 pos = new Vector3(positionX, 0, positionZ);
            
            Vector3[] cellVert = GetMeshVertices(pos);
            vertices.AddRange(cellVert);
        }
        return vertices;
    }

    private List<int> GetFieldTriangles()
    {
        List<int> triangles = new List<int>(_airCellDatas.Count * 12);
        int vertexIndex = 0;
        foreach (var cell in _airCellDatas)
        {
            for (int i = 0; i < 4; i++)
            {
                triangles.Add(vertexIndex);
                triangles.Add(i + 1 + vertexIndex);
                triangles.Add(i + 2 + vertexIndex);
            }

            vertexIndex += 6;
        }
        return triangles;
    }
}
