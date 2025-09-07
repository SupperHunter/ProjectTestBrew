using System.Collections.Generic;
using UnityEngine;

public class PlayerMover : MonoBehaviour
{
    private float speed = 2f;
    private List<Cell> path;
    private int currentIndex = 0;

    public void SetPath(List<Cell> newPath)
    {
        path = newPath;
        currentIndex = 0;
    }

    void Update()
    {
        if (path == null || currentIndex >= path.Count) return;

        transform.position = Vector3.MoveTowards(transform.position, path[currentIndex].position, speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, path[currentIndex].position) < 0.01f)
        {
            currentIndex++;
        }
    }
}
