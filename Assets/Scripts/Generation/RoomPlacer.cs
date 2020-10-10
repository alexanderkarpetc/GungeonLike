﻿﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomPlacer : MonoBehaviour {
	[SerializeField]
	GameObject RoomObj;

	public Vector2 roomDimensions = new Vector2(1, 1);
	public Vector2 gutterSize = new Vector2(1, 1);

	public void Place(Room[,] rooms)
	{
		foreach (Room room in rooms)
		{
			if (room == null)
			{
				continue;
			}

			var pos = new Vector3(room.gridPos.x * (roomDimensions.x + gutterSize.x),  room.gridPos.y * (roomDimensions.y + gutterSize.y), 0);
			Instantiate(RoomObj, pos, Quaternion.identity);
		}
	}
}
