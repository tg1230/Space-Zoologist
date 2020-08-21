﻿using UnityEngine;
using System.Collections.Generic;

// TODO refactor MoveInDirection
/// <summary>
/// Takes in a path (List<Vector3>) and moves the attached gameobject through it or moves in a specified direction.
/// </summary>
public class MovementController : MonoBehaviour
{
    public bool DestinationReached { get; private set; }
    public bool HasPath = false;
    public bool IsPaused = false;

    private Animal Animal { get; set; }
    private List<Vector3> PathToDestination { get; set; }
    private int PathIndex = 0;
    private Vector3 NextPathTile { get; set; }
    // Animal doesn't change direction until they've moved a certain distance in that direction
    private float ChangeDirectionThreshold = 0.5f;
    private float ChangeDirectionMovement = 0f;

    public void Start()
    {
        this.Animal = this.gameObject.GetComponent<Animal>();
        this.DestinationReached = true;
    }

    /// <summary>
    /// Called before update to assign a path.
    /// </summary>
    /// <param name="pathToDestination"></param>
    public void AssignPath(List<Vector3> pathToDestination, bool pathFound)
    {
        this.HasPath = pathFound;
        if (!pathFound)
        {
            Debug.Log("Error path not found");
            return;
        }
        this.PathToDestination = pathToDestination;
        this.NextPathTile = new Vector3(this.PathToDestination[0].x + 0.5f, this.PathToDestination[0].y + 0.5f, 0);
        this.DestinationReached = false;
        this.PathIndex = 0;
        this.UpdateVisualLogic(this.NextPathTile);
    }

    /// <summary>
    /// Called in update to move towards destination. Returns true when destination reached.
    /// </summary>
    /// <returns></returns>
    public void MoveTowardsDestination()
    {
        if (IsPaused)
        {
            this.Animal.MovementData.MovementStatus = Movement.idle;
            return;
        }
        if (this.PathToDestination.Count == 0)
        {
            this.PathIndex = 0;
            this.DestinationReached = true;
            return;
        }
        if (this.NextPathVectorReached(this.NextPathTile, this.transform.position))
        {
            this.PathIndex++;
            // Destination reached
            if (this.PathIndex == this.PathToDestination.Count)
            {
                this.PathIndex = 0;
                this.DestinationReached = true;
                return;
            }
            // Update to the next path tile and visual logic stuff
            else
            {
                // Need to translate back from grid to world
                this.NextPathTile = new Vector3(this.PathToDestination[this.PathIndex].x + 0.5f, this.PathToDestination[this.PathIndex].y + 0.5f, 0);
                // Debug.Log("("+this.NextPathTile.x+"),"+"("+this.NextPathTile.y+")");
                this.UpdateVisualLogic(this.NextPathTile);
            }
        }
        this.transform.position = this.MoveTowardsTile(this.transform.position, this.NextPathTile, this.Animal.MovementData.Speed);
    }

    public void MoveInDirection(Direction direction)
    {
        if (IsPaused)
        {
            this.Animal.MovementData.MovementStatus = Movement.idle;
            return;
        }
        Vector3 vectorDirection = new Vector3(0, 0, 0);
        float speed = this.Animal.MovementData.Speed * Time.deltaTime;
        switch(direction)
        {
            case Direction.up:
            {
                vectorDirection = new Vector3(this.transform.position.x, this.transform.position.y + speed , 0);
                break;
            }
            case Direction.down:
            {
                vectorDirection = new Vector3(this.transform.position.x, this.transform.position.y + -speed, 0);
                break;
            }
            case Direction.left:
            {
                vectorDirection = new Vector3(this.transform.position.x + -speed, this.transform.position.y, 0);
                break;
            }
            case Direction.right:
            {
                vectorDirection = new Vector3(this.transform.position.x + speed, this.transform.position.y, 0);
                break;
            }
            case Direction.upRight:
            {
                vectorDirection = new Vector3(this.transform.position.x + speed, this.transform.position.y + speed, 0);
                break;
            }
            case Direction.upLeft:
            {
                vectorDirection = new Vector3(this.transform.position.x + -speed,this.transform.position.y + speed, 0);
                break;
            }
            case Direction.downRight:
            {
                vectorDirection = new Vector3(this.transform.position.x + speed,this.transform.position.y + -speed, 0);
                break;
            }
            case Direction.downLeft:
            {
                vectorDirection = new Vector3(this.transform.position.x + -speed,this.transform.position.y + -speed, 0);
                break;
            }
        }
        if (this.ChangeDirectionMovement < this.ChangeDirectionThreshold)
        {
            this.ChangeDirectionMovement += Vector3.Distance(this.transform.position, vectorDirection);
        }
        else
        {
            this.UpdateVisualLogic(vectorDirection);
            this.ChangeDirectionMovement = 0f;
        }
        this.transform.position = vectorDirection;
    }

    public void StandStill()
    {
        this.Animal.MovementData.MovementStatus = Movement.idle;
        this.Animal.MovementData.CurrentDirection = Direction.down;
    }

    // Can modify pointReachedOffset to have more precise movement towards each destination point
    private bool NextPathVectorReached(Vector3 destination, Vector3 currentLocation)
    {
        float pointReachedOffset = 0.5f;
        return currentLocation.x < destination.x + pointReachedOffset && currentLocation.x > destination.x - pointReachedOffset &&
        currentLocation.y < destination.y + pointReachedOffset && currentLocation.y > destination.y - pointReachedOffset;
    }

    // Can be modified for different movements potentially
    private Vector3 MoveTowardsTile(Vector3 currentPosition, Vector3 pathTile, float movementSpeed)
    {
        float step = movementSpeed * Time.deltaTime;
        return Vector3.MoveTowards(currentPosition, pathTile, step);
    }

    public void UpdateVisualLogic(Vector3 destination)
    {
        this.HandleDirectionChange(this.transform.position, destination);
        if (this.Animal.MovementData.Speed > this.Animal.MovementData.RunThreshold)
        {
            this.Animal.MovementData.MovementStatus = Movement.running;
        }
        else
        {
            this.Animal.MovementData.MovementStatus = Movement.walking;
        }
    }

    // Can be modified for different angles of direction change
    private void HandleDirectionChange(Vector3 currentPosition, Vector3 nextTile)
    {
        Vector3 direction = (nextTile - currentPosition).normalized;
        int angle = (int)Vector3.Angle(Vector3.up, direction);
        // Moving left. Subtracting 360 and making the angle positive makes it easy to determine what the angle of direction is
        if (direction.x <= 0)
        {
            angle -= 360;
            if (angle < 0)
            {
                angle *= -1;
            }
            if (angle > 310)
            {
                this.Animal.MovementData.CurrentDirection = Direction.up;
            }
            else if (angle < 230)
            {
                this.Animal.MovementData.CurrentDirection = Direction.down;
            }
            else
            {
                this.Animal.MovementData.CurrentDirection = Direction.right;
            }
        }
        else if (direction.x > 0)
        {
            if (angle < 50)
            {
                this.Animal.MovementData.CurrentDirection = Direction.up;
            }
            else if (angle > 130)
            {
                this.Animal.MovementData.CurrentDirection = Direction.down;
            }
            else
            {
                this.Animal.MovementData.CurrentDirection = Direction.left;
            }
        }
    }

}