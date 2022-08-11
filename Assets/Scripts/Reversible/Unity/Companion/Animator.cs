using System;
using Reversible.Value;
using UnityEngine;

namespace Reversible.Unity.Companion {
  public readonly struct Animator: ICompanion {
    // Clock
    private readonly UnityEngine.Animator animator_;

    // Layer states
    private struct LayerState {
      public int hash;
      public float time;
    }
    private readonly Dense<LayerState>[] layers_;

    // Parameters
    private struct ParameterState {
      public float[] floatStates;
      public int[] intStates;
      public bool[] boolStates;
    }
    private readonly AnimatorControllerParameter[] parameters_;
    private readonly int[] parameterIdx_;
    private readonly Dense<ParameterState>? params_;

    // Triggers
    private readonly int[] triggers_;

    public Animator(ClockController clockController, UnityEngine.Animator animator) {
      var clock = clockController.Clock;

      animator_ = animator;
      {
        var layerCount = animator_.layerCount;
        layers_ = new Dense<LayerState>[layerCount];
        for (var i = 0; i < layerCount; i++) {
          var info = animator_.GetCurrentAnimatorStateInfo(i);
          layers_[i] = new Dense<LayerState>(clock, new LayerState {
            hash = info.shortNameHash,
            time = info.normalizedTime,
          });
        }
      }

      var parameters = animator_.parameters;
      var numValueParams = 0;
      var numTriggers = 0;
      foreach (var p in parameters) {
        if (p.type == AnimatorControllerParameterType.Trigger) {
          numTriggers++;
        } else {
          numValueParams++;
        }
      }
      if (numValueParams > 0) {
        parameters_ = new AnimatorControllerParameter[parameters.Length];
        parameterIdx_ = new int[parameters.Length];
        Array.Copy(parameters, parameters_, parameters.Length);
        Array.Sort(parameters_, (a, b) => a.nameHash - b.nameHash);
        var numFloats = 0;
        var numInts = 0;
        var numBooleans = 0;
        for (var i = 0; i < parameters_.Length; i++) {
          var p = parameters_[i];
          switch (p.type) {
            case AnimatorControllerParameterType.Float:
              parameterIdx_[i] = numFloats;
              numFloats++;
              break;
            case AnimatorControllerParameterType.Int:
              parameterIdx_[i] = numInts;
              numInts++;
              break;
            case AnimatorControllerParameterType.Bool:
              parameterIdx_[i] = numBooleans;
              numBooleans++;
              break;
            case AnimatorControllerParameterType.Trigger:
            default:
              break;
          }
        }

        var initial = new ParameterState {
          floatStates = numFloats > 0 ? new float[numFloats] : null,
          intStates = numInts > 0 ? new int[numInts] : null,
          boolStates = numBooleans > 0 ? new bool[numBooleans] : null,
        };
        for (var i = 0; i < parameters_.Length; i++) {
          var p = parameters_[i];
          switch (p.type) {
            case AnimatorControllerParameterType.Float:
              initial.floatStates[parameterIdx_[i]] = p.defaultFloat;
              break;
            case AnimatorControllerParameterType.Int:
              initial.intStates[parameterIdx_[i]] = p.defaultInt;
              break;
            case AnimatorControllerParameterType.Bool:
              initial.boolStates[parameterIdx_[i]] = p.defaultBool;
              break;
            case AnimatorControllerParameterType.Trigger:
            default:
              break;
          }
        }
        params_ = new Dense<ParameterState>(clockController.Clock, CloneParamState, initial);
      } else {
        parameters_ = null;
        parameterIdx_ = null;
        params_ = null;
      }
      if (numTriggers > 0) {
        triggers_ = new int[numTriggers];
        var triggerIdx = 0;
        foreach (var p in parameters) {
          if (p.type == AnimatorControllerParameterType.Trigger) {
            triggers_[triggerIdx] = p.nameHash;
            triggerIdx++;
          }
        }
      } else {
        triggers_ = null;
      }
    }

    private static void CloneParamState(ref ParameterState dst, in ParameterState src) {
      if (src.floatStates != null) {
        dst.floatStates ??= new float[src.floatStates.Length];
        Array.Copy(src.floatStates, dst.floatStates, src.floatStates.Length);
      }
      if (src.intStates != null) {
        dst.intStates ??= new int[src.intStates.Length];
        Array.Copy(src.intStates, dst.intStates, src.intStates.Length);
      }
      if (src.boolStates != null) {
        dst.boolStates = new bool[src.boolStates.Length];
        Array.Copy(src.boolStates, dst.boolStates, src.boolStates.Length);
      }
    }

    public void OnTick() {
      var layerCount = animator_.layerCount;
      for (var i = 0; i < layerCount; i++) {
        var info = animator_.GetCurrentAnimatorStateInfo(i);
        ref var layer = ref layers_[i].Mut;
        layer.hash = info.shortNameHash;
        layer.time = info.normalizedTime;
      }
      if (params_ != null) {
        for (var i = 0; i < parameters_.Length; i++) {
          var p = parameters_[i];
          ref var state = ref params_.Value.Mut;
          switch (p.type) {
            case AnimatorControllerParameterType.Float:
              state.floatStates[parameterIdx_[i]] = animator_.GetFloat(p.nameHash);
              break;
            case AnimatorControllerParameterType.Int:
              state.intStates[parameterIdx_[i]] = animator_.GetInteger(p.nameHash);
              break;
            case AnimatorControllerParameterType.Bool:
              state.boolStates[parameterIdx_[i]] = animator_.GetBool(p.nameHash);
              break;
            case AnimatorControllerParameterType.Trigger:
            default:
              break;
          }
        }
      }
    }

    public void OnBack() {
      var layerCount = animator_.layerCount;
      for (var i = 0; i < layerCount; i++) {
        ref readonly var layer = ref layers_[i].Ref;
        animator_.Play(layer.hash, i, layer.time);
      }
      if (params_ != null) {
        for (var i = 0; i < parameters_.Length; i++) {
          var p = parameters_[i];
          ref readonly var state = ref params_.Value.Ref;
          switch (p.type) {
            case AnimatorControllerParameterType.Float:
              animator_.SetFloat(p.nameHash, state.floatStates[parameterIdx_[i]]);
              break;
            case AnimatorControllerParameterType.Int:
              animator_.SetInteger(p.nameHash, state.intStates[parameterIdx_[i]]);
              break;
            case AnimatorControllerParameterType.Bool:
              animator_.SetBool(p.nameHash, state.boolStates[parameterIdx_[i]]);
              break;
            case AnimatorControllerParameterType.Trigger:
            default:
              break;
          }
        }
      }
    }
    
    public void OnLeap() {
      if (triggers_ == null) {
        return;
      }
      foreach(var t in triggers_) {
        animator_.ResetTrigger(t);
      }
    }
  }
}
