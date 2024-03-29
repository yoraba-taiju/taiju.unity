﻿using UnityEngine;

namespace Effect {
  public class MagicElementEmitter: MonoBehaviour {
    [SerializeField] private MagicElement magicElementPrefab;
    [SerializeField] private float explosionVelocity = 50.0f;
    [SerializeField] public int numMagicElements = 10;
    [SerializeField] public float defaultPeriod = 0.5f;
    [SerializeField] public float periodRandom = 0.1f;
    public void Start() {
      var field = GameObject.FindGameObjectWithTag("Field").transform;
      var trans = transform;
      for (var i = 0; i < numMagicElements; ++i) {
        var color = MagicElement.Colors[Random.Range(0, MagicElement.Colors.Length)];
        var velocity = Quaternion.Euler(0f, 0f, Random.Range(-180.0f, 180.0f)) * (Vector3.right * explosionVelocity);
        var magicElement = Instantiate(magicElementPrefab, trans.localPosition, Quaternion.identity, field);
        magicElement.color = color;
        magicElement.period = Random.Range(-periodRandom, periodRandom) + defaultPeriod;
        magicElement.initialVelocity = velocity;
      }
      Destroy(gameObject);
    }
  }
}
