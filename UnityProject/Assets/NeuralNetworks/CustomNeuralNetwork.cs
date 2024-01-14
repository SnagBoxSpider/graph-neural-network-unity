using System;
using System.Collections.Generic;

namespace NeuralNetworks
{
  public class CustomNeuralNetwork
  {
    public int NeuronsCount
    {
      get { return neurons.Count; }
    }
    public int ConnectionsCount
    {
      get { return connections.Count; }
    }

    protected int neuronIdCounter = 1;
    protected int connectionIdCounter = 1;

    protected Dictionary<int, NeuronData> neurons;
    protected Dictionary<int, ConnectionData> connections;

    protected Dictionary<int, Dictionary<int, int>> neuronsConnections;
    protected Dictionary<int, Dictionary<int, int>> neuronsConnectionsInverse;

    #region Constructors

    public CustomNeuralNetwork() : this(0) { }

    public CustomNeuralNetwork(int neuronsCapacity)
    {
      neurons = new Dictionary<int, NeuronData>(neuronsCapacity);
      connections = new Dictionary<int, ConnectionData>();

      neuronsConnections = new Dictionary<int, Dictionary<int, int>>(neuronsCapacity);
      neuronsConnectionsInverse = new Dictionary<int, Dictionary<int, int>>(neuronsCapacity);
    }

    public CustomNeuralNetwork(CustomNeuralNetwork neuralNetwork)
    {
      neuronIdCounter = neuralNetwork.neuronIdCounter;
      connectionIdCounter = neuralNetwork.connectionIdCounter;

      neurons = new Dictionary<int, NeuronData>(neuralNetwork.neurons);
      foreach (var neuron in neuralNetwork.neurons)
      {
        neurons[neuron.Key] = neuron.Value.Clone();
      }
      connections = new Dictionary<int, ConnectionData>(neuralNetwork.connections);
      foreach (var connection in neuralNetwork.connections)
      {
        connections[connection.Key] = connection.Value.Clone();
      }

      neuronsConnections = new Dictionary<int, Dictionary<int, int>>(neuralNetwork.neuronsConnections.Count);
      foreach (var neuronConnections in neuralNetwork.neuronsConnections)
      {
        neuronsConnections[neuronConnections.Key] = new Dictionary<int, int>(neuronConnections.Value);
      }
      neuronsConnectionsInverse = new Dictionary<int, Dictionary<int, int>>(neuralNetwork.neuronsConnectionsInverse.Count);
      foreach (var neuronConnectionsInverse in neuralNetwork.neuronsConnectionsInverse)
      {
        neuronsConnectionsInverse[neuronConnectionsInverse.Key] = new Dictionary<int, int>(neuronConnectionsInverse.Value);
      }
    }

    public CustomNeuralNetwork Clone()
    {
      return new CustomNeuralNetwork(this);
    }

    #endregion

    #region Neurons

    public int AddNeuron()
    {
      return AddNeuron(new NeuronData());
    }

    public int AddNeuron(NeuronData data)
    {
      int neuronId = neuronIdCounter++;

      AddNeuron(neuronId, data);

      return neuronId;
    }

    private bool AddNeuron(int neuronId, NeuronData data)
    {
      if (neurons.ContainsKey(neuronId))
        return false;

      neurons.Add(neuronId, data);
      neuronsConnections.Add(neuronId, new Dictionary<int, int>());
      neuronsConnectionsInverse.Add(neuronId, new Dictionary<int, int>());
      return true;
    }

    public void SetNeuron(int neuronId)
    {
      SetNeuron(neuronId, default(NeuronData));
    }
    public void SetNeuron(int neuronId, NeuronData data)
    {
      neurons[neuronId] = data;
    }

    public bool RemoveNeuron(int neuronId)
    {
      ClearConnections(neuronId);
      ClearConnectionsInverse(neuronId);
      neuronsConnections.Remove(neuronId);
      neuronsConnectionsInverse.Remove(neuronId);
      return neurons.Remove(neuronId);
    }

    public NeuronData GetNeuron(int neuronId)
    {
      return neurons[neuronId];
    }

