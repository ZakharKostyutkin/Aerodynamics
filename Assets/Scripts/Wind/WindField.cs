using UnityEngine;

public class WindField : MonoBehaviour
{
    [SerializeField]
    private WindCell _windCell;

    [SerializeField] private Vector3 _firstHexPosition;
    [SerializeField] private float _radius = 2;
    
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

        for (int i = 0; i < 6; i++)
        {
            GameObject hexGO1 = Instantiate(_windCell.gameObject, _firstHexPosition, Quaternion.identity);
            WindCell hex1 = hexGO1.GetComponent<WindCell>();
            hex1.SetRadius(_radius);
            
            CalculateHexPosition(hexGO1.transform,hexGO.transform,NeighborDirections[i].x,  NeighborDirections[i].y);
        }
    }

    private void CalculateHexPosition(Transform hex, Transform parentHex, float q, float r)
    {
        float positionX = Mathf.Sqrt(3) * _radius * (q + (r / 2f)) + parentHex.position.x;
        float positionZ = 3f / 2f * _radius * r + parentHex.position.z;
        
        hex.transform.position = new Vector3(positionX, parentHex.position.y, positionZ);
    }
}
