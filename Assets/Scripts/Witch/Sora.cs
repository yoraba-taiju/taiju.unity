using Donut.Unity;
using UnityEngine;

namespace Witch {
  public class Sora : DonutBehaviour {
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
    }

    private void OnBecameVisible() {
      // https://forum.unity.com/threads/check-if-gameobject-in-visible-on-screen.424586/
      Debug.Log("Visible");
    }
  }
}