using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {

	private float speed = 8.0f;
	private bool moving = false;
	
	private List<Vector2> path = new List<Vector2>();
	private Vector3 targetPos;
	

	public void init (Vector3 pos) {
		locateAtPos(pos);
	}
	

	void Update () {
		if (moving) {
			move();
			snapToGround();
		}		
	}


	public void locateAtPos (Vector3 pos) {
		transform.position = pos;
		targetPos = pos;

		Invoke("snapToGround", 0);
	}


	public void moveTo (Vector3 pos) {
		// get path from player to pos
		path = Grid.astar.SearchPath(
			(int)transform.position.x, (int)transform.position.z, 
			(int)pos.x, (int)pos.z
		);

		// if no path, escape
		if (path.Count == 0) {
			print ("No path available");
			return;
		}

		// start moving
    	moveStep();
	}


	public void moveStep () {
    	// remove current path node
    	path.RemoveAt(0);
    	
    	// if there is no path left, end moving
		if (path.Count == 0) {
			moving = false;
    		return;
    	}

		// move to position at current path node
		targetPos = new Vector3 (path[0].x, 0, path[0].y);
		moving = true;
	}


	public void move () {
		// Move our position a step closer to the target.
		transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);

		// check for arrival to target position
		if (targetPos.x - transform.position.x == 0 && targetPos.z - transform.position.z == 0) {
			moveStep();
		}
	}


	private void snapToGround () {
		RaycastHit hit;
		Vector3 pos = new Vector3(transform.position.x, transform.position.y + 5, transform.position.z);
		if (Physics.Raycast(pos, -Vector3.up * 100, out hit)) {
			if (hit.transform != transform) {
				transform.position = new Vector3(
					transform.position.x,
					hit.point.y, // + 0.01f,
					transform.position.z
				);
				//if (shadow) { shadow.offsetY = transform.position.y; }
			}
		}
	}
}
