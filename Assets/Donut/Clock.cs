using System;
using Lib;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Donut {
  public class Clock : MonoBehaviour {
    private PlayerInput playerInput_;
    private const uint RECORD_LEN = 600;
    private uint leaps_;
    private uint current_;
    private PlayableDirector playableDirector_;
    
    private readonly double[] times_ = new double[RECORD_LEN];

    public PlayerInput PlayerInput => playerInput_;

    private void Start() {
      playableDirector_ = gameObject.GetComponent<PlayableDirector>();
      playerInput_ = new PlayerInput();
      playerInput_.Enable();
    }

    private void Update() {
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
