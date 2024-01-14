using UnityEngine;

public class Unit : MonoBehaviour
{
  public UnitNeuralNetwork UnitNeuralNetwork 
  {
    get => unitNeuralNetwork;
    set => unitNeuralNetwork = value;
  }
  public UnitBody Body => body;

  [SerializeField]
  private UnitBody body;

  private UnitNeuralNetwork unitNeuralNetwork;

  public void CreateUnitNeuralNetwork()
  {
    int visionSize = body.Vision.Size.x * body.Vision.Size.y;
    unitNeuralNetwork = new UnitNeuralNetwork();
    unitNeuralNetwork.Create(body.Segments.Count, 2, visionSize);
  }
  private void Awake()
  {
    //Body
    for (int i = 0; i < body.Segments.Count; i++)
    {
      int index = i;
      var segment = body.Segments[i];
      segment.Trigger.CollisionEnter.AddListener((c, c1) => UpdateSegmentCollisionNeuron(index, 0f));
      segment.Trigger.CollisionExit.AddListener((c, c1) => UpdateSegmentCollisionNeuron(index, 1f));
    }

    //body.Vision.DataChanged.AddListener(vision_DataChanged);
  }

  private void FixedUpdate()
  {
    if (unitNeuralNetwork == null)
      return;

    UpdateTimeNeuron();
    UpdateSegmentngAnglesNeurons();
    unitNeuralNetwork.Runner.Step();
    MoveSegments();
  }

  private void MoveSegments()
  {
    for (int i = 0; i < body.Segments.Count; i++)
    {
      var segment = body.Segments[i];
      segment.RotateX(unitNeuralNetwork.GetNeuron(unitNeuralNetwork.segmentOutputTwistNeuronIds[i]));
      segment.RotateY(unitNeuralNetwork.GetNeuron(unitNeuralNetwork.segmentOutputSwingYNeuronIds[i]));
      segment.RotateZ(unitNeuralNetwork.GetNeuron(unitNeuralNetwork.segmentOutputSwingZNeuronIds[i]));
    }
  }

  private void UpdateSegmentngAnglesNeurons()
  {
    for (int i = 0; i < body.Segments.Count; i++)
    {
      var segment = body.Segments[i];

      unitNeuralNetwork.SetNeuron(unitNeuralNetwork.segmentInputTwistNeuronIds[i], segment.ArticulationBody.xDrive.target / 180f);
      unitNeuralNetwork.SetNeuron(unitNeuralNetwork.segmentInputSwingYNeuronIds[i], segment.ArticulationBody.xDrive.target / 180f);
      unitNeuralNetwork.SetNeuron(unitNeuralNetwork.segmentInputSwingZNeuronIds[i], segment.ArticulationBody.xDrive.target / 180f);
    }
  }
  
  private void UpdateTimeNeuron()
  {
    unitNeuralNetwork.SetNeuron(unitNeuralNetwork.timeNeuronId, Mathf.Sin(Time.time));
  }
  private void UpdateSegmentCollisionNeuron(int index, float value)
  {
    unitNeuralNetwork.SetNeuron(unitNeuralNetwork.segmentInputCollisionNeuronIds[index], value);    
  }

  /*private void vision_DataChanged()
  {
    Color[] colors = body.Vision.Colors;
    for (int i = 0; i < colors.Length; i++)
    {
      var color = colors[i];
      unitNeuralNetwork.SetNeuron(unitNeuralNetwork.visionNeuronIds[i], color.g);
    }
  }*/
}
