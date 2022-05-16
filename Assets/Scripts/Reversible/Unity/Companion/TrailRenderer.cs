using System;
using Reversible.Value;
using UnityEngine;

namespace Reversible.Unity.Companion {
  public struct TrailRenderer: ICompanion {
    private readonly UnityEngine.TrailRenderer trailRenderer_;
    private int numMaxPositions_;

    private struct Record {
      public int count;
      public Vector3[] positions;
    }
    private Dense<Record> record_;

    private static void CloneRecord(ref Record dst, in Record src) {
      dst.count = src.count;
      if (dst.positions == null) {
        dst.positions = new Vector3[src.positions.Length];
      } else if (dst.positions.Length < src.positions.Length) {
        dst.positions = new Vector3[src.positions.Length];
      }
      Array.Copy(src.positions, dst.positions, src.count);
    }

    public TrailRenderer(ClockHolder holder, UnityEngine.TrailRenderer trailRenderer) {
      trailRenderer_ = trailRenderer;
      numMaxPositions_ = Math.Max(256, trailRenderer.positionCount);

      var positions = new Vector3[numMaxPositions_];
      var count = trailRenderer.GetPositions(positions);

      record_ = new Dense<Record>(holder.Clock, CloneRecord, new Record {
        count = count,
        positions = positions,
      });
    }
    
    public void OnTick() {
      ref var record = ref record_.Mut;
      var count = trailRenderer_.positionCount;
      numMaxPositions_ = Math.Max(numMaxPositions_, count);
      if (count < record.positions.Length) {
        record.count = trailRenderer_.GetPositions(record.positions);
      } else {
        record.positions = new Vector3[numMaxPositions_];
        record.count = trailRenderer_.GetPositions(record.positions);
      }
    }
    public void OnBack() {
      ref readonly var record = ref record_.Ref;
      trailRenderer_.Clear();
      trailRenderer_.AddPositions(record.positions[..record.count]);
    }
    public void OnLeap() {
    }
  }
}