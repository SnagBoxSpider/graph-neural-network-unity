using NeuralNetworks;
using System.Linq;

[System.Serializable]
public class UnitNeuralNetworkSaveData
{
  public float[] NeuralNetworkWeights;
  public CustomNeuralNetworkRunnerSaveData Runner;

  public int TimeNeuronId;
  public int[] SegmentInputCollisionNeuronIds;
  public int[] SegmentInputSwingZNeuronIds;
  public int[] SegmentInputTwistNeuronIds;
  public int[] SegmentInputSwingYNeuronIds;

  public int[] SegmentOutputSwingZNeuronIds;
  public int[] SegmentOutputTwistNeuronIds;
  public int[] SegmentOutputSwingYNeuronIds;
}

public class UnitNeuralNetwork
{
  public MutationNeuralNetwork NeuralNetwork
  {
    get { return neuralNetwork; }
  }
  public CustomNeuralNetworkRunner Runner
  {
    get { return networkRunner; }
  }

  //Vision
  /*public int[] visionNeuronIds;
  private int[] visionTypeNeuronIds;*/
  
  public int timeNeuronId;
  //Segments
  public int[] segmentInputCollisionNeuronIds;
  public int[] segmentInputSwingZNeuronIds;
  public int[] segmentInputTwistNeuronIds;
  public int[] segmentInputSwingYNeuronIds;

  public int[] segmentOutputSwingZNeuronIds;
  public int[] segmentOutputTwistNeuronIds;
  public int[] segmentOutputSwingYNeuronIds;

  private MutationNeuralNetwork neuralNetwork;
  private CustomNeuralNetworkRunner networkRunner;

  public UnitNeuralNetwork() { }

  public void ResetAllNeuronValues()
  {
    foreach (var neuronId in neuralNetwork.GetNeuronIds().ToList())
    {
      SetNeuron(neuronId, 0);
    }
  }

  public UnitNeuralNetwork Clone()
  {
    UnitNeuralNetwork unitNeuralNetwork = new UnitNeuralNetwork();

    unitNeuralNetwork.neuralNetwork = new MutationNeuralNetwork(neuralNetwork);
    unitNeuralNetwork.networkRunner = new CustomNeuralNetworkRunner(unitNeuralNetwork.neuralNetwork);

    /*visionNeuronIds.CopyTo(unitNeuralNetwork.visionNeuronIds, 0);
    visionTypeNeuronIds.CopyTo(unitNeuralNetwork.visionTypeNeuronIds, 0);*/

    unitNeuralNetwork.timeNeuronId = timeNeuronId;

    unitNeuralNetwork.segmentInputCollisionNeuronIds = new int[segmentInputCollisionNeuronIds.Length];
    segmentInputCollisionNeuronIds.CopyTo(unitNeuralNetwork.segmentInputCollisionNeuronIds, 0);
    unitNeuralNetwork.segmentInputSwingZNeuronIds = new int[segmentInputSwingZNeuronIds.Length];
    segmentInputSwingZNeuronIds.CopyTo(unitNeuralNetwork.segmentInputSwingZNeuronIds, 0);
    unitNeuralNetwork.segmentInputTwistNeuronIds = new int[segmentInputTwistNeuronIds.Length];
    segmentInputTwistNeuronIds.CopyTo(unitNeuralNetwork.segmentInputTwistNeuronIds, 0);
    unitNeuralNetwork.segmentInputSwingYNeuronIds = new int[segmentInputSwingYNeuronIds.Length];
    segmentInputSwingYNeuronIds.CopyTo(unitNeuralNetwork.segmentInputSwingYNeuronIds, 0);

    unitNeuralNetwork.segmentOutputSwingZNeuronIds = new int[segmentOutputSwingZNeuronIds.Length];
    segmentOutputSwingZNeuronIds.CopyTo(unitNeuralNetwork.segmentOutputSwingZNeuronIds, 0);
    unitNeuralNetwork.segmentOutputTwistNeuronIds = new int[segmentOutputTwistNeuronIds.Length];
    segmentOutputTwistNeuronIds.CopyTo(unitNeuralNetwork.segmentOutputTwistNeuronIds, 0);
    unitNeuralNetwork.segmentOutputSwingYNeuronIds = new int[segmentOutputSwingYNeuronIds.Length];
    segmentOutputSwingYNeuronIds.CopyTo(unitNeuralNetwork.segmentOutputSwingYNeuronIds, 0);

    return unitNeuralNetwork;
  }

