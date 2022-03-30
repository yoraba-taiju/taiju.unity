using System;
using Donut.Unity;
using UnityEngine;

namespace Witch {
  public class Sora : DonutBehaviour {
    [SerializeField] public GameObject bullet;
    private GameObject field_;
    protected override void OnStart() {
      //playerInput_.Player.Move.performed += context => Debug.Log($"{context.ReadValue<Vector2>()}");
      field_ = GameObject.FindGameObjectWithTag("Field");
    }
    protected override void OnForward() {
      var player = playerInput.Player;
      var move = player.Move.ReadValue<Vector2>() * Time.deltaTime * 7;
      var trans = transform;
      var pos = trans.position;
      pos.x += move.x;
      pos.y += move.y;
      trans.position = pos;
      if (player.Fire.triggered) {
        Fire();
      }
    }

    private void Fire() {
      var b = Instantiate(bullet, field_.transform);
      var bt = b.transform;
      bt.localPosition = transform.localPosition + Vector3.right * 1f;
      bt.rotation = Quaternion.identity;
      b.GetComponent<Rigidbody2D>().AddForce(Vector2.right * 15, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D col) {
      Debug.Log("Collided");
    }
  }
}