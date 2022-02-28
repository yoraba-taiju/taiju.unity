using System;
using Donut.Unity;
using Donut.Values;
using UnityEngine;

namespace Donut.Reversible {
  public class ReversibleParticle: MonoBehaviour {
    // Clock
    private Clock clock_;
    private uint bornAt_;
    private ParticleSystem particleSystem_;

    // Animation records
    private int maxParticles_;
    private Dense<Record> record_;

    private struct Record {
      public float time;
      public int count;
      public ParticleSystem.Particle[] particles;
    }

    private static void CloneRecord(ref Record dst, in Record src) {
      dst.time = src.time;
      dst.count = src.count;
      if (dst.particles == null) {
        dst.particles = new ParticleSystem.Particle[src.particles.Length];
      } else {
        if (dst.particles.Length < src.particles.Length) {
          dst.particles = new ParticleSystem.Particle[src.particles.Length];
        }
        Array.Copy(src.particles, dst.particles, src.particles.Length);
      }
    }

    private void Start() {
      var clockObj = GameObject.FindGameObjectWithTag("Clock");
      clock_ = clockObj.GetComponent<ClockHolder>().Clock;
      bornAt_ = clock_.CurrentTick;
      particleSystem_ = gameObject.GetComponent<ParticleSystem>();
      maxParticles_ = particleSystem_.main.maxParticles;
      
      var particles = new ParticleSystem.Particle[maxParticles_];
      var count = particleSystem_.GetParticles(particles);

      record_ = new Dense<Record>(clock_, CloneRecord, new Record {
        time = 0.0f,
        count = count,
        particles = particles,
      });
    } 

    private void Update() {
      if (bornAt_ > clock_.CurrentTick) {
        Destroy(gameObject);
        return;
      }
      if (clock_.IsTicking) {
        ref var record = ref record_.Value;
        record.time = particleSystem_.time;
        // FIXME: PERFORMANCE
        record.count = particleSystem_.GetParticles(record.particles);
        if (particleSystem_.isPaused) {
          particleSystem_.Play();
        }
      } else {
        ref var record = ref record_.Value;
        particleSystem_.time = record.time;
        particleSystem_.SetParticles(record.particles, record.count, 0);
        if (particleSystem_.isPlaying) {
          particleSystem_.Pause();
        }
      }
    }
  }

}