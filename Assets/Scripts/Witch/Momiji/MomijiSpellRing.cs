using Reversible.Unity;
using UnityEngine;

namespace Witch.Momiji {
  public class MomijiSpellRing: ReversibleBehaviour {
    private float startAt_;
    private Vector3 pole_;
    [SerializeField] private float angularVelocity = 230f;
    [SerializeField] private float poleAngularVelocity = 230f;
    [SerializeField] public float duration;
    protected override void OnStart() {
      startAt_ = IntegrationTime;
      pole_ = Quaternion.Euler(Random.Range(-180, 180), Random.Range(-180, 180), Random.Range(-180, 180)) * Vector3.right;
    }

    protected override void OnForward() {
      var startFrom = IntegrationTime - startAt_;
      var trans = transform;
      float scale;
      if (startFrom <= duration/2.0f) {
        scale = Mathf.Min(Mathf.Pow(startFrom ,2.0f) * 6.0f, 6.0f);
      } else {
        var t = duration - startFrom;
        scale = Mathf.Min(Mathf.Pow(t ,2.0f) * 3.0f, 6.0f);
      }
      trans.localScale  = new Vector3(scale, scale, 2.0f);
      trans.localRotation = Quaternion.AngleAxis(startFrom * poleAngularVelocity, pole_) *
                            Quaternion.AngleAxis(startFrom * angularVelocity, Vector3.up);
    }
  }
}