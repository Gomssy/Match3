using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Move : MonoBehaviour
{
    public float speed = 20f;
    public float stopDistance = 0.1f;
    public bool isMoving = false;
    public Vector3 dest;
    public GameObject target;

    private List<Vector3> nextMoves = new List<Vector3>();
    private event Action OnCompleted;


    public void MovePiece(Vector3 _destination, Action action = null)
    {
        dest = _destination;
        OnCompleted += action;
        isMoving = true;
    }

    public void MovePiece(List<Vector3> desList, Action action = null)
    {
        if (desList.Count == 0) return;
        MovePiece(desList[0], action);
        desList.RemoveAt(0);
        nextMoves = desList;
    }

    public void Stop()
    {
        isMoving = false;
        OnCompleted = null;
    }

    private void Update()
    {
        if (!isMoving) return;
        //목표지점까지 이동
        transform.position = Vector3.MoveTowards(transform.position, dest, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, dest) < stopDistance)
        {
            if (nextMoves.Count == 0)
            {
                transform.position = dest;
                OnCompleted?.Invoke();
                Stop();
                return;
            }
            else
            {
                dest = nextMoves[0];
                nextMoves.RemoveAt(0);
                return;
            }
        }
        
    }
}