    public IEnumerable<int> GetNeuronIds()
    {
      return neurons.Keys;
    }
    public IEnumerable<KeyValuePair<int, NeuronData>> GetNeurons()
    {
      return neurons;
    }

    #endregion

    #region Connections

    public bool AddConnection(int fromNeuronId, int toNeuronId)
    {
      return AddConnection(fromNeuronId, toNeuronId, new ConnectionData());
    }
    public bool AddConnection(int fromNeuronId, int toNeuronId, ConnectionData data)
    {
      if (ContainsConnection(fromNeuronId, toNeuronId))
        return false;

      int id = connectionIdCounter++;

      connections[id] = data;
      neuronsConnections[fromNeuronId][toNeuronId] = id;
      neuronsConnectionsInverse[toNeuronId][fromNeuronId] = id;

      return true;
    }

    public bool SetConnection(int fromNeuronId, int toNeuronId)
    {
      return AddConnection(fromNeuronId, toNeuronId, new ConnectionData());
    }
    public void SetConnection(int fromNeuronId, int toNeuronId, ConnectionData data)
    {
      int id = GetConnectionId(fromNeuronId, toNeuronId);
      connections[id] = data;
    }
    public void SetConnection(int connectionId, ConnectionData data)
    {
      connections[connectionId] = data;
    }

    public bool RemoveConnection(int fromNeuronId, int toNeuronId)
    {
      if (!ContainsConnection(fromNeuronId, toNeuronId))
        return false;

      var connectionId = neuronsConnections[fromNeuronId][toNeuronId];
      return neuronsConnections[fromNeuronId].Remove(toNeuronId) &&
        neuronsConnectionsInverse[toNeuronId].Remove(fromNeuronId) && connections.Remove(connectionId);
    }

    public bool ClearConnections(int fromNeuronId)
    {
      if (!neuronsConnections.ContainsKey(fromNeuronId))
        return false;

      foreach (var connection in neuronsConnections[fromNeuronId])
      {
        neuronsConnectionsInverse[connection.Key].Remove(fromNeuronId);
        connections.Remove(connection.Value);
      }
      neuronsConnections.Clear();
      return true;
    }
    public bool ClearConnectionsInverse(int toNeuronId)
    {
      if (!neuronsConnectionsInverse.ContainsKey(toNeuronId))
        return false;

      foreach (var connection in neuronsConnectionsInverse[toNeuronId])
      {
        neuronsConnections[connection.Key].Remove(toNeuronId);
        connections.Remove(connection.Value);
      }
      neuronsConnectionsInverse.Clear();
      return true;
    }

    public bool ContainsConnection(int fromNeuronId, int toNeuronId)
    {
      if (!neuronsConnections.TryGetValue(fromNeuronId, out var neuronConnections))
        return false;

      return neuronConnections.ContainsKey(toNeuronId);
    }

    public int GetConnectionId(int fromNeuronId, int toNeuronId)
    {
      return neuronsConnections[fromNeuronId][toNeuronId];
    }

    public ConnectionData GetConnection(int fromNeuronId, int toNeuronId)
    {
      var id = GetConnectionId(fromNeuronId, toNeuronId);
      return connections[id];
    }
    public ConnectionData GetConnection(int connectionId)
    {
      return connections[connectionId];
    }
    public IEnumerable<int> GetAllConnectionIds()
    {
      return connections.Keys;
    }

    public IEnumerable<ConnectionData> GetConnections()
    {
      return connections.Values;
    }
    public IEnumerator<ConnectionData> GetConnections(int fromNeuronId)
    {
      foreach (var connectionId in neuronsConnections[fromNeuronId].Values)
      {
        yield return connections[connectionId];
      }
    }

    public IEnumerable<int> GetConnectedNeurons(int fromNeuronId)
    {
      return neuronsConnections[fromNeuronId].Keys;
    }
    public IEnumerable<int> GetConnectedInverseNeurons(int fromNeuronId)
    {
      return neuronsConnectionsInverse[fromNeuronId].Keys;
    }

    #endregion
  }
}
