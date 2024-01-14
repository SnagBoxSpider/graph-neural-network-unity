using NeuralNetworks;
using System.Collections.Generic;
using UnityEngine;

public class TestNN : MonoBehaviour
{
  public UnitBody Body => body;

  [SerializeField]
  private UnitBody body;

  private NeuralNetwork neuralNetwork;

  private float[] input;
  private void Awake()
  {
    int len = body.Segments.Count * 4 + body.Vision.Size.x * body.Vision.Size.y;
    neuralNetwork = new NeuralNetwork(new int[] { len, 5, (body.Segments.Count * 3) });
    input = new float[len];

    //Body
    for (int i = 0; i < body.Segments.Count; i++)
    {
      int index = i;
      var segment = body.Segments[i];
      segment.Trigger.CollisionEnter.AddListener((c, c1) => UpdateSegmentCollisionNeuron(index, 0f));
      segment.Trigger.CollisionExit.AddListener((c, c1) => UpdateSegmentCollisionNeuron(index, 1f));
    }

    body.Vision.DataChanged.AddListener(vision_DataChanged);
  }

  private void FixedUpdate()
  {
    UpdateSegmentngAnglesNeurons();

    float[] outputs = neuralNetwork.Run(input);

    for (int i = 0; i < body.Segments.Count; i++)
    {
      var segment = body.Segments[i];
      segment.RotateX(outputs[i]);
      segment.RotateY(outputs[i]);
      segment.RotateZ(outputs[i]);
    }
  }

  private void UpdateSegmentngAnglesNeurons()
  {
    for (int i = 0; i < body.Segments.Count; i++)
    {
      var segment = body.Segments[i];

      input[i] = segment.ArticulationBody.xDrive.target / 360f;
      input[i] = segment.ArticulationBody.xDrive.target / 360f;
      input[i] = segment.ArticulationBody.xDrive.target / 360f;
    }
  }

  private void UpdateSegmentCollisionNeuron(int index, float value)
  {
    input[body.Segments.Count * 3 + index] = value;
  }

  private void vision_DataChanged()
  {
    Color[] colors = body.Vision.Colors;
    for (int i = 0; i < colors.Length; i++)
    {
      var color = colors[i];
      input[body.Segments.Count * 4 + i] = color.g;
    }
  }
}
