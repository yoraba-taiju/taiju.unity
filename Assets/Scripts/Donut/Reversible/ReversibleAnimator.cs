using Donut.Unity;
using Donut.Values;
using UnityEngine;

namespace Donut.Reversible {
  public class ReversibleAnimator: MonoBehaviour {
    // Clock
    private Clock clock_;
    private uint bornAt_;
    private Animator animator_;

    // Animation records
    private struct LayerState {
      public Dense<int> hash;
      public Dense<float> time;
    };

    private LayerState[] layers_;
    private void Start() {
      var clockObj = GameObject.FindGameObjectWithTag("Clock");
      clock_ = clockObj.GetComponent<ClockHolder>().Clock;
      animator_ = gameObject.GetComponent<Animator>();
      bornAt_ = clock_.CurrentTick;

      var layerCount = animator_.layerCount;
      layers_ = new LayerState[layerCount];
      for (var i = 0; i < layerCount; i++) {
        var info = animator_.GetCurrentAnimatorStateInfo(i);
        layers_[i].hash = new Dense<int>(clock_, info.shortNameHash);
        layers_[i].time = new Dense<float>(clock_, info.normalizedTime);
      }
    } 

    private void Update() {
      if (bornAt_ > clock_.CurrentTick) {
        Destroy(gameObject);
        return;
      }
      if (clock_.IsTicking) {
        var layerCount = animator_.layerCount;
        for (var i = 0; i < layerCount; i++) {
          var info = animator_.GetCurrentAnimatorStateInfo(i);
          layers_[i].hash.Value = info.shortNameHash;
          layers_[i].time.Value = info.normalizedTime;
        }
      } else {
        var layerCount = animator_.layerCount;
        for (var i = 0; i < layerCount; i++) {
          animator_.Play(layers_[i].hash.Value, 0, layers_[i].time.Value);
        }
      }
    }
  }
}
