using System;
using System.Collections.Generic;
using Enemy;
using UnityEngine;

namespace Reversible.Unity {
  public sealed class World : MonoBehaviour {
    /* Clock */
    private Clock clock_;
    private ClockController clockController_;

    /* GameObject Management */
    public HashSet<Transform> LivingEnemies { get; } = new();
    private readonly LinkedList<Tuple<uint, GameObject>> graveYard_ = new();

    private struct LayerName {
      public static readonly int Enemy = LayerMask.NameToLayer("Enemy");
    }

    private void Start() {
      clockController_ = gameObject.GetComponent<ClockController>();
      clock_ = clockController_.Clock;
    }

    private void Update() {
      if (clockController_.Ticked) {
        RemoveOutdated(clock_.CurrentTick);
      } else if (clockController_.Backed) {
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
          if (obj.layer == LayerName.Enemy) {
            LivingEnemies.Add(obj.transform);
          }

          obj.SetActive(true);
        } else {
          break;
        }
      }
    }

    public void RegisterEnemy(Transform enemyTransform) {
      LivingEnemies.Add(enemyTransform);
    }

    public void Destroy(GameObject obj) {
      LivingEnemies.Remove(obj.transform);
      graveYard_.AddLast(new Tuple<uint, GameObject>(clock_.CurrentTick, obj));
      obj.SetActive(false);
    }
  }
}