using System;
using Donut;
using UnityEngine;

namespace Lib {
  public class Witch : DonutBehaviour {
    protected override void OnStart() {
      //playerInput_.Player.Move.performed += context => Debug.Log($"{context.ReadValue<Vector2>()}");
    }
    protected override void OnUpdate() {
      var player = PlayerInput.Player;
      var move = player.Move.ReadValue<Vector2>() * Time.deltaTime * 7;
      var trans = transform;
      var pos = trans.position;
      pos.x += move.x;
      pos.y += move.y;
      trans.position = pos;
      if (player.BackClock.triggered) {
        Debug.Log("Back");
      }
    }

    private void OnBecameVisible() {
      Debug.Log("Visible");
    }
  }
}