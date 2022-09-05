using System;
using System.Collections.Generic;
using Enemy;
using Lib;
using UnityEngine;

namespace Reversible.Unity {
  public sealed class World : MonoBehaviour {
    /* Clock */
    private Clock clock_;
    private Player player_;

    /* GameObject Management */
    public HashSet<EnemyBehaviour> LivingEnemies { get; } = new();
    private RingBuffer<(uint, ReversibleBase)> deactivated_ = new (16384);

    private struct LayerName {
      public static readonly int Enemy = LayerMask.NameToLayer("Enemy");
    }

    private void Start() {
      player_ = gameObject.GetComponent<Player>();
      clock_ = player_.Clock;
    }

    private void Update() {
      if (player_.Ticked) {
        DestroyDeactivated(clock_.CurrentTick);
      } else if (player_.Backed) {
        RestoreDeactivated(clock_.CurrentTick);
      }
    }

    private void DestroyDeactivated(uint currentTick) {
      while (deactivated_.Count > 0) {
        var (destroyedAt, rev) = deactivated_.First;
        if (destroyedAt + Clock.HISTORY_LENGTH < currentTick) {
          deactivated_.RemoveFirst();
          Destroy(rev.gameObject);
        } else {
          break;
        }
      }
    }

    private void RestoreDeactivated(uint currentTick) {
      while (deactivated_.Count > 0) {
        var (destroyedAt, rev) = deactivated_.Last;
        if (destroyedAt >= currentTick) {
          //Debug.Log($"Current: {currentTick} Restored: {destroyedAt}");
          deactivated_.RemoveLast();
          rev.gameObject.SetActive(true);
          rev.OnReactivated();
        } else {
          break;
        }
      }
    }

    public void RegisterEnemy(EnemyBehaviour enemy) {
      LivingEnemies.Add(enemy);
    }
    public void UnregisterEnemy(EnemyBehaviour enemy) {
      LivingEnemies.Remove(enemy);
    }
    public void Deactivate(ReversibleBase rev) {
      var obj = rev.gameObject;
      if (!obj.activeSelf) {
        return;
      }
      rev.OnDeactivated();
      obj.SetActive(false);
      deactivated_.AddLast((clock_.CurrentTick, rev));
    }
  }
}