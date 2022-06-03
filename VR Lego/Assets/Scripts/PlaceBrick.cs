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
    private int brick = 1;
    public Transform controller;

    public SteamVR_ActionSet actionSet;
    public SteamVR_Action_Boolean switchBrick;
    public SteamVR_Action_Boolean prevBrick;
    public SteamVR_Action_Boolean switchColor;
    public SteamVR_Action_Boolean prevColor;
    public SteamVR_Action_Boolean placeBrick;
    public SteamVR_Action_Boolean rotateBrick;
    public SteamVR_Action_Boolean deleteMode;

    private bool deleteModeActive = true;

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

        if (prevBrick.GetStateDown(SteamVR_Input_Sources.Any))
        {
            ChangeBrickPrev();
        }

        if (deleteMode.GetStateDown(SteamVR_Input_Sources.Any))
        {
            if (deleteModeActive == true)
            {
                deleteModeActive = false;
                SetNextBrick();
            }
            else if (deleteModeActive == false)
            {
                deleteModeActive = true;
                CurrentBrick.GetComponent<Renderer>().material = materials[0];
                GameObject.DestroyImmediate(CurrentBrick.gameObject);
                CurrentBrick = null;
            }
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

            if ((Input.GetKeyDown(KeyCode.Space) || placeBrick.GetStateDown(SteamVR_Input_Sources.Any)) && CurrentBrick.GetComponent<Brick>().IsNotColliding == true)
            {
                CurrentBrick.Collider.enabled = true;
                CurrentBrick.GetComponent<Renderer>().material = materials[color];

                var rot = CurrentBrick.transform.rotation;
                var pos = CurrentBrick.transform.position;
                for (int a = 0; a < CurrentBrick.transform.childCount; a++)
                {
                    CurrentBrick.transform.GetChild(a).gameObject.SetActive(true);
                }
                CurrentBrick.GetComponent<Brick>().LegoLayer();
                CurrentBrick = null;
                SetNextBrick();
                CurrentBrick.transform.rotation = rot;
                CurrentBrick.transform.position = pos;
            }

            if (Input.GetKeyDown(KeyCode.G) || switchColor.GetStateDown(SteamVR_Input_Sources.Any))
            {
                ChangeColor();
            }

            if (prevColor.GetStateDown(SteamVR_Input_Sources.Any))
            {
                ChangeColorPrev();
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
        //CurrentBrick.Collider.enabled = false;
        CurrentBrick.GetComponent<Renderer>().material = materials[color+1];
        CurrentBrick.GetComponent<Brick>().NoRaycastLayer();
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
            brick = 1;
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
        else
        {
            SetNextBrick();
        }
    }

    private void ChangeBrickPrev()
    {
        brick -= 1;
        if (brick < 1)
        {
            brick = PrefabBrick.Length - 1;
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
        else
        {
            SetNextBrick();
        }
    }

    private void ChangeColorPrev()
    {
        color -= 2;
        if (color < 1)
        {
            color = materials.Length - 2;
        }
        if (CurrentBrick != null)
            CurrentBrick.GetComponent<Renderer>().material = materials[color + 1];
    }
}
