using Photon.Pun;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(PhotonView))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private Animator m_Animator;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float rotationSpeed = 720f;
    [SerializeField] private PhotonView photonView;
    [SerializeField] private SkinnedMeshRenderer m_SickMan;
    private Material[] lineMaterials;
    private LineRenderer lineRenderer;
    private List<Vector3> path;
    private int currentIndex;
    private bool currentState = false;
    void Start()
    {
        lineMaterials = Resources.LoadAll<Material>("LineMaterials");
        Debug.Log("count Materia;: " + lineMaterials.Length);
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        if (lineMaterials != null && lineMaterials.Length > 0)
        {
            lineRenderer.material = m_SickMan.material = lineMaterials[UnityEngine.Random.Range(0, lineMaterials.Length)];
        }
        Color startCol = UnityEngine.Random.ColorHSV(0f, 1f, 0.7f, 1f, 0.7f, 1f);
        Color endCol = UnityEngine.Random.ColorHSV(0f, 1f, 0.7f, 1f, 0.7f, 1f);
        lineRenderer.startColor = startCol;
        lineRenderer.endColor = endCol;
        path = new List<Vector3>();
    }
    void Update()
    {
        // chỉ xử lý input và pathfinding cho player của mình
        if (photonView.IsMine)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    MazeCell mazeCell = hit.collider.GetComponent<MazeCell>();
                    if (mazeCell != null)
                    {
                        Vector2Int start = WorldToMaze(transform.position);
                        Vector2Int target = mazeCell.CellPos;
                        Pathfinding pf = new Pathfinding(MazeGenerator.Instance.CellGrid);
                        path = pf.FindPath(start, target);

                        if (path.Count > 0)
                            path.Insert(0, transform.position);

                        DrawPath(path);
                        currentIndex = 1;
                    }
                }
            }
        }

        // di chuyển (chỉ đối với player local, còn remote đã sync transform qua PhotonTransformView)
        if (photonView.IsMine && path != null && currentIndex < path.Count)
        {
            Vector3 targetPos = path[currentIndex];
            targetPos.y = transform.position.y;
            Vector3 direction = (targetPos - transform.position).normalized;
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
            ChangeAnimationState(true);
            if (Vector3.Distance(transform.position, targetPos) < 0.05f)
                currentIndex++;
        }
        else if (photonView.IsMine && path != null && currentIndex >= path.Count)
        {
            ChangeAnimationState(false);
            path = null;
        }
    }

    void ChangeAnimationState(bool newState)
    {
        if (currentState == newState) return;
        m_Animator.SetBool("Walk", newState);
        currentState = newState;
    }

    void DrawPath(List<Vector3> pathPoints)
    {
        if (pathPoints == null || pathPoints.Count == 0) { lineRenderer.positionCount = 0; return; }

        float y = transform.position.y + 0.05f;
        var arr = new Vector3[pathPoints.Count];
        for (int i = 0; i < pathPoints.Count; i++)
            arr[i] = new Vector3(pathPoints[i].x, y, pathPoints[i].z);

        lineRenderer.positionCount = arr.Length;
        lineRenderer.SetPositions(arr);
    }

    Vector2Int WorldToMaze(Vector3 pos)
    {
        var grid = MazeGenerator.Instance.CellGrid;
        int h = grid.Count;
        int w = grid[0].Count;

        float best = float.MaxValue;
        Vector2Int bestIdx = Vector2Int.zero;
        for (int y = 0; y < h; y++)
            for (int x = 0; x < w; x++)
            {
                Vector3 p = grid[y][x].position;
                float dx = pos.x - p.x;
                float dz = pos.z - p.z;
                float d2 = dx * dx + dz * dz;
                if (d2 < best)
                {
                    best = d2;
                    bestIdx = new Vector2Int(x, y);
                }
            }

        return bestIdx;
    }

    bool IsPlaying(string stateName)
    {
        return m_Animator.GetCurrentAnimatorStateInfo(0).IsName(stateName);
    }
}
