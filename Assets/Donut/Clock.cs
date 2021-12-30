using System;
using Lib;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Donut {
  public class Clock : MonoBehaviour {
    private const uint RECORD_LENGTH = 600;

    /* player info */
    private PlayerInput playerInput_;
    private PlayableDirector playableDirector_;
    
    /* current leaps */
    private uint currentLeaps_ = 0;
    private uint currentTicks_ = 0;

    /* Timeline management */
    private readonly double[] times_ = new double[RECORD_LENGTH];
    private uint minTimes_ = 0;

    private void Start() {
      playableDirector_ = gameObject.GetComponent<PlayableDirector>();
      playerInput_ = new PlayerInput();
      playerInput_.Enable();
    }

    private void Update() {
      var player = playerInput_.Player;
      var backPressed = player.BackClock;
      var isLeaping = backPressed.IsPressed();
      if (isLeaping) {
        if (backPressed.triggered) {
          currentLeaps_++;
          Debug.Log("backed");
        }
        if (currentTicks_ > 0) {
          currentTicks_--;
        }
        playableDirector_.time = times_[currentTicks_ % RECORD_LENGTH];
      } else {
        times_[currentTicks_ % RECORD_LENGTH] = playableDirector_.time;
        
        currentTicks_++;
      }
    }

    public PlayerInput PlayerInput => playerInput_;
    public uint CurrentTicks => currentTicks_;
    public uint CurrentLeaps => currentLeaps_;
  }
}
