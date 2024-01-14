using System;
using System.Collections.Generic;

namespace GraphNeuralNetworks
{
  public delegate float ActivationFunction(float value);

  public class Neuron : INeuronReadOnly
  {
    public readonly static ActivationFunction DefaultActivationFunction = (value) => (float)Math.Tanh(value);

    internal GraphNeuralNetwork parent = null;

    internal Dictionary<Neuron, Connection> incomingConnections = new Dictionary<Neuron, Connection>();
    internal Dictionary<Neuron, Connection> outgoingConnections = new Dictionary<Neuron, Connection>();

    internal int id;
    protected float value;
    protected ActivationFunction activationFunction;

    public GraphNeuralNetwork Parent { get => parent; }

    public IReadOnlyDictionary<Neuron, Connection> IncomingConnections { get => incomingConnections; }
    public IReadOnlyDictionary<Neuron, Connection> OutgoingConnections { get => outgoingConnections; }

    public int Id { get => id; }

    public float Value
    {
      get => value;
      set => this.value = value;
    }

    public ActivationFunction ActivationFunction
    {
      get => activationFunction;
      set => activationFunction = value;
    }

    public Neuron(int id) : this(id, 0, DefaultActivationFunction) { }
    public Neuron(int id, ActivationFunction activationFunction) : this(id, 0, activationFunction) { }
    public Neuron(int id, float value, ActivationFunction activationFunction)
    {
      this.id = id;
      this.value = value;
      this.activationFunction = activationFunction;
    }

    internal void Invalidate()
    {
      parent = null;

      foreach (var incomingNeuron in incomingConnections.Keys)
      {
        incomingNeuron.outgoingConnections.Remove(this, out Connection connection);
        connection.Invalidate();
      }
      incomingConnections.Clear();

      foreach (var incomingNeuron in outgoingConnections.Keys)
      {
        incomingNeuron.incomingConnections.Remove(this, out Connection connection);
      }
      outgoingConnections.Clear();
    }
  }
}
