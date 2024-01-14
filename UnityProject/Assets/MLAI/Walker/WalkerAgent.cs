using GraphNeuralNetworks;
using System.Collections.Generic;
using UnityEngine;

public class WalkerAgent : Agent
{
  public Transform Orinetation => orinetation;
  public HumanoidBody Body => humanoidBody;
  public HumanoidBodyNeurons BodyNeurons => humanoidBodyNeurons;

  public IReadOnlyDictionary<BodyPart, BodyPartNeurons> BodyPartsAndNeurons => bodyPartsAndNeurons;

  public Transform Target
  {
    get => target;
    set => target = value;
  }

  [SerializeField]
  private HumanoidBody humanoidBody;

  [SerializeField]
  private Transform target;

  [SerializeField]
  private Transform orinetation;

  private HumanoidBodyNeurons humanoidBodyNeurons = new HumanoidBodyNeurons();

  private Vector3Neurons currentVelocityNeurons = new Vector3Neurons("currentVelocity");
  private Vector3Neurons targetPositionNeurons = new Vector3Neurons("targetPosition");
  private QuaternionNeurons targetRotationDeltaNeurons = new QuaternionNeurons("targetRotationDelta");
  
  private Dictionary<BodyPart, BodyPartNeurons> bodyPartsAndNeurons = new Dictionary<BodyPart, BodyPartNeurons>();

  private Vector3 offset;

  protected override void Awake()
  {
    offset = Body.Chest.transform.localPosition;

    base.Awake();

    UpdateBodyPartsAndNeurons();

    CreateBodyPartNeurons();
    CreateConnections();
  }

  private void FixedUpdate()
  {
    UpdateOrinetation();

    UpdateOtherNeourons();
    UpdateBodyPartNeourons();

    networkRunner.Step();

    UpdateBodyPart();
  }

  public void UpdateBodyPartsAndNeurons()
  {
    bodyPartsAndNeurons[humanoidBody.Head] = humanoidBodyNeurons.Head;

    bodyPartsAndNeurons[humanoidBody.Chest] = humanoidBodyNeurons.Chest;
    bodyPartsAndNeurons[humanoidBody.Belly] = humanoidBodyNeurons.Belly;
    bodyPartsAndNeurons[humanoidBody.Pelvis] = humanoidBodyNeurons.Pelvis;

    bodyPartsAndNeurons[humanoidBody.ForearmL] = humanoidBodyNeurons.ForearmL;
    bodyPartsAndNeurons[humanoidBody.ShinL] = humanoidBodyNeurons.ShinL;

    bodyPartsAndNeurons[humanoidBody.ForearmR] = humanoidBodyNeurons.ForearmR;
    bodyPartsAndNeurons[humanoidBody.ShinR] = humanoidBodyNeurons.ShinR;

    bodyPartsAndNeurons[humanoidBody.ArmL] = humanoidBodyNeurons.ArmL;
    bodyPartsAndNeurons[humanoidBody.ForearmL] = humanoidBodyNeurons.ForearmL;

    bodyPartsAndNeurons[humanoidBody.ArmR] = humanoidBodyNeurons.ArmR;
    bodyPartsAndNeurons[humanoidBody.ForearmR] = humanoidBodyNeurons.ForearmR;

  }

  #region CreateNeuralNetwork
  private void CreateBodyPartNeurons()
  {
    neuralNetwork.AddNeurons(currentVelocityNeurons);
    neuralNetwork.AddNeurons(targetPositionNeurons);
    neuralNetwork.AddNeurons(targetRotationDeltaNeurons);

    foreach (var bodyPartNeurons in humanoidBodyNeurons.Neurons)
    {
      neuralNetwork.AddNeurons(bodyPartNeurons);
    }
  }

  private void CreateConnections()
  {
    foreach (var toBodyPartNeurons in humanoidBodyNeurons.Neurons)
    {
      CreateConnections(currentVelocityNeurons, toBodyPartNeurons);
      CreateConnections(targetPositionNeurons, toBodyPartNeurons);

      CreateConnections(targetRotationDeltaNeurons, toBodyPartNeurons);
    }

    foreach (var frombodyPartNeurons in bodyPartsAndNeurons.Values)
    {
      foreach (var toBodyPartNeurons in bodyPartsAndNeurons.Values)
      {
        CreateConnections(frombodyPartNeurons, toBodyPartNeurons);
      }
    }
  }

