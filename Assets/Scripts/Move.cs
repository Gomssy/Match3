using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Move : MonoBehaviour
{
    public float speed;
    public float stopDistance = 0.1f;
    public bool isMoving = false;
    public Vector3 dest;

    private List<Vector3> nextMoves = new List<Vector3>();
    private List<Action> actions = new List<Action>();

    public void MovePiece(Vector3 _destination, Action action = null)
    {
        dest = _destination;
        if (action != null)
        {
            actions.Add(action);
        }
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
        actions.Clear();
    }

    private void Update()
    {
        if (!isMoving) return;

        transform.position = Vector3.MoveTowards(transform.position, dest, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, dest) < stopDistance)
        {
            if (nextMoves.Count == 0)
            {
                transform.position = dest;
                foreach (var action in actions)
                {
                    action?.Invoke();
                }
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