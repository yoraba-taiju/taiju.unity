using System.Runtime.CompilerServices;
using UnityEngine;

namespace Reversible.Unity.ReversibleGameObject {
  public abstract class ReversibleGameObject : MonoBehaviour {
    protected Player player;
    protected World world;
    protected Clock clock;
    private uint bornAt_;

    protected float IntegrationTime {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      get => player.IntegrationTime;
    }

    protected abstract void OnStart();

    public void Start() {
      var backstage = GameObject.FindGameObjectWithTag("Backstage");
      player = backstage.GetComponent<Player>();
      world = backstage.GetComponent<World>();
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