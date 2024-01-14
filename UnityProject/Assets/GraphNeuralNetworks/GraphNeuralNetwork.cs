using System;
using System.Collections.Generic;

namespace GraphNeuralNetworks
{
  public class GraphNeuralNetwork
  {
    public int NeuronsCount => neurons.Count;

    protected Dictionary<int, Neuron> neurons;
    protected Dictionary<int, Connection> connections;

    #region Constructors

    public GraphNeuralNetwork()
    {
      neurons = new Dictionary<int, Neuron>();
      connections = new Dictionary<int, Connection>();
    }

    public GraphNeuralNetwork(int neuronsCapacity, int connectionsCapacity)
    {
      neurons = new Dictionary<int, Neuron>(neuronsCapacity);
      connections = new Dictionary<int, Connection>(connectionsCapacity);
    }

    #endregion

    #region Neurons

    public bool ContainsNeuron(int neuronId)
    {
      return neurons.ContainsKey(neuronId);
    }

    #region Add
    public bool AddNeuron(Neuron neuron)
    {
      ValidateNewNeuron(neuron);

      if (neurons.ContainsKey(neuron.id))
        return false;

      protected_AddNeuron(neuron);
      return true;
    }
    protected void protected_AddNeuron(Neuron neuron)
    {
      neuron.parent = this;
      neurons.Add(neuron.id, neuron);
    }
    #endregion

    #region Remove
    public bool RemoveNeuron(int neuronId)
    {
      if (!neurons.ContainsKey(neuronId))
        return false;

      protected_RemoveNeuron(neurons[neuronId]);
      return true;
    }

    public void RemoveNeuron(Neuron neuron)
    {
      ValidateNeuron(neuron);
      protected_RemoveNeuron(neuron);
    }
    protected void protected_RemoveNeuron(Neuron neuron)
    {
      neurons.Remove(neuron.id);
      neuron.Invalidate();
    }
    #endregion

    #region Get
    public Neuron GetNeuron(int neuronId)
    {
      return neurons[neuronId];
    }

    public IEnumerable<Neuron> GetNeurons()
    {
      return neurons.Values;
    }
    #endregion

    internal void ValidateNewNeuron(Neuron neuronId)
    {
      if (neuronId == null)
      {
        throw new ArgumentNullException("Neuron");
      }

      if (neuronId.parent != null)
      {
        throw new InvalidOperationException("Neuron belongs to another network");
      }
    }
    internal void ValidateNeuron(Neuron neuronId)
    {
      if (neuronId == null)
      {
        throw new ArgumentNullException("Neuron");
      }

      if (neuronId.parent != this)
      {
        throw new InvalidOperationException("Neuron belongs to another network");
      }
    }

    #endregion

    #region Connections
    protected static int CreateConnectionId(int fromNeuronId, int toNeuronId)
    {
      return (fromNeuronId * 0x1f1f1f1f) ^ toNeuronId;
    }

    public bool AddConnection(Neuron fromNeuron, Neuron toNeuron, Connection connection)
    {
      ValidateNeuron(fromNeuron);
      ValidateNeuron(toNeuron);
      ValidateNewConnection(connection);

      if (fromNeuron.outgoingConnections.ContainsKey(toNeuron))
        return false;

      connection.parent = this;
      connection.id = CreateConnectionId(fromNeuron.id, toNeuron.id);

      connections.Add(connection.id, connection);

      fromNeuron.outgoingConnections.Add(toNeuron, connection);
      toNeuron.incomingConnections.Add(fromNeuron, connection);

      return true;
    }
    public Connection RemoveConnection(Neuron fromNeuron, Neuron toNeuron)
    {
      ValidateNeuron(fromNeuron);
      ValidateNeuron(toNeuron);

      if (fromNeuron.outgoingConnections.ContainsKey(toNeuron))
        return null;

      fromNeuron.outgoingConnections.Remove(toNeuron, out Connection connection);
      toNeuron.incomingConnections.Remove(fromNeuron);

      connections.Remove(connection.id);

      connection.Invalidate();

      return connection;
    }

    public Connection GetConnection(int connectionId)
    {
      return connections[connectionId];
    }
    public IEnumerable<Connection> GetConnections()
    {
      return connections.Values;
    }

    internal void ValidateNewConnection(Connection connection)
    {
      if (connection == null)
      {
        throw new ArgumentNullException("Connection");
      }

      if (connection.parent != null)
      {
        throw new InvalidOperationException("Connection belongs to another network");
      }
    }

    #endregion
  }
}
