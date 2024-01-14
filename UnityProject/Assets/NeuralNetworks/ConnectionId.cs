using System;

namespace NeuralNetworks
{
  public struct ConnectionId
  {      
    public readonly static ConnectionId Empty = new ConnectionId(Guid.Empty);

    public readonly Guid Id;

    private ConnectionId(Guid id)
    {
      Id = id;
    }

    public static ConnectionId NewId()
    {
      return new ConnectionId(Guid.NewGuid());
    }
  }
}
