using Reversible.Unity;
using Reversible.Value;
using UnityEngine;
using Witch.Bullet;

namespace Witch.Momiji {
  public class FairyOfLight : ReversibleBehaviour {
    private Transform field_;

    // Color of this fairy
    [SerializeField] public Color color = new(1, 1,1, 1);

    // Rotation
    [SerializeField] private float poleRotationSpeed = 60.0f;
    [SerializeField] public Vector3 poleRotationAxis = Vector3.up;
    [SerializeField] private float selfRotationSpeed = 180.0f;
    [SerializeField] private float selfRadius = 4.0f;
    [SerializeField] public float initialAngle;
    [SerializeField] public float initialPoleAngle;

    // Time management
    [SerializeField] private float bornTime = 1.0f;
    [SerializeField] private float rotateTime = 4.0f;
    [SerializeField] private float endTime = 6.0f;
    public float Duration { get; private set; }

    private Dense<float> totalTime_;

    // Spirit components
    private Transform spirit_;
    private Transform trail_;
    
    // Arrow emission
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] public float arrowLaunchDelay;
    [SerializeField] public float arrowLaunchInterval = 37f/41f;
    private Dense<float> nextArrowLaunch_;

    protected override void OnStart() {
      field_ = GameObject.FindWithTag("Field").transform;

      var trans = transform;
      spirit_ = trans.Find("Spirit")!;
      trail_ = spirit_.Find("Trail")!;
      spirit_.gameObject.GetComponent<MeshRenderer>().material.color = color;
      spirit_.GetComponent<Light>().color = color;

      var lineRenderer = trail_.GetComponent<LineRenderer>();
      lineRenderer.endColor = color;
      Duration = endTime + trail_.GetComponent<MakeLineRendererAsReversibleTrail>().lifeTime;
      totalTime_ = new Dense<float>(clock, 0.0f);
      nextArrowLaunch_ = new Dense<float>(clock, arrowLaunchDelay);
    }

    protected override void OnForward() {
      var trans = transform;
      var dt = Time.deltaTime;
      ref var totalTime = ref totalTime_.Mut;
      ref var nextArrowLaunch = ref nextArrowLaunch_.Mut;
      if (totalTime <= bornTime) {
        var progress = totalTime / bornTime;
        var spiritScale = 0.7f * progress;
        spirit_.localScale = new Vector3(spiritScale, spiritScale, spiritScale);
        trans.localRotation = Quaternion.AngleAxis(initialPoleAngle + totalTime * poleRotationSpeed, poleRotationAxis);
        spirit_.localPosition = Quaternion.AngleAxis(initialAngle + totalTime * selfRotationSpeed, Vector3.forward) * Vector3.right * (selfRadius * progress);
      } else if (totalTime <= rotateTime) {
        trans.localRotation = Quaternion.AngleAxis(initialPoleAngle + totalTime * poleRotationSpeed, poleRotationAxis);
        spirit_.localPosition = Quaternion.AngleAxis(initialAngle + totalTime * selfRotationSpeed, Vector3.forward) * Vector3.right * selfRadius;
        { // Launching Arrow
          if (nextArrowLaunch < 0.0f) {
            //LaunchArrow();
            nextArrowLaunch += arrowLaunchInterval;
          }
          nextArrowLaunch -= dt;
        }
      } else if (totalTime <= endTime){
        var spanTime = totalTime - rotateTime;
        var progress = (1 - spanTime / (endTime - rotateTime));
        var spiritScale = 0.7f * progress;
        spirit_.localScale = new Vector3(spiritScale, spiritScale, spiritScale);
        trans.localRotation = Quaternion.AngleAxis(initialPoleAngle + totalTime * poleRotationSpeed, poleRotationAxis);
        spirit_.localPosition = Quaternion.AngleAxis(initialAngle + totalTime * selfRotationSpeed, Vector3.forward) * Vector3.right * (selfRadius * progress);
      } else if (totalTime <= Duration) {
        spirit_.localScale = Vector3.zero;
      } else {
        Destroy();
      }

      totalTime += dt;
    }

    private void LaunchArrow() {
      var trans = transform;
      var transPosition = trans.localPosition;
      var spiritPosition = field_.InverseTransformPoint(spirit_.position);
      var direction = spiritPosition - transPosition;
      var obj = Instantiate(arrowPrefab, spiritPosition, Quaternion.LookRotation(direction, Vector3.up), field_);
      var arrow = obj.GetComponent<ArrowOfLight>();
      arrow.initialVelocity = direction * 100.0f;
      arrow.color = color;
    }
  }
}
