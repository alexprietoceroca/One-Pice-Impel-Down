using UnityEngine;

public class Colisones : MonoBehaviour
{
   void Start()
    {
        var mallas = GetComponentsInChildren<MeshFilter>(true);

        foreach (var mf in mallas)
        {
            if (mf.sharedMesh == null || mf.sharedMesh.vertexCount == 0)
                continue; 

            GameObject go = mf.gameObject;

            MeshCollider col = go.GetComponent<MeshCollider>();
            if (col == null) col = go.AddComponent<MeshCollider>();

            col.convex = false;
            col.cookingOptions = MeshColliderCookingOptions.CookForFasterSimulation;

            Rigidbody rb = go.GetComponent<Rigidbody>();
            if (rb != null) Destroy(rb);

            go.isStatic = true;
        }
    }
}