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

    protected Brick CurrentBrick;
    private int color = 1;
    private int brick = 0;
    public Transform controller;

    public SteamVR_ActionSet actionSet;
    public SteamVR_Action_Boolean switchBrick;
    public SteamVR_Action_Boolean switchColor;
    public SteamVR_Action_Boolean placeBrick;
    public SteamVR_Action_Boolean rotateBrick;

    public bool colliding = false;

    void Start()
    {
        //SetNextBrick();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H) || switchBrick.GetStateDown(SteamVR_Input_Sources.Any))
        {
            ChangeBrick();
        }

        if (CurrentBrick != null)
        {
            if (Physics.Raycast(controller.transform.position, controller.transform.forward, out var hitinfo, float.MaxValue, LegoLogic.LayerMaksLego))
            {
                var position = LegoLogic.SnapToGrid(hitinfo.point);
                
                //if (Input.GetKeyDown(KeyCode.I))
                //{
                //    position.x += LegoLogic.Grid.x;
                //}
                //if (Input.GetKeyDown(KeyCode.K))
                //{
                //    position.x -= LegoLogic.Grid.x;
                //}
                //if (Input.GetKeyDown(KeyCode.L))
                //{
                //    position.z += LegoLogic.Grid.z;
                //}
                //if (Input.GetKeyDown(KeyCode.J))
                //{
                //    position.z -= LegoLogic.Grid.z;
                //}
                //if (Input.GetKeyDown(KeyCode.N))
                //{
                //    position.y -= LegoLogic.Grid.y;
                //}
                CurrentBrick.transform.position = position;
            }
            if ((Input.GetKeyDown(KeyCode.Space) || placeBrick.GetStateDown(SteamVR_Input_Sources.Any)) && colliding == false)
            {
                Debug.Log("plaatsen werkt");
                CurrentBrick.Collider.enabled = true;
                CurrentBrick.GetComponent<Renderer>().material = materials[color];

                var rot = CurrentBrick.transform.rotation;
                var pos = CurrentBrick.transform.position;
                for (int a = 0; a < CurrentBrick.transform.childCount; a++)
                {
                    CurrentBrick.transform.GetChild(a).gameObject.SetActive(true);
                }
                CurrentBrick.GetComponent<Brick>().CurrentBrick = false;
                CurrentBrick = null;
                SetNextBrick();
                CurrentBrick.transform.rotation = rot;
                CurrentBrick.transform.position = pos;
            }

            if (Input.GetKeyDown(KeyCode.G) || switchColor.GetStateDown(SteamVR_Input_Sources.Any))
            {
                ChangeColor();
            }

            if (Input.GetKeyDown(KeyCode.P) || rotateBrick.GetStateDown(SteamVR_Input_Sources.Any))
            {
                CurrentBrick.transform.Rotate(Vector3.up, 90);
            }
        }

        else {
            if (Physics.Raycast(controller.transform.position, controller.transform.forward, out var hitinfo, float.MaxValue, LegoLogic.LayerMaksLego))
            {
                if ((Input.GetKeyDown(KeyCode.Space) || placeBrick.GetStateDown(SteamVR_Input_Sources.Any)) && hitinfo.collider.gameObject.tag == "LegoDeletable")
                {
                    GameObject.DestroyImmediate(hitinfo.collider.gameObject);
                }
            }
        }
    }

    public void SetNextBrick()
    {
        CurrentBrick = Instantiate(PrefabBrick[brick]);
        CurrentBrick.Collider.enabled = false;
        CurrentBrick.GetComponent<Renderer>().material = materials[color+1];
        CurrentBrick.GetComponent<Brick>().CurrentBrick = true;
    }

    private void ChangeColor()
    {
        color += 2;
        if (color > (materials.Length - 1))
        {
            color = 1;
        }
        if (CurrentBrick != null)
            CurrentBrick.GetComponent<Renderer>().material = materials[color + 1];
    }

    private void ChangeBrick()
    {
        brick += 1;
        if (brick > (PrefabBrick.Length - 1))
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

            if (brick != 0)
            {
                SetNextBrick();
                CurrentBrick.transform.rotation = rot;
                CurrentBrick.transform.position = pos;
            }
        }
        else if (brick != 0)
        {
            SetNextBrick();
        }
    }
}
