using Unity.Netcode;

namespace GamePlay.Enemy.State
{
  public struct EnemyState : INetworkSerializable
  {
    public float Hp;

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
      serializer.SerializeValue(ref Hp);
    }
  }
}