  public void Create(int segmentsCount, int visionTypesCount, int visionSize)
  {
    neuralNetwork = new MutationNeuralNetwork();
    networkRunner = new CustomNeuralNetworkRunner(neuralNetwork);

    timeNeuronId = neuralNetwork.AddNeuron(new NeuronData($"Time (Input)"));

    //CreateVisionNetwork(visionTypesCount, visionSize);
    CreateSegmentsNetwork(segmentsCount);

    /*for (int i = 0; i < visionTypeNeuronIds.Length; i++)
    {
      neuralNetwork.AddAllConnections(visionTypeNeuronIds[i], segmentOutputSwingZNeuronIds);
      neuralNetwork.AddAllConnections(visionTypeNeuronIds[i], segmentOutputTwistNeuronIds);
      neuralNetwork.AddAllConnections(visionTypeNeuronIds[i], segmentOutputSwingYNeuronIds);
    }*/

    neuralNetwork.AddAllConnections(timeNeuronId, segmentOutputSwingZNeuronIds);
    neuralNetwork.AddAllConnections(timeNeuronId, segmentOutputTwistNeuronIds);
    neuralNetwork.AddAllConnections(timeNeuronId, segmentOutputSwingYNeuronIds);

    neuralNetwork.AddAllConnections(segmentInputCollisionNeuronIds, segmentOutputSwingZNeuronIds);
    neuralNetwork.AddAllConnections(segmentInputCollisionNeuronIds, segmentOutputTwistNeuronIds);
    neuralNetwork.AddAllConnections(segmentInputCollisionNeuronIds, segmentOutputSwingYNeuronIds);

    neuralNetwork.AddAllConnections(segmentInputSwingZNeuronIds, segmentOutputSwingZNeuronIds);
    neuralNetwork.AddAllConnections(segmentInputSwingZNeuronIds, segmentOutputTwistNeuronIds);
    neuralNetwork.AddAllConnections(segmentInputSwingZNeuronIds, segmentOutputSwingYNeuronIds);

    neuralNetwork.AddAllConnections(segmentInputTwistNeuronIds, segmentOutputSwingZNeuronIds);
    neuralNetwork.AddAllConnections(segmentInputTwistNeuronIds, segmentOutputTwistNeuronIds);
    neuralNetwork.AddAllConnections(segmentInputTwistNeuronIds, segmentOutputSwingYNeuronIds);

    neuralNetwork.AddAllConnections(segmentInputSwingYNeuronIds, segmentOutputSwingZNeuronIds);
    neuralNetwork.AddAllConnections(segmentInputSwingYNeuronIds, segmentOutputTwistNeuronIds);
    neuralNetwork.AddAllConnections(segmentInputSwingYNeuronIds, segmentOutputSwingYNeuronIds);

    neuralNetwork.SetRandomWeights();
  }
  /*private void CreateVisionNetwork(int visionTypesCount, int visionSize)
  {
    visionNeuronIds = new int[visionSize];
    for (int i = 0; i < visionSize; i++)
    {
        visionNeuronIds[i] = neuralNetwork.AddNeuron(new NeuronData($"Vision Sensor [{i}] (Input)"));
    }

    visionTypeNeuronIds = new int[visionTypesCount];
    for (int i = 0; i < visionTypesCount; i++)
    {
      visionTypeNeuronIds[i] = neuralNetwork.AddNeuron(new NeuronData($"VisionType [{i}] (Hidden)"));
    }

    neuralNetwork.AddAllConnections(visionNeuronIds, visionTypeNeuronIds);
  }*/

