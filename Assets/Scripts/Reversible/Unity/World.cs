using System;
using System.Collections.Generic;
using UnityEngine;

namespace Reversible.Unity {
  public class World: MonoBehaviour {
    /* Clock */
    private Clock clock_;
    private ClockHolder clockHolder_;
    private readonly HashSet<Transform> livingObjects_ = new();
    private readonly LinkedList<Tuple<uint, GameObject>> deadObjects_ = new();

    public HashSet<Transform> LivingObjects => livingObjects_;

    private void Start() {
      clockHolder_ = gameObject.GetComponent<ClockHolder>();
      clock_ = clockHolder_.Clock;
    }

    private void Update() {
      if (clockHolder_.Ticked) {
        RemoveOutdated(clock_.CurrentTick);
      } else if(clockHolder_.Backed) {
        RestoreOutdated(clock_.CurrentTick);
      }
    }

    private void RemoveOutdated(uint currentTick) {
      while (deadObjects_.Count > 0) {
        var (destroyedAt, obj) = deadObjects_.First.Value;
        if (destroyedAt + Clock.HISTORY_LENGTH < currentTick) {
          deadObjects_.RemoveFirst();
          MonoBehaviour.Destroy(obj);
        } else {
          break;
        }
      }
    }

    private void RestoreOutdated(uint currentTick) {
      while (deadObjects_.Count > 0) {
        var (destroyedAt, obj) = deadObjects_.Last.Value;
        if (destroyedAt >= currentTick) {
          deadObjects_.RemoveLast();
          livingObjects_.Add(obj.transform);
          obj.SetActive(true);
        } else {
          break;
        }
      }
    }

    public void Register(GameObject obj) {
      livingObjects_.Add(obj.transform);
    }

    public void Destroy(GameObject obj) {
      livingObjects_.Remove(obj.transform);
      deadObjects_.AddLast(new Tuple<uint, GameObject>(clock_.CurrentTick, obj));
      obj.SetActive(false);
    }
  }
}
