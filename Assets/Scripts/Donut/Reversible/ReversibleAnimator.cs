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
      public int hash;
      public float time;
    }

    private Dense<LayerState>[] layers_;
    private void Start() {
      var clockObj = GameObject.FindGameObjectWithTag("Clock");
      clock_ = clockObj.GetComponent<ClockHolder>().Clock;
      animator_ = gameObject.GetComponent<Animator>();
      bornAt_ = clock_.CurrentTick;

      var layerCount = animator_.layerCount;
      layers_ = new Dense<LayerState>[layerCount];
      for (var i = 0; i < layerCount; i++) {
        var info = animator_.GetCurrentAnimatorStateInfo(i);
        layers_[i] = new Dense<LayerState>(clock_, new LayerState {
          hash = info.shortNameHash,
          time = info.normalizedTime,
        });
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
          ref var layer = ref layers_[i].Mut;
          layer.hash = info.shortNameHash;
          layer.time = info.normalizedTime;
        }
      } else {
        var layerCount = animator_.layerCount;
        for (var i = 0; i < layerCount; i++) {
          ref readonly var layer = ref layers_[i].Ref;
          animator_.Play(layer.hash, i, layer.time);
        }
      }
    }
  }
}
