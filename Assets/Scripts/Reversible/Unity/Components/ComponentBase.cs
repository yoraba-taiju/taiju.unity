using System.Runtime.CompilerServices;
using UnityEngine;

namespace Reversible.Unity.Components {
  public abstract class ComponentBase : MonoBehaviour {
    protected Player player;
    protected World world;
    protected Clock clock;
    private uint bornAt_;

    protected float CurrentTime {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      get => player.IntegrationTime;
    }

    protected abstract void OnStart();

    public void Start() {
      var stageOwner = GameObject.FindGameObjectWithTag("StageOwner");
      player = stageOwner.GetComponent<Player>();
      world = stageOwner.GetComponent<World>();
      clock = player.Clock;
      bornAt_ = clock.CurrentTick;
      OnStart();
    }

    public void Update() {
      if (bornAt_ > clock.CurrentTick) {
        Destroy(gameObject);
        return;
      }

      if (player.IsForwarding) {
        OnForward();
      } else {
        OnReverse();
      }
    }

    protected abstract void OnForward();
    protected abstract void OnReverse();
  }
}