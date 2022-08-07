using System;
using Reversible.Unity;
using Reversible.Value;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Witch.Momiji {
  public class MomijiSpell: ReversibleBehaviour {
    [SerializeField] private GameObject spiritPrefab;

    private static readonly Color[] Colors = {
      Color.HSVToRGB(1.0f/7.0f, 0.3f, 1.0f),
      Color.HSVToRGB(2.0f/7.0f, 0.3f, 1.0f),
      Color.HSVToRGB(3.0f/7.0f, 0.3f, 1.0f),
      Color.HSVToRGB(4.0f/7.0f, 0.3f, 1.0f),
      Color.HSVToRGB(5.0f/7.0f, 0.3f, 1.0f),
      Color.HSVToRGB(6.0f/7.0f, 0.3f, 1.0f),
      Color.HSVToRGB(7.0f/7.0f, 0.3f, 1.0f),
    };
    private Transform field_;
    private Dense<float> totalTime_;
    private float duration_;
    protected override void OnStart() {
      totalTime_ = new Dense<float>(clock, 0.0f);
      field_ = GameObject.FindWithTag("Field").transform;
      for (var i = 0; i < 7; ++i) {
        var spirit = Instantiate(spiritPrefab, Vector3.zero, Quaternion.identity, field_);
        spirit.transform.localPosition = transform.localPosition;
        var fairy = spirit.GetComponent<FairyOfLight>();
        fairy.color = Colors[i];
        var rot = Quaternion.Euler(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));
        fairy.poleRotationAxis = rot * Vector3.left;
        fairy.initialAngle = Random.Range(0.0f, 360.0f);
        fairy.initialPoleAngle = Random.Range(0.0f, 360.0f);
        duration_ = Mathf.Max(duration_, fairy.Duration);
      }
    }

    protected override void OnForward() {
      ref var totalTime = ref totalTime_.Mut;
      if (totalTime < duration_) {
        totalTime += Time.deltaTime;
        return;
      }

      Destroy();
    }
  }
}