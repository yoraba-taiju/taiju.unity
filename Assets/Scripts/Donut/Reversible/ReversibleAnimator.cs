using System;
using Donut.Unity;
using Donut.Values;
using UnityEngine;

namespace Donut.Reversible {
  public class ReversibleAnimator: MonoBehaviour {
    // Clock
    private ClockHolder holder_;
    private Clock clock_;
    private uint bornAt_;
    private Animator animator_;

    // Layer states
    private struct LayerState {
      public int hash;
      public float time;
    }
    private Dense<LayerState>[] layers_;

    // Parameters
    private struct ParameterState {
      public float[] floatStates;
      public int[] intStates;
      public bool[] boolStates;
    }
    private AnimatorControllerParameter[] parameters_;
    private int[] parameterIdx_;
    private Dense<ParameterState>? params_;

    // Triggers
    private int[] triggers_;

    private void Start() {
      var clockObj = GameObject.FindGameObjectWithTag("Clock");
      holder_ = clockObj.GetComponent<ClockHolder>();
      clock_ = holder_.Clock;
      animator_ = gameObject.GetComponent<Animator>();
      bornAt_ = clock_.CurrentTick;

      {
        var layerCount = animator_.layerCount;
        layers_ = new Dense<LayerState>[layerCount];
        for (var i = 0; i < layerCount; i++) {
          var info = animator_.GetCurrentAnimatorStateInfo(i);
          layers_[i] = new Dense<LayerState>(clock_, new LayerState {
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
        var numFloat = 0;
        var numInt = 0;
        var numBool = 0;
        for (var i = 0; i < parameters_.Length; i++) {
          var p = parameters_[i];
          switch (p.type) {
            case AnimatorControllerParameterType.Float:
              parameterIdx_[i] = numFloat;
              numFloat++;
              break;
            case AnimatorControllerParameterType.Int:
              parameterIdx_[i] = numInt;
              numInt++;
              break;
            case AnimatorControllerParameterType.Bool:
              parameterIdx_[i] = numBool;
              numBool++;
              break;
            case AnimatorControllerParameterType.Trigger:
            default:
              break;
          }
        }
        var initial = new ParameterState {
          floatStates = numFloat > 0 ? new float[numFloat] : null,
          intStates = numInt > 0 ? new int[numInt] : null,
          boolStates = numBool > 0 ? new bool[numBool] : null,
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
        params_ = new Dense<ParameterState>(clock_, CloneParamState, initial);
      } else {
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

    private void Update() {
      if (bornAt_ > clock_.CurrentTick) {
        Destroy(gameObject);
        return;
      }
      if (holder_.Ticked) {
        if (triggers_ != null && holder_.Leaped) {
          foreach(var t in triggers_) {
            animator_.ResetTrigger(t);
          }
        }
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
      } else if (holder_.Backed) {
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
    }
  }
}
