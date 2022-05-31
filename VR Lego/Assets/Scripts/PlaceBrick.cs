using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class PlaceBrick : MonoBehaviour
{
    public Brick[] PrefabBrick;
    public Material TransparentMat;
    public Material BrickMat;
    public Material[] materials;
    public Camera VRCamera;

    protected Brick CurrentBrick;
    protected bool PositionOk = true;
    private int color = 1;
    private int brick = 0;
    public Transform controller;

    public SteamVR_ActionSet actionSet;
    public SteamVR_Action_Boolean switchBrick;
    public SteamVR_Action_Boolean switchColor;
    public SteamVR_Action_Boolean placeBrick;
    public SteamVR_Action_Boolean rotateBrick;

    void Start()
    {
        SetNextBrick();
}

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G) || switchColor.GetStateDown(SteamVR_Input_Sources.Any))
        {
            color += 2;
            if (color > (materials.Length-1))
{
                color = 1;
            }
            if (CurrentBrick != null)
                CurrentBrick.GetComponent<Renderer>().material = materials[color + 1];
        }
        if (Input.GetKeyDown(KeyCode.H) || switchBrick.GetStateDown(SteamVR_Input_Sources.Any))
        {
            brick += 1;
            if (brick > (PrefabBrick.Length-1))
            {
                brick = 0;
            }
            if (CurrentBrick != null)
            {
                CurrentBrick.GetComponent<Renderer>().material = materials[0];

                var rot = CurrentBrick.transform.rotation;
                var pos = CurrentBrick.transform.position;
                GameObject.DestroyImmediate(CurrentBrick.gameObject);
                CurrentBrick = null;
                SetNextBrick();
                CurrentBrick.transform.rotation = rot;
                CurrentBrick.transform.position = pos;
            }
        }

        if (CurrentBrick != null)
        {
            //snap to grid
            //if (Physics.Raycast(controller.transform.position, controller.transform.forward, out var hitinfo, float.MaxValue, LegoLogic.LayerMaksLego))
            if (Physics.Raycast(VRCamera.transform.position, controller.transform.position, out var hitinfo, float.MaxValue, LegoLogic.LayerMaksLego))
            {
                var position = LegoLogic.SnapToGrid(hitinfo.point);
                //var position = LegoLogic.SnapToGrid(CurrentBrick.transform.position);
                //var position = (LegoLogic.SnapToGrid(controller.transform.position));
                var placeposition = position;
                PositionOk = false;

                if (Input.GetKeyDown(KeyCode.I))
                {
                    placeposition.x += LegoLogic.Grid.x;
                }
                if (Input.GetKeyDown(KeyCode.K))
                {
                    placeposition.x -= LegoLogic.Grid.x;
                }
                if (Input.GetKeyDown(KeyCode.L))
                {
                    placeposition.z += LegoLogic.Grid.z;
                }
                if (Input.GetKeyDown(KeyCode.J))
                {
                    placeposition.z -= LegoLogic.Grid.z;
                }
                if (Input.GetKeyDown(KeyCode.N))
                {
                    placeposition.y -= LegoLogic.Grid.y;
                }

                //for (int i = 0; i < 100; i++)
                //{
                //    var collider = Physics.OverlapBox(placeposition + CurrentBrick.transform.rotation * CurrentBrick.Collider.center, CurrentBrick.Collider.size / 2, CurrentBrick.transform.rotation, LegoLogic.LayerMaksLego);
                //    PositionOk = collider.Length == 0;
                //    if (!PositionOk)
                //        break;
                //    else
                //        placeposition.y -= LegoLogic.Grid.y;
                //}
                //for (int i = 0; i < 100; i++)
                //{
                //    var collider = Physics.OverlapBox(placeposition + CurrentBrick.transform.rotation * CurrentBrick.Collider.center, CurrentBrick.Collider.size / 2, CurrentBrick.transform.rotation, LegoLogic.LayerMaksLego);
                //    PositionOk = collider.Length == 0;
                //    if (PositionOk)
                //        break;
                //    else
                //    {
                //        placeposition.y += LegoLogic.Grid.y;
                //    }
                //}
                if (PositionOk)
                    CurrentBrick.transform.position = placeposition;
                else
                    CurrentBrick.transform.position = position;
            }
            //if (Physics.Raycast(VRCamera.transform.position, VRCamera.transform.forward, out var hitinfo, float.MaxValue, LegoLogic.LayerMaksLego))
            //{
            //    //snap to grid
            //    var position = LegoLogic.SnapToGrid(hitinfo.point);

            //    //try to find a collision free position
            //    var placeposition = position;
            //    PositionOk = false;
            //    for (int i = 0; i < 10; i++)
            //    {
            //        var collider = Physics.OverlapBox(placeposition + CurrentBrick.transform.rotation * CurrentBrick.Collider.center, CurrentBrick.Collider.size / 2, CurrentBrick.transform.rotation, LegoLogic.LayerMaksLego);
            //        PositionOk = collider.Length == 0;
            //        if (PositionOk)
            //            break;
            //        else
            //            placeposition.y += LegoLogic.Grid.y;
            //    }
            //    if (PositionOk)
            //        CurrentBrick.transform.position = placeposition;
            //    else
            //        CurrentBrick.transform.position = position;
            //}
            if ((Input.GetKeyDown(KeyCode.Space) || placeBrick.GetStateDown(SteamVR_Input_Sources.Any)) && CurrentBrick != null && PositionOk)
            {
                Debug.Log("plaatsen werkt");
                CurrentBrick.Collider.enabled = true;
                CurrentBrick.GetComponent<Renderer>().material = materials[color];

                var rot = CurrentBrick.transform.rotation;
                var pos = CurrentBrick.transform.position;
                CurrentBrick = null;
                SetNextBrick();
                CurrentBrick.transform.rotation = rot;
                CurrentBrick.transform.position = pos;
            }

            if (Input.GetKeyDown(KeyCode.P) || rotateBrick.GetStateDown(SteamVR_Input_Sources.Any))
            {
                CurrentBrick.transform.Rotate(Vector3.up, 90);
            }
        }
    }

    public void SetNextBrick()
    {
        CurrentBrick = Instantiate(PrefabBrick[brick]);
        CurrentBrick.Collider.enabled = false;
        CurrentBrick.GetComponent<Renderer>().material = materials[color+1];
    }
}
