using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class World : MonoBehaviour {

	public TouchControls touchControls;
	public TouchLayer touchLayer;

	public CameraOrbit cam;
	public DungeonGenerator dungeon;
	public Avatar player;
	public List<Avatar> monsters;


	void Start () {
		cam = Camera.main.GetComponent<CameraOrbit>();
		dungeon = GameObject.Find("Dungeon").GetComponent<DungeonGenerator>();
		initTouchControls();
	}


	void Update () {
		// SPACE -> Generate a new Test Dungeon
		if (Input.GetButtonDown("Jump")) {
			generateDungeon();
		}
	}


	private void generateDungeon () {
			// Generate a new Seed
			dungeon.seed = System.DateTime.Now.Millisecond*1000 + System.DateTime.Now.Minute*100;
			Random.seed = dungeon.seed;

			// Generate Dungeon
			Debug.Log ("Dungeon Generation Started");
			dungeon.GenerateDungeon(dungeon.seed);

			// initialize grid
			initGrid();

			// create player at the center of a random room
			Room room = dungeon.rooms[Random.Range(0, dungeon.rooms.Count - 1)];
			Vector3 pos = new Vector3(Mathf.Round(room.boundary.center.x), 0, Mathf.Round(room.boundary.center.y));
			player = createPlayer(pos);

			// create some monsters
			monsters = createMonsters(20);
	}


	private void initGrid () {
		// init grid cells for astar calculations
        Grid.InitEmpty(dungeon.MAP_WIDTH, dungeon.MAP_HEIGHT);
        print ("Initializing grid " + Grid.xsize + "," + Grid.ysize);

        // set walkability map
        for (int y = 0; y < Grid.ysize; y++) {
			for (int x = 0; x < Grid.xsize; x++) {
				// set rooms and corridor cells to walkable
				Tile tile = dungeon.tiles[x, y];
				if (tile.id == Tile.TILE_ROOM || tile.id == Tile.TILE_CORRIDOR) {
					Grid.setWalkable(y, x, true);
				} else {
					Grid.setWalkable(y, x, false);
				}
			}
		}
	}


	private Avatar createPlayer (Vector3 pos) {
		// if we already have a player, just locate it at starting position
		if (player) {
			player.locateAtPos(pos);
			return player;
		}

		Avatar newPlayer = createAvatar(pos, true);
		cam.target = newPlayer.transform;

		return newPlayer;
	}


	private List<Avatar> createMonsters (int max) {
		List<Avatar> monster = new List<Avatar>();

		for (var i = 0; i < max; i++) {
			Room room = dungeon.rooms[Random.Range(0, dungeon.rooms.Count - 1)];
			//Vector3 pos = new Vector3(Mathf.Round(room.boundary.center.x), 0, Mathf.Round(room.boundary.center.y));

			Vector3 pos = new Vector3(
				Mathf.Round(Random.Range(room.boundary.Left() + 1, room.boundary.Right() - 1)),
				0,
				Mathf.Round(Random.Range(room.boundary.Top() + 1, room.boundary.Bottom() - 1))
			);

			monsters.Add(createAvatar(pos, false));
		}

		return monsters;
	}


	private Avatar createAvatar (Vector3 pos, bool useTorch) {
		// create avatar
		GameObject obj = (GameObject)Instantiate(Resources.Load("avatar/Prefabs/Avatar"), Vector3.zero, Quaternion.identity);
		obj.transform.parent = transform;

		Avatar avatar = obj.GetComponent<Avatar>();
		avatar.init(this, pos, useTorch);

		return avatar;
	}

	// *****************************************************
	// Visibility
	// *****************************************************

	public void setVisibility (float posX, float posY, int radius) {
		bool[,] lit = new bool[Grid.xsize, Grid.ysize];

		ShadowCaster.ComputeFieldOfViewWithShadowCasting(
			(int)posX, (int)posY, radius,
			(x1, y1) => !Grid.getWalkable(x1, y1), //world.dungeon.tiles[x1, y1].id == Tile.TILE_WALL, //!Grid.getWalkable(x1, y1), // // !Grid.getWalkable(y1, x1), //map[x1, y1] == wallMap, // 
			(x2, y2) => { lit[x2, y2] = true; }
		);

		for (int y = 0; y < dungeon.MAP_HEIGHT; y++) {
			for (int x = 0; x < dungeon.MAP_WIDTH; x++) {
				Tile tile = dungeon.tiles[x, y];
				if (tile.obj) {
					if (lit[y, x]) {
						tile.obj.renderer.material.color = new Color(0.5f, 0.5f, 0.5f, 0.5f); //SetActive(lit[x, y]);
					} else {
						tile.obj.renderer.material.color = new Color(0.1f, 0.1f, 0.1f, 0.5f); //SetActive(lit[x, y]);
					}
				}
			}
		}
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


	public void onTouchPress (TouchEvent e) {
		// Vector3 pos = e.activeTouch.getPos3d(Camera.main);
		Transform obj = e.activeTouch.getObject3d(Camera.main);
		if (!obj) return;

		player.moveTo(new Vector3(obj.position.x, 0, obj.position.z));

		// get tile at current path node
		//Tile tile = dungeon.tiles[(int)path[0].y, (int)path[0].x];
		//Vector3 pos = tile.obj.transform.position + Vector3.up * tile.obj.transform.lossyScale.y / 2;
		//pos += Vector3.up * transform.lossyScale.y / 2;;
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
