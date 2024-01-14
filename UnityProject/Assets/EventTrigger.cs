using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class EventTrigger : MonoBehaviour
{
  public class TriggerEnterEvent : UnityEvent<Collider, Collider> { }

  public TriggerEnterEvent TriggerEnter => triggerEnter;
  public TriggerEnterEvent TriggerStay => triggerStay;
  public TriggerEnterEvent TriggerExit => triggerExit;

  private new Collider collider;

  [SerializeField]
  private TriggerEnterEvent triggerEnter;
  [SerializeField]
  private TriggerEnterEvent triggerStay;
  [SerializeField]
  private TriggerEnterEvent triggerExit;

  private void Awake()
  {
    collider = GetComponent<Collider>();
  }

  private void OnTriggerEnter(Collider other)
  {
    triggerEnter.Invoke(collider, other);
  }
  private void OnTriggerStay(Collider other)
  {
    triggerStay.Invoke(collider, other);
  }
  private void OnTriggerExit(Collider other)
  {
    triggerExit.Invoke(collider, other);
  }
}
