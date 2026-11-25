using System;
using UnityEngine;

public class WindCell : MonoBehaviour
{
    [SerializeField] private float _height = 2;
    [SerializeField] private float _radius = 2;

    private void OnDrawGizmos()
    {
        for (int i = 0; i < 6; i++)
        {
            
            float angle = 60 * i - 30;
            float radians = angle * Mathf.Deg2Rad;
            
            Vector3 posTop = new Vector3(
                _radius * Mathf.Cos(radians),
                _height / 2,
                _radius * Mathf.Sin(radians));
            
            Vector3 posBot = new Vector3(
                _radius * Mathf.Cos(radians),
                -_height / 2,
                _radius * Mathf.Sin(radians));
            
            Vector3 newPosTop = new Vector3(
                _radius * Mathf.Cos((angle + 60) * Mathf.Deg2Rad),
                _height / 2,
                _radius * Mathf.Sin((angle + 60) * Mathf.Deg2Rad));
            
            Vector3 newPosBot = new Vector3(
                _radius * Mathf.Cos((angle + 60) * Mathf.Deg2Rad),
                -_height / 2,
                _radius * Mathf.Sin((angle + 60) * Mathf.Deg2Rad));
            
            Vector3 worldTop = transform.TransformPoint(posTop);
            Vector3 worldBot = transform.TransformPoint(posBot);
            Vector3 newWorldTop = transform.TransformPoint(newPosTop);
            Vector3 newWorldBot = transform.TransformPoint(newPosBot);
            
            Gizmos.color = Color.red;
            
            Gizmos.DrawLine(worldTop, worldBot);
            Gizmos.DrawLine(worldTop, newWorldTop);
            Gizmos.DrawLine(worldBot, newWorldBot);
        }
    }

    public void SetRadius(float radius)
    {
        _radius = radius;
    }
}
