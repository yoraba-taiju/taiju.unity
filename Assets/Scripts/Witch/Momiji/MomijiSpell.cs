using Effect;
using Reversible.Unity;
using UnityEngine;

namespace Witch.Momiji {
  public class MomijiSpell : ReversibleBehaviour {
    [SerializeField] private GameObject spiritPrefab;
    [SerializeField] private MomijiSpellRing ringPrefab;
    private readonly MomijiSpellRing[] rings_ = new MomijiSpellRing[2];

    private Transform field_;
    private float startAt_;
    [SerializeField] private float duration = 4.5f;

    protected override void OnStart() {
      field_ = GameObject.FindWithTag("Field").transform;
      startAt_ = IntegrationTime;
      for (var i = 0; i < MagicElement.Colors.Length; ++i) {
        var spirit = Instantiate(spiritPrefab, transform.localPosition, Quaternion.identity, field_);
        var fairy = spirit.GetComponent<FairyOfLight>();
        fairy.color = MagicElement.Colors[i];
        var rot = Quaternion.Euler(Random.Range(-180.0f, 180.0f), Random.Range(-180.0f, 180.0f), Random.Range(-180.0f, 180.0f));
        fairy.poleRotationAxis = rot * Vector3.right;
        fairy.initialAngle = Random.Range(-180.0f, 180.0f);
        fairy.initialPoleAngle = Random.Range(-180.0f, 180.0f);
        fairy.arrowLaunchDelay = i / 7.0f * fairy.arrowLaunchInterval + 53f / 113f;
      }

      for (var i = 0; i < rings_.Length; ++i) {
        rings_[i] = Instantiate(ringPrefab, transform.localPosition, Quaternion.identity, field_);
        rings_[i].duration = duration;
      }
    }

    protected override void OnForward() {
      var startFrom = IntegrationTime - startAt_;
      if (startFrom < duration) {
        return;
      }

      foreach (var ring in rings_) {
        ring.Deactivate();
      }
      Deactivate();
    }

    protected override void OnReverse() {
    }

    protected override void OnLeap() {
    }
  }
}