  private void CreateConnections(BodyPartNeurons frombodyPartNeurons, BodyPartNeurons toBodyPartNeurons)
  {
    CreateConnections(frombodyPartNeurons.GroundNeuronOutput, toBodyPartNeurons);

    CreateConnections(frombodyPartNeurons.VelocityNeuronOutput, toBodyPartNeurons);
    CreateConnections(frombodyPartNeurons.AngularVelocityNeuronOutput, toBodyPartNeurons);

    CreateConnections(frombodyPartNeurons.PositionNeuronOutput, toBodyPartNeurons);
    CreateConnections(frombodyPartNeurons.RotationNeuronOutput, toBodyPartNeurons);

    CreateConnections(frombodyPartNeurons.ForceLimitNeuronOutput, toBodyPartNeurons);
  }
  private void CreateConnections(QuaternionNeurons fromNeurons, BodyPartNeurons toBodyPartNeurons)
  {
    CreateConnections(fromNeurons.NeuronW, toBodyPartNeurons);
    CreateConnections(fromNeurons.NeuronX, toBodyPartNeurons);
    CreateConnections(fromNeurons.NeuronY, toBodyPartNeurons);
    CreateConnections(fromNeurons.NeuronZ, toBodyPartNeurons);
  }
  private void CreateConnections(Vector3Neurons fromNeurons, BodyPartNeurons toBodyPartNeurons)
  {
    CreateConnections(fromNeurons.NeuronX, toBodyPartNeurons);
    CreateConnections(fromNeurons.NeuronY, toBodyPartNeurons);
    CreateConnections(fromNeurons.NeuronZ, toBodyPartNeurons);
  }
  private void CreateConnections(Neuron fromNeuron, BodyPartNeurons toBodyPartNeurons)
  {
    CreateConnection(fromNeuron, toBodyPartNeurons.RotationNeuronInput.NeuronX);
    CreateConnection(fromNeuron, toBodyPartNeurons.RotationNeuronInput.NeuronY);
    CreateConnection(fromNeuron, toBodyPartNeurons.RotationNeuronInput.NeuronZ);

    CreateConnection(fromNeuron, toBodyPartNeurons.ForceLimitNeuronInput.NeuronX);
    CreateConnection(fromNeuron, toBodyPartNeurons.ForceLimitNeuronInput.NeuronY);
    CreateConnection(fromNeuron, toBodyPartNeurons.ForceLimitNeuronInput.NeuronZ);
  }
  private void CreateConnection(Neuron fromNeuron, Neuron toNeuron)
  {
    neuralNetwork.AddConnection(fromNeuron, toNeuron, new Connection(Random.Range(-0.1f, 0.1f)));
  }
  #endregion

  private void UpdateOrinetation()
  {
    orinetation.position = humanoidBody.Chest.transform.position;
    orinetation.rotation = Quaternion.LookRotation(target.position - humanoidBody.Chest.transform.position);
  }

  #region Update Neourons And BodyParts
  private void UpdateOtherNeourons()
  {
    var currentVelocity = humanoidBody.GetAvgVelocity();

    //Текущая сила отностильено ориентации
    currentVelocityNeurons.SetValue(orinetation.InverseTransformDirection(currentVelocity));

    //Вращение до цели относительно персонажа
    targetRotationDeltaNeurons.SetValue(Quaternion.FromToRotation(Body.Head.transform.forward, orinetation.forward));

    //Позиция цели относительно персонажа
    targetPositionNeurons.SetValue(orinetation.InverseTransformPoint(target.transform.position));

    networkRunner.AddNeuronConnectionsToQueue(currentVelocityNeurons.NeuronX);
    networkRunner.AddNeuronConnectionsToQueue(currentVelocityNeurons.NeuronY);
    networkRunner.AddNeuronConnectionsToQueue(currentVelocityNeurons.NeuronZ);

    networkRunner.AddNeuronConnectionsToQueue(targetPositionNeurons.NeuronX);
    networkRunner.AddNeuronConnectionsToQueue(targetPositionNeurons.NeuronY);
    networkRunner.AddNeuronConnectionsToQueue(targetPositionNeurons.NeuronZ);
  }

