using System.Linq;
using Donut.Values;
using UnityEngine;
using UnityEngine.Playables;

namespace Donut.Unity {
  public class PlayableWithHistory: MonoBehaviour {
    // Clock
    private Clock clock_;
    private PlayableDirector playableDirector_;
    private uint BornAt { get; set; }

    // Transform records
    private Dense<double> time_;
    private void Start() {
      clock_ = GameObject.FindGameObjectWithTag("Clock").GetComponent<ClockHolder>().Clock;
      BornAt = clock_.CurrentTick;
      playableDirector_ = gameObject.GetComponent<PlayableDirector>();
      time_ = new Dense<double>(clock_, playableDirector_.time);
    }

    private void Update() {
      if (BornAt > clock_.CurrentTick) {
        Destroy(gameObject);
        return;
      }
      { // Manage transforms
        var trans = transform;
        if (clock_.IsTicking) {
          time_.Value = playableDirector_.time;
        } else {
          playableDirector_.time = time_.Value;
        }
      }
    }
  }
}
