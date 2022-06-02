using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class Brick : MonoBehaviour
{
    [HideInInspector]
    public BoxCollider Collider;
    [HideInInspector]
    public LODGroup BrickMeshes;

    public bool CurrentBrick = true;

    //public Player player;

    private int collisionCount = 0;
   
    public bool IsNotColliding {
        get { return collisionCount == 0; }
    }

    public void Awake()
    {
        Collider = GetComponent<BoxCollider>();
        BrickMeshes = GetComponent<LODGroup>();
        //player = GetComponent(typeof(Player)) as Player;
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

    //void OnCollisionEnter(Collision other)
    //{
    //    Debug.Log("collision enter");
    //    player.GetComponent<PlaceBrick>().colliding = true;
    //}

    //void OnCollisionStay(Collision other)
    //{
    //    Debug.Log("collision stay");
    //    if (CurrentBrick)
    //    {
    //        player.GetComponent<PlaceBrick>().colliding = true;
    //    }
    //}

    //void OnCollisionExit(Collision other)
    //{
    //    Debug.Log("collision exit");
    //    if (CurrentBrick)
    //    {
    //        player.GetComponent<PlaceBrick>().colliding = false;
    //    }
    //}

    //void OnCollisionEnter(Collision col)
    //{
    //    collisionCount++;
    //}

    //void OnCollisionExit(Collision col)
    //{
    //    collisionCount--;
    //    if (collisionCount < 0) {
    //        collisionCount = 0;
    //    }
    //}
}