  private void CreateSegmentsNetwork(int segmentsCount)
  {
    segmentInputSwingZNeuronIds = new int[segmentsCount];
    segmentInputTwistNeuronIds = new int[segmentsCount];
    segmentInputSwingYNeuronIds = new int[segmentsCount];

    segmentInputCollisionNeuronIds = new int[segmentsCount];

    for (int i = 0; i < segmentsCount; i++)
    {
      segmentInputSwingZNeuronIds[i] = neuralNetwork.AddNeuron(new NeuronData($"Segment = {i} Z (Input)"));
      segmentInputTwistNeuronIds[i] = neuralNetwork.AddNeuron(new NeuronData($"Segment = {i} X (Input)"));
      segmentInputSwingYNeuronIds[i] = neuralNetwork.AddNeuron(new NeuronData($"Segment = {i} Y (Input)"));

      segmentInputCollisionNeuronIds[i] = neuralNetwork.AddNeuron(new NeuronData($"Segment = {i} Collision (Input)"));
    }

    segmentOutputSwingZNeuronIds = new int[segmentsCount];
    segmentOutputTwistNeuronIds = new int[segmentsCount];
    segmentOutputSwingYNeuronIds = new int[segmentsCount];

    for (int i = 0; i < segmentsCount; i++)
    {
      segmentOutputSwingZNeuronIds[i] = neuralNetwork.AddNeuron(new NeuronData($"Move Segment = {i} Z (Output)"));
      segmentOutputTwistNeuronIds[i] = neuralNetwork.AddNeuron(new NeuronData($"Move Segment = {i} X (Output)"));
      segmentOutputSwingYNeuronIds[i] = neuralNetwork.AddNeuron(new NeuronData($"Move Segment = {i} Y (Output)"));
    }
  }


  public float GetNeuron(int neuronId)
  {
    return neuralNetwork.GetNeuron(neuronId).Value;
  }
  public void SetNeuron(int neuronId, float value)
  {
    NeuronData neuronData = neuralNetwork.GetNeuron(neuronId);
    neuronData.Value = value;
    networkRunner.AddNeuronConnectionsToQueue(neuronId);
  }

  #region SaveLoad
  public UnitNeuralNetworkSaveData Save()
  {
    return new UnitNeuralNetworkSaveData
    {
      NeuralNetworkWeights = neuralNetwork.GetConnections().Select((c) => c.Weight).ToArray(),
      Runner = networkRunner.Save(),

      TimeNeuronId = timeNeuronId,

      SegmentInputCollisionNeuronIds = segmentInputCollisionNeuronIds,
      SegmentInputSwingZNeuronIds = segmentInputSwingZNeuronIds,
      SegmentInputTwistNeuronIds = segmentInputTwistNeuronIds,
      SegmentInputSwingYNeuronIds = segmentInputSwingYNeuronIds,

      SegmentOutputSwingZNeuronIds = segmentOutputSwingZNeuronIds,
      SegmentOutputTwistNeuronIds = segmentOutputTwistNeuronIds,
      SegmentOutputSwingYNeuronIds = segmentOutputSwingYNeuronIds,
    };
  }

  public void Load(UnitNeuralNetworkSaveData saveData)
  {
    neuralNetwork.SetWeights(saveData.NeuralNetworkWeights);
    networkRunner.Load(saveData.Runner);

    timeNeuronId = saveData.TimeNeuronId;

    segmentInputCollisionNeuronIds = saveData.SegmentInputCollisionNeuronIds;
    segmentInputSwingZNeuronIds = saveData.SegmentInputSwingZNeuronIds;
    segmentInputTwistNeuronIds = saveData.SegmentInputTwistNeuronIds;
    segmentInputSwingYNeuronIds = saveData.SegmentInputSwingYNeuronIds;

    segmentOutputSwingZNeuronIds = saveData.SegmentOutputSwingZNeuronIds;
    segmentOutputTwistNeuronIds = saveData.SegmentOutputTwistNeuronIds;
    segmentOutputSwingYNeuronIds = saveData.SegmentOutputSwingYNeuronIds;
  }
  #endregion
}
