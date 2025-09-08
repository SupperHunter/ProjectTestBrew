using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPoints : MonoBehaviour
{
    protected List<Vector3> ObjectVertices;       //Global vertex points on the object mesh
    protected List<Vector3> ObjectUniqueVertices; //Global distinct vertex points on the object mesh
    protected List<Vector3> ObjectLocalVertices;  //Local vertex points on the object mesh
    protected List<Vector3> CornerPoints;
    protected List<Vector3> EdgeVectors;
    protected Vector3 RandomPoint;

    void Start()
    {
        ObjectVertices = new List<Vector3>();
        ObjectUniqueVertices = new List<Vector3>();
        ObjectLocalVertices = new List<Vector3>();
        CornerPoints = new List<Vector3>();
        EdgeVectors = new List<Vector3>();
        CalculateObjectVertices();
    }

    public List<Vector3> GetCornerPoints()
    {
        CalculateCornerPoints();
        return CornerPoints;
    }

    public List<Vector3> GetObjectGlobalVertices()
    {
        CalculateObjectVertices();
        return ObjectVertices;
    }

    public List<Vector3> GetObjectLocalVertices()
    {
        CalculateObjectVertices();
        return ObjectLocalVertices;
    }

    public List<Vector3> GetObjectUniqueVertices()
    {
        CalculateObjectVertices();
        return ObjectUniqueVertices;
    }

    public Vector3 GetRandomPoint() 
    {
        CalculateRandomPoint();
        return RandomPoint;
    }

    private void CalculateObjectVertices() 
    {
        ObjectLocalVertices.Clear();
        ObjectVertices.Clear();
        ObjectUniqueVertices.Clear();
        ObjectLocalVertices = new List<Vector3>(GetComponent<MeshFilter>().sharedMesh.vertices);
        foreach (Vector3 point in ObjectLocalVertices)
        {
            ObjectVertices.Add(transform.TransformPoint(point));
        }
        ObjectUniqueVertices = ObjectVertices.Distinct().ToList();
    }

    protected virtual void CalculateEdgeVectors(int VectorCornerIdx)
    {
        CalculateCornerPoints();
        EdgeVectors.Clear();
    }

    protected virtual void CalculateEdgeVectors(int VectorCornerIdx, int vectorfaceidx)
    {
        CalculateCornerPoints();
        EdgeVectors.Clear();
    }

    protected virtual void CalculateRandomPoint() { }

    protected virtual void CalculateCornerPoints()
    {
        CalculateObjectVertices();
        CornerPoints.Clear();
        
    }

}
