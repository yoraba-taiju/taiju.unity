using System;
using System.Collections.Generic;
using Enemy;
using UnityEngine;

namespace Reversible.Unity {
  public sealed class World : MonoBehaviour {
    /* Clock */
    private Clock clock_;
    private Player player_;

    /* GameObject Management */
    public HashSet<EnemyBehaviour> LivingEnemies { get; } = new();
    private readonly LinkedList<Tuple<uint, GameObject>> graveYard_ = new();

    private struct LayerName {
      public static readonly int Enemy = LayerMask.NameToLayer("Enemy");
    }

    private void Start() {
      player_ = gameObject.GetComponent<Player>();
      clock_ = player_.Clock;
    }

    private void Update() {
      if (player_.Ticked) {
        RemoveOutdated(clock_.CurrentTick);
      } else if (player_.Backed) {
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
            LivingEnemies.Add(obj.transform.GetComponent<EnemyBehaviour>());
          }

          obj.SetActive(true);
        } else {
          break;
        }
      }
    }

    public void RegisterEnemy(EnemyBehaviour enemy) {
      LivingEnemies.Add(enemy);
    }
    public void Destroy(GameObject obj) {
      if (obj.layer == LayerName.Enemy) {
        LivingEnemies.Remove(obj.GetComponent<EnemyBehaviour>());
      }
      graveYard_.AddLast(new Tuple<uint, GameObject>(clock_.CurrentTick, obj));
      obj.SetActive(false);
    }
  }
}