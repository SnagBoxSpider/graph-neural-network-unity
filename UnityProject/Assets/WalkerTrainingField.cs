using System;
using UnityEngine;

public class WalkerTrainingField : TrainingField<WalkerAgent>
{
  [SerializeField] private CustomNeuralNetworkView neuralNetworkView;

  [SerializeField] private WalkerAgent agentPrefab;

  [SerializeField] private Transform startPoint;
  [SerializeField] private Transform targetPoint;
  [SerializeField] private float targetSpeed = 1f;

  protected override void OnEpisodeBegin()
  {
    base.OnEpisodeBegin();

    Agent.ResetAgent();
    Agent.TeleportAgent(startPoint.position, startPoint.rotation);
  }

  protected override void OnEpisodeUpdate()
  {
    base.OnEpisodeUpdate();
    var cubeForward = Agent.Orinetation.forward;

    // Set reward for this step according to mixture of the following elements.
    // a. Match target speed
    //This reward will approach 1 if it matches perfectly and approach zero as it deviates
    var targetVelocity = cubeForward * targetSpeed;
    var matchSpeedReward = GetMatchingVelocityReward(targetVelocity, Agent.Body.GetAvgVelocity());

    //Check for NaNs
    if (float.IsNaN(matchSpeedReward))
    {
      throw new ArgumentException($"NaN in moveTowardsTargetReward.\n cubeForward: {cubeForward}\n" +
          $" currentVelocity: {Agent.Body.GetAvgVelocity()}\n targetVelocity: {targetVelocity}");
    }

    //Если перс смотрит в направлении
    var headForward = Agent.Body.Head.transform.forward;
    var lookAtTargetReward = (Vector3.Dot(cubeForward, headForward) + 1) * .5F;

    //Check for NaNs
    if (float.IsNaN(lookAtTargetReward))
    {
      throw new ArgumentException($"NaN in lookAtTargetReward.\n cubeForward: {cubeForward}\n head.forward: {headForward}");
    }

    Reward += matchSpeedReward * lookAtTargetReward * Time.fixedDeltaTime;
  }

  protected override void OnEpisodeEnd()
  {
    base.OnEpisodeEnd();

    Agent.ResetAgent();
    Agent.TeleportAgent(startPoint.position, startPoint.rotation);
  }

  //normalized value of the difference in avg speed vs goal walking speed.
  public float GetMatchingVelocityReward(Vector3 velocityGoal, Vector3 actualVelocity)
  {
    //distance between our actual velocity and goal velocity
    var velDeltaMagnitude = Mathf.Clamp(Vector3.Distance(actualVelocity, velocityGoal), 0, targetSpeed);

    //return the value on a declining sigmoid shaped curve that decays from 1 to 0
    //This reward will approach 1 if it matches perfectly and approach zero as it deviates
    return Mathf.Pow(1 - Mathf.Pow(velDeltaMagnitude / targetSpeed, 2), 2);
  }

  protected override WalkerAgent CreateAgent()
  {
    var agent = Instantiate(agentPrefab, startPoint.position, startPoint.rotation, transform);
    agent.Target = targetPoint;

    if(neuralNetworkView != null)
      neuralNetworkView.NeuralNetwork = agent.NeuralNetwork;

    return agent;
  }
}
