using System;
using System.Collections.Generic;
using UnityEngine;

namespace Reversible.Unity {
  public class World: MonoBehaviour {
    /* Clock */
    private Clock clock_;
    private ClockHolder clockHolder_;
    private readonly HashSet<Transform> livings_ = new();
    private readonly LinkedList<Tuple<uint, GameObject>> graveYard_ = new();

    public HashSet<Transform> Livings => livings_;

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
      while (graveYard_.Count > 0) {
        var (destroyedAt, obj) = graveYard_.First.Value;
        if (destroyedAt + Clock.HISTORY_LENGTH < currentTick) {
          graveYard_.RemoveFirst();
          MonoBehaviour.Destroy(obj);
        } else {
          break;
        }
      }
    }

    private void RestoreOutdated(uint currentTick) {
      while (graveYard_.Count > 0) {
        var (destroyedAt, obj) = graveYard_.Last.Value;
        if (destroyedAt >= currentTick) {
          graveYard_.RemoveLast();
          livings_.Add(obj.transform);
          obj.SetActive(true);
        } else {
          break;
        }
      }
    }

    public void Register(GameObject obj) {
      livings_.Add(obj.transform);
    }

    public void Destroy(GameObject obj) {
      livings_.Remove(obj.transform);
      graveYard_.AddLast(new Tuple<uint, GameObject>(clock_.CurrentTick, obj));
      obj.SetActive(false);
    }
  }
}
