using UnityEngine;

namespace Reversible.Unity.Specialized {
  public abstract class SpecializedReversibleComponentBehaviour: RawReversibleBehaviour {
    private uint bornAt_;
    
    protected new void Start() {
      var self = this as RawReversibleBehaviour;
      self.Start();
      bornAt_ = clock.CurrentTick;
    }
    public new void Update() {
      if (bornAt_ > clock.CurrentTick) {
        Destroy(gameObject);
        return;
      }
      var self = this as RawReversibleBehaviour;
      self.Update();
    }
  }
}