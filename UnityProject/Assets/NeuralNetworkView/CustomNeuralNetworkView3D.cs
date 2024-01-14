using GraphNeuralNetworks;
using System.Collections.Generic;
using UnityEngine;

public class CustomNeuralNetworkView3D : CustomNeuralNetworkView
{
  public override GraphNeuralNetwork NeuralNetwork
  {
    get { return neuralNetwork; }
    set
    {
      ClearViews();
      neuralNetwork = value;
      CreateViews();
    }
  }

  [SerializeField]
  private NeuronView prefabNeuron;
  [SerializeField]
  private ConnectionView prefabConnection;

  private GraphNeuralNetwork neuralNetwork;

  private Dictionary<Neuron, NeuronView> neuronViews = new Dictionary<Neuron, NeuronView>();
  private Dictionary<Connection, ConnectionView> connectionViews = new Dictionary<Connection, ConnectionView>();

  private static Stack<NeuronView> neuronViewsPool = new Stack<NeuronView>();
  private static Stack<ConnectionView> connectionViewPool = new Stack<ConnectionView>();

  private void Update()
  {
    if (Camera.current == null)
      return;

    foreach (var neuronViewPair in neuronViews)
    {
      var neuron = neuronViewPair.Key;
      var view = neuronViewPair.Value;
      view.Value = neuron.Value;
      view.UpdateNameRotation();
    }

    foreach (var connectionViewPair in connectionViews)
    {
      var connection = connectionViewPair.Key;
      var view = connectionViewPair.Value;
      view.Value = view.FromNeuronView.Value * connection.Weight;
    }
  }

  private void FixedUpdate()
  {
    if (Camera.current != null)
    {
      foreach (var neuronView in neuronViews.Values)
      {
        neuronView.UpdateNameRotation();
      }
    }

    foreach (var connectionView in connectionViews.Values)
    {
      connectionView.UpdatePosition();
    }
  }
  private void ClearViews()
  {
    foreach (var neuronView in neuronViews.Values)
    {
      neuronView.gameObject.SetActive(false);
      neuronViewsPool.Push(neuronView);
    }
    foreach (var connectionView in connectionViews.Values)
    {
      connectionView.gameObject.SetActive(false);
      connectionViewPool.Push(connectionView);
    }
    neuronViews.Clear();
    connectionViews.Clear();
  }

  private void CreateViews()
  {
    foreach (var neuron in neuralNetwork.GetNeurons())
    {
      CreateNeuronView((NamedNeuron)neuron);
    }

    foreach (var neuron in neuralNetwork.GetNeurons())
    {
      foreach (var connection in neuron.IncomingConnections)
      {
        CreateConnectionView(connection.Value, neuronViews[neuron], neuronViews[connection.Key]);
        neuronViews[neuron].Attach(neuronViews[connection.Key], Mathf.Max(0, neuralNetwork.NeuronsCount * 0.01f));
      }
    }
  }

  private NeuronView CreateNeuronView(NamedNeuron neuron)
  {
    var view = neuronViewsPool.Count > 0 ? neuronViewsPool.Pop() : Instantiate(prefabNeuron,
      transform.position + new Vector3(Random.Range(-2f, 2f), Random.Range(-2f, 2f), Random.Range(-2f, 2f)), Quaternion.identity, transform);

    view.Name = neuron.Name;

    neuronViews.Add(neuron, view);
    return view;
  }

  private ConnectionView CreateConnectionView(Connection connection, NeuronView fromNeuronView, NeuronView toNeuronView)
  {
    var view = connectionViewPool.Count > 0 ? connectionViewPool.Pop() : Instantiate(prefabConnection, transform);
    view.SetTargets(fromNeuronView, toNeuronView);

    connectionViews.Add(connection, view);
    return view;
  }
}
