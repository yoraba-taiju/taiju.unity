using System.Runtime.CompilerServices;
using UnityEngine;

namespace Reversible.Unity.Specialized {
  public abstract class SpecializedReversibleComponentBehaviour : MonoBehaviour {
    private uint bornAt_;
    protected Player player;
    protected World world;
    protected Clock clock;

    protected float CurrentTime {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      get => player.IntegrationTime;
    }

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
      if (player.Ticked) {
        OnTick();
      } else if (player.Backed) {
        OnBack();
      } else if (player.Leaped) {
        OnLeap();
      }
    }

    protected abstract void OnStart();

    protected abstract void OnTick();

    protected abstract void OnBack();

    protected abstract void OnLeap();
  }
}