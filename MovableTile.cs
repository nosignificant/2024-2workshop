using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableTile : Tile
{
    public bool isCollision = false;
    Vector2 direction;

    private void OnCollisionEnter(Collision collision)
    {
        isCollision = true;
        direction = collision.contacts[0].normal * -1;
    }

    void Update()
    {
        NormalizePos();
    }

    void NormalizePos()
    {
        Vector2 startPos = base.GetPos();
        Vector2 endPos = startPos;
        

        if (Mathf.Abs(direction.x) < 0.5f)
        {
            direction.x = 0;
        }
        else
        {
            direction.y = 0;
        }
        direction.Normalize();


        if (isCollision)
        {
            endPos = startPos + direction;
            this.transform.position = endPos;

            // 충돌 처리 이후 상태 초기화
            isCollision = false;
        }
    }

}
