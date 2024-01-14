using System.Collections.Generic;
using UnityEngine;

namespace NeuralNetworks
{
  [System.Serializable]
  public class CustomNeuralNetworkRunnerSaveData
  {
    public int[] CallQueue;
  }

  public class CustomNeuralNetworkRunner
  {
    public MutationNeuralNetwork Network
    {
      get { return network; }
      set 
      {
        network = value;
        ClearQueue();
      }
    }
    public IReadOnlyCollection<int> CallQueue
    {
      get { return callQueue; }
    }

    private MutationNeuralNetwork network = new MutationNeuralNetwork();

    private HashSet<int> callQueueHashSet = new HashSet<int>();
    private List<int> callQueue = new List<int>();

    private List<int> currentCallQueue = new List<int>();

    public CustomNeuralNetworkRunner(MutationNeuralNetwork network)
    {
      this.network = network;
    }

    public void Step()
    {
      currentCallQueue.Clear();
      foreach (var item in callQueue)
      {
        currentCallQueue.Add(item);
      }
      ClearQueue();

      foreach (var neuronId in currentCallQueue)
      {
        CalculateNeuronValue(neuronId);
        AddNeuronConnectionsToQueue(neuronId);
      }
    }

    public bool ContainsNeuronInQueue(int neuronId)
    {
      return callQueueHashSet.Contains(neuronId);
    }
    public void AddNeuronConnectionsToQueue(int neuronId)
    {
      foreach (var toNeuronId in network.GetConnectedNeurons(neuronId))
      {
        AddNeuronToQueue(toNeuronId);
      }
    }
    public bool AddNeuronToQueue(int neuronId)
    {
      if (callQueueHashSet.Contains(neuronId))
        return false;

      callQueueHashSet.Add(neuronId);
      callQueue.Add(neuronId);
      return true;
    }
    public void ClearQueue()
    {
      callQueueHashSet.Clear();
      callQueue.Clear();
    }

    private void CalculateNeuronValue(int neuronId)
    {
      float value = 0;
      foreach (var fromNeuronId in network.GetConnectedInverseNeurons(neuronId))
      {
        NeuronData fromNeuronData = network.GetNeuron(fromNeuronId);
        value += fromNeuronData.Value * network.GetConnection(fromNeuronId, neuronId).Weight;
      }

      NeuronData neuronData = network.GetNeuron(neuronId);
      neuronData.SetValueUsingActivator(value);
    }


    #region SaveLoad
    public CustomNeuralNetworkRunnerSaveData Save()
    {
      return new CustomNeuralNetworkRunnerSaveData
      {
        CallQueue = callQueue.ToArray(),
      };
    }

    public void Load(CustomNeuralNetworkRunnerSaveData saveData)
    {
      ClearQueue();
      foreach (var id in saveData.CallQueue)
      {
        AddNeuronToQueue(id);
      }
    }
    #endregion
  }
}
