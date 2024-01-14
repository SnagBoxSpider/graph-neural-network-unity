using GraphNeuralNetworks;
using System.Collections.Generic;
using UnityEngine;

public class CustomNeuralNetworkView2D : CustomNeuralNetworkView
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
  private float radius = 10f;
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
      view.Value = view.FromNeuronView.Value;
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
    float angleDelta = 360f / neuralNetwork.NeuronsCount;
    int i = 0;
    foreach (var neuron in neuralNetwork.GetNeurons())
    {
      CreateNeuronView((NamedNeuron)neuron, angleDelta * i);
      i++;
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

  private NeuronView CreateNeuronView(NamedNeuron neuron, float angle)
  {
    Quaternion rotation = Quaternion.Euler(0, angle, 0);
    var view = neuronViewsPool.Count > 0 ? neuronViewsPool.Pop() : Instantiate(prefabNeuron,
      transform.position + rotation * transform.forward * radius, rotation, transform);

    view.Name = neuron.Name;

    neuronViews.Add(neuron, view);
    return view;
  }

  private ConnectionView CreateConnectionView(Connection connection, NeuronView fromNeuronView, NeuronView toNeuronView)
  {
    var view = connectionViewPool.Count > 0 ? connectionViewPool.Pop() : Instantiate(prefabConnection, transform);
    view.SetTargets(fromNeuronView, toNeuronView);
    view.UpdatePosition();

    connectionViews.Add(connection, view);
    return view;
  }
}
