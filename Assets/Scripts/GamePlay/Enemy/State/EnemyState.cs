using Unity.Netcode;

namespace GamePlay.Enemy.State
{
  // todo use just float hp without this struct
  public struct EnemyState : INetworkSerializable
  {
    public float Hp;

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
      serializer.SerializeValue(ref Hp);
    }
  }
}