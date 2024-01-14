using System;

namespace NeuralNetworks
{
  public struct NeuronId
  {
    public readonly static NeuronId Empty = new NeuronId(Guid.Empty);

    public readonly Guid Id;

    private NeuronId(Guid id)
    {
      Id = id;
    }

    public static NeuronId NewId()
    {
      return new NeuronId(Guid.NewGuid());
    }
  }
}
