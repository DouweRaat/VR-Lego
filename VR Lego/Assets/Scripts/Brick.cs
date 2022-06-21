using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

[ExecuteInEditMode]
public class Brick : MonoBehaviour
{
    [HideInInspector]
    public BoxCollider Collider;
    [HideInInspector]
    public LODGroup BrickMeshes;

    //private int collisionCount = 0;
   
    //public bool IsNotColliding {
    //    get { return collisionCount == 0; }
    //}

    public void Awake()
    {
        Collider = GetComponent<BoxCollider>();
        BrickMeshes = GetComponent<LODGroup>();
    }

    public void SetMaterial(Material mat)
    {
        var lods = BrickMeshes.GetLODs();
        for (var i = 0; i < lods.Length; i++)
        {
            for (var j = 0; j < lods[i].renderers.Length; j++)
            {
                lods[i].renderers[j].material = mat;
            }
        }
    }

    //void OnCollisionEnter(Collision col)
    //{
    //    collisionCount++;
    //}

    //void OnCollisionExit(Collision col)
    //{
    //    collisionCount--;
    //    if (collisionCount < 0)
    //    {
    //        collisionCount = 0;
    //    }
    //}

    //public void NoRaycastLayer()
    //{
    //    int layerIgnoreRaycast = LayerMask.NameToLayer("Ignore Raycast");
    //    gameObject.layer = layerIgnoreRaycast;
    //}

    //public void LegoLayer()
    //{
    //    int layerLego = LayerMask.NameToLayer("Lego");
    //    gameObject.layer = layerLego;
    //}
}
