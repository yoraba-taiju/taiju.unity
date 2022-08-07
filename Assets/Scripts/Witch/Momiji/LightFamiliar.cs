using Reversible.Unity;
using Reversible.Value;
using UnityEngine;

namespace Witch.Momiji {
  public class LightFamiliar : ReversibleBehaviour {
    [SerializeField] private Vector3 pole = Vector3.up;
    [SerializeField] private Vector3 poleRotationAxis = Vector3.up;
    [SerializeField] private float initialAngle;
    [SerializeField] private float bornTime = 1.0f;
    [SerializeField] private float rotateTime = 7.0f;
    [SerializeField] private float endTime = 10.0f;
    private float destroyTime_;
    [SerializeField] private float poleRotationSpeed = 60.0f;
    [SerializeField] private float selfRotationSpeed = 180.0f;
    [SerializeField] private float selfRadius = 5.0f;
    private Quaternion poleRotation_;
    private Dense<float> totalTime_;
    private Transform spirit_;
    private TrailRenderer spiritTrailRenderer_;
    protected override void OnStart() {
      var trans = transform;
      poleRotation_ = Quaternion.FromToRotation(Vector3.up, pole.normalized);
      totalTime_ = new Dense<float>(clock, 0.0f);
      spirit_ = trans.Find("Spirit");
      spiritTrailRenderer_ = spirit_.GetComponent<TrailRenderer>();
      destroyTime_ = endTime + spiritTrailRenderer_.time;
      trans.localRotation = poleRotation_;
    }

    protected override void OnForward() {
      var trans = transform;
      var dt = Time.deltaTime;
      ref var totalTime = ref totalTime_.Mut;
      if (totalTime <= bornTime) {
        var progress = totalTime / bornTime;
        spirit_.localPosition = (Quaternion.AngleAxis(initialAngle, Vector3.up) * Vector3.right) * (selfRadius * progress * progress);
      } else if (totalTime <= rotateTime) {
        var spanTime = totalTime - bornTime;
        trans.localRotation = Quaternion.AngleAxis(spanTime * poleRotationSpeed, poleRotationAxis) * poleRotation_;
        spirit_.localPosition = Quaternion.AngleAxis(initialAngle + spanTime * selfRotationSpeed, Vector3.up) * Vector3.right * selfRadius;
      } else if (totalTime <= endTime){
        var spanTime = totalTime - rotateTime;
        var progress = (1 - spanTime / (endTime - rotateTime));
        spirit_.localScale = new Vector3(progress * 0.8f, progress * 0.8f, progress * 0.8f);
        trans.localRotation = Quaternion.AngleAxis(spanTime * poleRotationSpeed, poleRotationAxis) * poleRotation_;
        spirit_.localPosition = Quaternion.AngleAxis(initialAngle + spanTime * selfRotationSpeed, Vector3.up) * Vector3.right * (selfRadius * progress * progress);
      } else if (totalTime <= destroyTime_) {
        spirit_.localScale = Vector3.zero;
      } else {
        Destroy();
      }
      totalTime += dt;
    }
  }
}
