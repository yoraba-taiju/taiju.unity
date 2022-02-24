using System.Linq;
using Donut.Unity;
using Donut.Values;
using UnityEngine;
using UnityEngine.Playables;

namespace Donut.Reversible {
  public class ReversiblePlayable: MonoBehaviour {
    // Clock
    private Clock clock_;
    private PlayableDirector playableDirector_;
    private uint bornAt_;

    // Transform records
    private Dense<double> time_;
    private void Start() {
      clock_ = GameObject.FindGameObjectWithTag("Clock").GetComponent<ClockHolder>().Clock;
      bornAt_ = clock_.CurrentTick;
      playableDirector_ = gameObject.GetComponent<PlayableDirector>();
      time_ = new Dense<double>(clock_, playableDirector_.time);
    }

    private void Update() {
      if (bornAt_ > clock_.CurrentTick) {
        Destroy(gameObject);
        return;
      }
      if (clock_.IsTicking) {
        time_.Value = playableDirector_.time;
      } else {
        // Debug.Log($"Back: {clock_.CurrentTick}: {playableDirector_.time} -> {time_.Value}");
        // time_.Debug();
        playableDirector_.time = time_.Value;
      }
    }
  }
}
