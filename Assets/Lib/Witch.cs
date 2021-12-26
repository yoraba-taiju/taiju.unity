using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

namespace Lib {
  public class Witch : MonoBehaviour {
    private PlayerInput playerInput_;
    private void Start() {
      playerInput_ = new PlayerInput();
      playerInput_.Enable();
      //playerInput_.Player.Move.performed += context => Debug.Log($"{context.ReadValue<Vector2>()}");
    }

    // Update is called once per frame
    private void Update() {
      var move = playerInput_.Player.Move.ReadValue<Vector2>() * Time.deltaTime * 7;
      var trans = transform;
      var pos = trans.position;
      pos.x += move.x;
      pos.y += move.y;
      trans.position = pos;
    }
  }
}