using System;
using Donut.Unity;
using UnityEngine;

namespace Witch {
  public class Sora : DonutBehaviour {
    protected override void OnStart() {
      //playerInput_.Player.Move.performed += context => Debug.Log($"{context.ReadValue<Vector2>()}");
    }
    protected override void OnForward() {
      var player = playerInput.Player;
      var move = player.Move.ReadValue<Vector2>() * Time.deltaTime * 7;
      var trans = transform;
      var pos = trans.position;
      pos.x += move.x;
      pos.y += move.y;
      trans.position = pos;
    }

    private void OnTriggerEnter2D(Collider2D col) {
      Debug.Log("Collided");
    }
  }
}