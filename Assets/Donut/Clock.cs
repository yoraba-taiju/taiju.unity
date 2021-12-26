using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Donut {
  public class Clock : MonoBehaviour {
    const uint RECORD_LEN = 600;
    private uint leaps_;
    private uint current_;
    private PlayableDirector playableDirector_;
    
    private readonly double[] times_ = new double[RECORD_LEN];

    private void Start() {
      playableDirector_ = gameObject.GetComponent<PlayableDirector>();
    }

    private void Update() {
      this.
      tick();
    }

    public Clock() {
      this.leaps_ = 0;
      this.current_ = 0;
    }

    public void tick() {
      current_++;
    }

    public void back() {
      current_--;
    }
  }
}
