using System.Collections.Generic;

public class HumanoidBodyNeurons
{
  public IReadOnlyList<BodyPartNeurons> Neurons => neurons;

  public BodyPartNeurons Head => head;

  public BodyPartNeurons Chest => chest;
  public BodyPartNeurons Belly => belly;
  public BodyPartNeurons Pelvis => pelvis;

  public BodyPartNeurons ThighL => thighL;
  public BodyPartNeurons ShinL => shinL;

  public BodyPartNeurons ThighR => thighR;
  public BodyPartNeurons ShinR => shinR;

  public BodyPartNeurons ArmL => armL;
  public BodyPartNeurons ForearmL => forearmL;

  public BodyPartNeurons ArmR => armR;
  public BodyPartNeurons ForearmR => forearmR;

  private BodyPartNeurons head;

  private BodyPartNeurons chest;
  private BodyPartNeurons belly;
  private BodyPartNeurons pelvis;

  private BodyPartNeurons thighL;
  private BodyPartNeurons shinL;

  private BodyPartNeurons thighR;
  private BodyPartNeurons shinR;

  private BodyPartNeurons armL;
  private BodyPartNeurons forearmL;

  private BodyPartNeurons armR;
  private BodyPartNeurons forearmR;

  private List<BodyPartNeurons> neurons = new List<BodyPartNeurons>(12);

  public HumanoidBodyNeurons()
  {
    neurons.Add(head = new BodyPartNeurons("head"));

    neurons.Add(chest = new BodyPartNeurons("chest"));
    neurons.Add(belly = new BodyPartNeurons("belly"));
    neurons.Add(pelvis = new BodyPartNeurons("pelvis"));

    neurons.Add(thighL = new BodyPartNeurons("thighL"));
    neurons.Add(shinL = new BodyPartNeurons("shinL"));

    neurons.Add(thighR = new BodyPartNeurons("thighR"));
    neurons.Add(shinR = new BodyPartNeurons("shinR"));

    neurons.Add(armL = new BodyPartNeurons("armL"));
    neurons.Add(forearmL = new BodyPartNeurons("forearmL"));

    neurons.Add(armR = new BodyPartNeurons("armR"));
    neurons.Add(forearmR = new BodyPartNeurons("forearmR"));
  }
}
