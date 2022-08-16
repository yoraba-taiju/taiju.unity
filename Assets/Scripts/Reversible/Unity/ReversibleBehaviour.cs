using System.Runtime.CompilerServices;
using UnityEngine;

namespace Reversible.Unity {
  public abstract class ReversibleBehaviour : MonoBehaviour {
    protected Player player;
    protected World world;
    protected Clock clock;

    protected float IntegrationTime {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      get => player.IntegrationTime;
    }
    protected PlayerInput Input {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      get => player.Input;
    }

    protected abstract void OnStart();
    protected abstract void OnForward();

    public void Start() {
      var clockObj = GameObject.FindGameObjectWithTag("Clock");
      player = clockObj.GetComponent<Player>();
      world = clockObj.GetComponent<World>();
      clock = player.Clock;
      OnStart();
    }

    protected virtual void OnReverse() {
    }

    protected virtual void OnLeap() {
    }

    public void Update() {
      if (player.Leaped) {
        OnLeap();
        return;
      }

      if (player.IsForwarding) {
        OnForward();
      } else {
        OnReverse();
      }
    }

    protected void Destroy() {
      world.Destroy(gameObject);
    }
    protected void Destroy(GameObject obj) {
      world.Destroy(obj);
    }
  }
}