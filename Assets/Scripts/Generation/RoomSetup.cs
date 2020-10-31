using System.Collections.Generic;
using UnityEngine;

public class RoomSetup {
	public enum RoomKind
	{
		Start = 1,
		Normal = 2,
		Boss = 3,
	}
	public Vector2 GridPos;
	public bool doorUp, doorDown, doorLeft, doorRight;
	public RoomKind Kind;
	public RoomSetup(Vector2 gridPos, RoomKind kind){
		GridPos = gridPos;
		Kind = kind;
	}
}
