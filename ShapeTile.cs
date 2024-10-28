using System.Collections;
using System.Collections.Generic; // Queue<> 자료형을 사용하기 위한 네임스페이스 추가
using UnityEngine;

public class ShapeTile : Tile
{
    private Queue<Vector2> pathQueue = new Queue<Vector2>();

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && pathQueue.Count > 0)
        {
            MoveAlongPath(); // 스페이스바 입력으로 이동 시작
        }
    }

    public void SetPath(Queue<Vector2> path)
    {
        pathQueue = path;
    }

    public void MoveAlongPath()
    {
        if (pathQueue.Count > 0)
        {
            StartCoroutine(MoveCoroutine());
        }
    }

    private IEnumerator MoveCoroutine()
    {
        while (pathQueue.Count > 0)
        {
            Vector2 nextPos = pathQueue.Dequeue();
            Vector3 targetPosition = new Vector3(nextPos.x, nextPos.y, transform.position.z);

            Debug.Log($"Moving towards: ({nextPos.x}, {nextPos.y})");

            while (Vector3.Distance(transform.position, targetPosition) > 0.05f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * 2);
                yield return null;
            }

            transform.position = targetPosition;
            yield return new WaitForSeconds(0.1f);
        }

        Debug.Log("Tile has reached the destination.");
    }
}