  private void UpdateBodyPartNeourons()
  {
    foreach (var pair in bodyPartsAndNeurons)
    {
      BodyPart bodyPart = pair.Key;
      BodyPartNeurons bodyPartNeurons = pair.Value;

      //Проверка земли
      bodyPartNeurons.SetGround(bodyPart.GroundChecker.Thouching);

      //Скорость в пространстве ориентации
      bodyPartNeurons.SetVelocity(orinetation.InverseTransformDirection(bodyPart.ArticulationBody.velocity));
      bodyPartNeurons.SetAngularVelocity(orinetation.InverseTransformDirection(bodyPart.ArticulationBody.angularVelocity));

      if (bodyPart != humanoidBody.Chest)
      {
        //Позиция относительно туловища
        bodyPartNeurons.SetPosition(orinetation.InverseTransformDirection(bodyPart.transform.position - humanoidBody.Chest.transform.position));
        //Вращение относительно туловища
        bodyPartNeurons.SetRotation(bodyPart.transform.localEulerAngles);
        //Максимальная сила применяемая к конечности
        bodyPartNeurons.SetForceLimit(bodyPart.GetForceLimitDrive());
      }

      networkRunner.AddNeuronConnectionsToQueue(bodyPartNeurons.GroundNeuronOutput);

      networkRunner.AddNeuronConnectionsToQueue(bodyPartNeurons.VelocityNeuronOutput.NeuronX);
      networkRunner.AddNeuronConnectionsToQueue(bodyPartNeurons.VelocityNeuronOutput.NeuronY);
      networkRunner.AddNeuronConnectionsToQueue(bodyPartNeurons.VelocityNeuronOutput.NeuronZ);

      networkRunner.AddNeuronConnectionsToQueue(bodyPartNeurons.AngularVelocityNeuronOutput.NeuronX);
      networkRunner.AddNeuronConnectionsToQueue(bodyPartNeurons.AngularVelocityNeuronOutput.NeuronY);
      networkRunner.AddNeuronConnectionsToQueue(bodyPartNeurons.AngularVelocityNeuronOutput.NeuronZ);

      networkRunner.AddNeuronConnectionsToQueue(bodyPartNeurons.PositionNeuronOutput.NeuronX);
      networkRunner.AddNeuronConnectionsToQueue(bodyPartNeurons.PositionNeuronOutput.NeuronY);
      networkRunner.AddNeuronConnectionsToQueue(bodyPartNeurons.PositionNeuronOutput.NeuronZ);

      networkRunner.AddNeuronConnectionsToQueue(bodyPartNeurons.RotationNeuronOutput.NeuronX);
      networkRunner.AddNeuronConnectionsToQueue(bodyPartNeurons.RotationNeuronOutput.NeuronY);
      networkRunner.AddNeuronConnectionsToQueue(bodyPartNeurons.RotationNeuronOutput.NeuronZ);
    }
  }

  private void UpdateBodyPart()
  {
    foreach (var pair in bodyPartsAndNeurons)
    {
      BodyPart bodyPart = pair.Key;
      BodyPartNeurons bodyPartNeurons = pair.Value;

      bodyPart.SetTargetRotation(bodyPartNeurons.GetRotation());
      bodyPart.SetForceLimitDrive(bodyPartNeurons.GetForceLimit());
    }
  }
  #endregion

  public override void TeleportAgent(Vector3 position, Quaternion rotation)
  {
    transform.position = position;
    humanoidBody.Chest.ArticulationBody.TeleportRoot(position + offset, rotation);
  }

  public override void ResetAgent()
  {
    base.ResetAgent();
    humanoidBody.ResetBody();
  }
}
