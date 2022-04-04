using System;
using Reversible.Unity;
using Reversible.Value;
using UnityEngine;

namespace Reversible.Companion {
  public struct ParticleSystem: ICompanion {
    private readonly UnityEngine.ParticleSystem particleSystem_;

    private struct Record {
      public float time;
      public int count;
      public UnityEngine.ParticleSystem.Particle[] particles;
    }
    private Dense<Record> record_;

    private static void CloneRecord(ref Record dst, in Record src) {
      dst.time = src.time;
      dst.count = src.count;
      if (dst.particles == null) {
        dst.particles = new UnityEngine.ParticleSystem.Particle[src.particles.Length];
      } else if (dst.particles.Length < src.particles.Length) {
        dst.particles = new UnityEngine.ParticleSystem.Particle[src.particles.Length];
      }
      Array.Copy(src.particles, dst.particles, src.count);
    }

    public ParticleSystem(ClockHolder holder, UnityEngine.ParticleSystem particleSystem) {
      particleSystem_ = particleSystem;
      var particles = new UnityEngine.ParticleSystem.Particle[particleSystem.main.maxParticles];
      var count = particleSystem_.GetParticles(particles);

      record_ = new Dense<Record>(holder.Clock, CloneRecord, new Record {
        time = particleSystem.time,
        count = count,
        particles = particles,
      });
    }
    
    public void OnTick() {
      ref var record = ref record_.Mut;
      record.time = particleSystem_.time;
      record.count = particleSystem_.GetParticles(record.particles);
      if (particleSystem_.isPaused) {
        particleSystem_.Play();
      }
    }
    public void OnBack() {
      ref readonly var record = ref record_.Ref;
      particleSystem_.time = record.time;
      particleSystem_.SetParticles(record.particles, record.count, 0);
      if (particleSystem_.isPlaying) {
        particleSystem_.Pause();
      }
    }
    public void OnLeap() {
    }
  }
}