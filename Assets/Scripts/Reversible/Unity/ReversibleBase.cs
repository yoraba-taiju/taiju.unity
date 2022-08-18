using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Reversible.Unity {
  public abstract class ReversibleBase : MonoBehaviour {
    protected Player player;
    protected World world;
    protected Clock clock;
    private uint bornAt_;

    protected float IntegrationTime {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      get => player.IntegrationTime;
    }
    protected PlayerInput Input {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      get => player.Input;
    }
    
    protected void Awake() {
      var backstage = GameObject.FindGameObjectWithTag("Backstage");
      player = backstage.GetComponent<Player>();
      world = backstage.GetComponent<World>();
      clock = player.Clock;
    }
    
    protected void Start() {
      bornAt_ = clock.CurrentTick;
    }

    private void Update() {
      if (bornAt_ > clock.CurrentTick) {
        Destroy(gameObject);
        return;
      }
      OnUpdate();
    }

    public void Deactivate() {
      world.Deactivate(this);
    }

    // functions
    protected abstract void OnUpdate();
    public abstract void OnDeactivated();
    public abstract void OnReactivated();
  }
}