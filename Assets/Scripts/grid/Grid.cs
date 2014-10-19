using UnityEngine;
using System.Collections;

public class Grid : MonoBehaviour {

	public TouchControls touchControls;
	public TouchLayer touchLayer;

	//public Tile[,] tiles;

	public DungeonGenerator dungeon;


	void Start () {
		dungeon = GameObject.Find("Dungeon").GetComponent<DungeonGenerator>();
		initTouchControls();
	}

	
	void Update () {
	}


	// *****************************************************
	// Gestures
	// *****************************************************

	private void initTouchControls() {
		// register touch events
		touchControls = GameObject.Find("TouchControls").GetComponent<TouchControls>();

		touchLayer = touchControls.getLayer("grid");
		touchLayer.onPress += onTouchPress;
		touchLayer.onRelease += onTouchRelease;
		touchLayer.onMove += onTouchMove;
		touchLayer.onSwipe += onTouchSwipe;
	}


	/*public void onTouchRelease (TouchEvent e) {
		// get tile at touch
		Tile tile = getTileAtTouch(e);
		if (!tile) {
			print ("no tile found!");
			return;
		}

		// if tile is interactive
		if (tile.isInteractive()) {
			// set target challenge num and direction
			avatar.challengeTargetNum = tile.challengeNum;
			
			// get direction
			int dir = 0;
			if (tile.challengeNum > avatar.challengeNum) dir = 1;
			if (tile.challengeNum < avatar.challengeNum) dir = -1;
			if ((tile.challengeNum == avatar.challengeNum) && (avatar.goingToFight != 0)) dir = -avatar.dir;

			//print ("going to " + tile.type + " " + avatar.challengeTargetNum + " in direction " + dir);

			// move avatar to next waypoint
			if (dir != 0) avatar.moveToNextWaypoint(dir);
		}
	}*/


	/*private Tile getTileAtTouch (TouchEvent e) {
		//Vector3 pos = e.activeTouch.getPos3d(Camera.main);
		//pos = new Vector3(Mathf.Round(pos.x), pos.y, Mathf.Round(pos.z));

		Tile tile = null;

		// get touched transform
		Transform obj = e.activeTouch.getObject3d(Camera.main);

		if (obj) {
			// get touched tile
			for (var i = 1; i <= 3; i++) {
				tile = obj.GetComponent<Tile>();
				if (tile) break;
				obj = obj.parent;
			}
		}

		if (!tile) print ("no tile found!");
		return tile;
	}*/


	public void onTouchPress (TouchEvent e) {
		// Vector3 pos = e.activeTouch.getPos3d(Camera.main);
		Transform obj = e.activeTouch.getObject3d(Camera.main);

		Vector3 pos = obj.position + new Vector3(0, obj.lossyScale.y / 2, 0);

		print("press " + obj + " " + pos);
	}

	public void onTouchRelease (TouchEvent e) {
		//print("release " + e.activeTouch.getPos3d(Camera.main));
	}


	public void onTouchMove (TouchEvent e) {
		//print("move " + e.activeTouch.getPos3d(Camera.main));
	}


	public void onTouchSwipe (TouchEvent e) {
		//print ("swipe " + e.activeTouch.getVelocity3d(Camera.main));
	}
}
