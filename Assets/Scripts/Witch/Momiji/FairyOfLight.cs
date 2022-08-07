using Reversible.Unity;
using Reversible.Value;
using UnityEngine;

namespace Witch.Momiji {
  public class FairyOfLight : ReversibleBehaviour {
    [SerializeField] private Color color = new(1, 1,1, 1);
    [SerializeField] private Vector3 rotationAxis = Vector3.up;
    [SerializeField] private float initialAngle;
    [SerializeField] private float bornTime = 1.0f;
    [SerializeField] private float rotateTime = 7.0f;
    [SerializeField] private float endTime = 10.0f;
    private float destroyTime_;
    [SerializeField] private float poleRotationSpeed = 60.0f;
    [SerializeField] private float selfRotationSpeed = 180.0f;
    [SerializeField] private float selfRadius = 5.0f;
    private Dense<float> totalTime_;
    private Transform spirit_;
    private MeshRenderer spiritMeshRenderer_;
    private Light spiritLight_;
    private TrailRenderer spiritTrailRenderer_;

    protected override void OnStart() {
      var trans = transform;
      spirit_ = trans.Find("Spirit");
      {
        spiritMeshRenderer_ = spirit_.gameObject.GetComponent<MeshRenderer>();
        spiritMeshRenderer_.material.color = color;
      }
      {
        spiritTrailRenderer_ = spirit_.GetComponent<TrailRenderer>();
        var colorGradient = spiritTrailRenderer_.colorGradient;
        var colorKeys = colorGradient.colorKeys;
        colorKeys[1].color = color;
        colorGradient.colorKeys = colorKeys;
        spiritTrailRenderer_.colorGradient = colorGradient;
      }
      {
        spiritLight_ = spirit_.GetComponent<Light>();
        spiritLight_.color = color;
      }

      totalTime_ = new Dense<float>(clock, 0.0f);
      destroyTime_ = endTime + spiritTrailRenderer_.time;
    }

    protected override void OnForward() {
      var trans = transform;
      var dt = Time.deltaTime;
      ref var totalTime = ref totalTime_.Mut;
      if (totalTime <= bornTime) {
        var progress = totalTime / bornTime;
        progress *= progress;
        var spiritScale = 0.7f * progress;
        spirit_.localScale = new Vector3(spiritScale, spiritScale, spiritScale);
        trans.localRotation = Quaternion.AngleAxis(totalTime * poleRotationSpeed, rotationAxis);
        spirit_.localPosition = Quaternion.AngleAxis(initialAngle + totalTime * selfRotationSpeed, Vector3.forward) * Vector3.right * (selfRadius * progress);
      } else if (totalTime <= rotateTime) {
        trans.localRotation = Quaternion.AngleAxis(totalTime * poleRotationSpeed, rotationAxis);
        spirit_.localPosition = Quaternion.AngleAxis(initialAngle + totalTime * selfRotationSpeed, Vector3.forward) * Vector3.right * selfRadius;
      } else if (totalTime <= endTime){
        var spanTime = totalTime - rotateTime;
        var progress = (1 - spanTime / (endTime - rotateTime));
        progress *= progress;
        var spiritScale = 0.7f * progress;
        spirit_.localScale = new Vector3(spiritScale, spiritScale, spiritScale);
        trans.localRotation = Quaternion.AngleAxis(totalTime * poleRotationSpeed, rotationAxis);
        spirit_.localPosition = Quaternion.AngleAxis(initialAngle + totalTime * selfRotationSpeed, Vector3.forward) * Vector3.right * (selfRadius * progress);
      } else if (totalTime <= destroyTime_) {
        spirit_.localScale = Vector3.zero;
      } else {
        Destroy();
      }
      totalTime += dt;
    }
  }
}
