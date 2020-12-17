using System.Collections.Generic;
using UnityEngine;

public class MazeRoomSetup {
	public enum RoomKind
	{
		Start = 1,
		Normal = 2,
		Boss = 3,
	}
	public Vector2 GridPos;
	public bool doorUp, doorDown, doorLeft, doorRight;
	public RoomKind Kind;
	public MazeRoomSetup(Vector2 gridPos, RoomKind kind){
		GridPos = gridPos;
		Kind = kind;
	}

	public override string ToString()
	{
		return $"RoomSetup doorUp:{doorUp}, doorDown:{doorDown}, doorLeft:{doorLeft}, doorRight:{doorRight}";
	}
}
