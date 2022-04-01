using System;
using System.Collections.Generic;
using UnityEngine;

namespace Donut.Unity {
  public class Graveyard : MonoBehaviour {
    /* Clock */
    private Clock clock_;
    private ClockHolder clockHolder_;
    private readonly LinkedList<Tuple<uint, GameObject>> objects_ = new();

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
      while (objects_.Count > 0) {
        var (destroyedAt, obj) = objects_.First.Value;
        if (destroyedAt + Clock.HISTORY_LENGTH < currentTick) {
          objects_.RemoveFirst();
          MonoBehaviour.Destroy(obj);
        } else {
          break;
        }
      }
    }
    private void RestoreOutdated(uint currentTick) {
      while (objects_.Count > 0) {
        var (destroyedAt, obj) = objects_.Last.Value;
        if (destroyedAt >= currentTick) {
          objects_.RemoveLast();
          obj.SetActive(true);
        } else {
          break;
        }
      }
    }

    public void Destroy(GameObject obj) {
      objects_.AddLast(new Tuple<uint, GameObject>(clock_.CurrentTick, obj));
      obj.SetActive(false);
    }
  }
}