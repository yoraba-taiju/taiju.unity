using Reversible.Unity;
using Reversible.Value;
using UnityEngine;

namespace Witch.Paper {
  public class Paper : ReversibleBehaviour {
    private readonly Mesh[] meshes_ = new Mesh[4];
    private MeshFilter meshFilter_;
    private MeshRenderer meshRenderer_;
    private Dense<float> timeToChange_;
    private Dense<int> meshIdx_;
    [SerializeField] private Vector2 minBound;
    [SerializeField] private Vector2 maxBound;

    protected override void OnStart() {
      meshFilter_ = GetComponent<MeshFilter>();
      meshRenderer_ = GetComponent<MeshRenderer>();
      meshRenderer_.sortingOrder = -2;
      timeToChange_ = new Dense<float>(clock, 0.2f);
      meshIdx_ = new Dense<int>(clock, 0);
      for (var i = 0; i < 4; ++i) {
        var mesh = new Mesh();
        var numTriangle = Random.Range(3, 4 + 1);
        var numVertex = numTriangle + 1;
        var vertices = new Vector3[numVertex];
        var uv = new Vector2[numVertex];
        var triangles = new int[numTriangle * 3];
        var phase = Random.Range(-Mathf.PI, Mathf.PI);
        var lastIdx = numVertex - 1;
        vertices[lastIdx] = Vector3.zero;
        uv[lastIdx] = Vector2.one / 2.0f;
        for (var j = 0; j <= numTriangle; ++j) {
          var angle = phase + Random.Range(-0.01f, 0.01f) + (Mathf.PI * 2.0f * j / numVertex);
          var size = new Vector2(Random.Range(minBound.x, maxBound.x), Random.Range(minBound.y, maxBound.y));
          var pos = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
          vertices[j] = pos * size;
          uv[j] = (pos + Vector2.one) / 2.0f;
        }

        for (var j = 0; j < numTriangle; ++j) {
          triangles[(j*3) + 0] = (j + 1) % numVertex;
          triangles[(j*3) + 1] = (j + 0) % numVertex;
          triangles[(j*3) + 2] = lastIdx;
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        mesh.Optimize();
        meshes_[i] = mesh;
      }
      meshFilter_.mesh = meshes_[0];
    }

    protected override void OnForward() {
      ref var timeToChange = ref timeToChange_.Mut;
      timeToChange -= Time.deltaTime;
      if (timeToChange > 0) {
        return;
      }
      timeToChange += 0.2f;
      ref var meshIdx = ref meshIdx_.Mut;
      meshIdx++;
      meshFilter_.mesh = meshes_[meshIdx % meshes_.Length];
    }

    protected override void OnReverse() {
      ref var meshIdx = ref meshIdx_.Mut;
      meshFilter_.mesh = meshes_[meshIdx % meshes_.Length];
    }
  }
}