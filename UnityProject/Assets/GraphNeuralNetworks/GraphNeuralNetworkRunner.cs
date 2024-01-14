using System.Collections.Generic;

namespace GraphNeuralNetworks
{
  public class GraphNeuralNetworkRunner
  {
    public GraphNeuralNetwork Network
    {
      get { return network; }
      set
      {
        ClearQueue();
        network = value;
      }
    }

    public IReadOnlyCollection<Neuron> CallQueueNeurons => callQueueNeurons;

    private GraphNeuralNetwork network;

    private HashSet<Neuron> callQueueHasSetNeurons = new HashSet<Neuron>();
    private Queue<Neuron> callQueueNeurons = new Queue<Neuron>();

    private Queue<Neuron> currentCallQeueNeurons = new Queue<Neuron>();

    public GraphNeuralNetworkRunner(GraphNeuralNetwork network)
    {
      this.network = network;
    }

    #region Step
    public void Step()
    {
      var buffer = currentCallQeueNeurons;
      currentCallQeueNeurons = callQueueNeurons;
      callQueueNeurons = buffer;

      ClearQueue();

      foreach (var neuron in currentCallQeueNeurons)
      {
        CalculateNeuronValue(neuron);
        AddNeuronConnectionsToQueue(neuron);
      }
    }
    protected void CalculateNeuronValue(Neuron neuron)
    {
      float value = 0;
      foreach (var connection in neuron.IncomingConnections)
      {
        value += connection.Key.Value * connection.Value.Weight;
      }
      neuron.Value = neuron.ActivationFunction.Invoke(value);
    }
    #endregion

    #region Add
    public void AddNeuronConnectionsToQueue(Neuron neuron)
    {
      network.ValidateNeuron(neuron);
      foreach (var toNeuron in neuron.OutgoingConnections.Keys)
      {
        protected_AddNeuronToQueue(toNeuron);
      }
    }

    public bool AddNeuronToQueue(Neuron neuron)
    {
      network.ValidateNeuron(neuron);
      return protected_AddNeuronToQueue(neuron);
    }

    protected bool protected_AddNeuronToQueue(Neuron neuron)
    {
      if (callQueueHasSetNeurons.Add(neuron))
      {
        callQueueNeurons.Enqueue(neuron);
        return true;
      }

      return false;
    }
    #endregion

    public void ClearQueue()
    {
      callQueueHasSetNeurons.Clear();
      callQueueNeurons.Clear();
    }
  }
}
