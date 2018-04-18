using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class RayCast : MonoBehaviour {

    private RaycastHit old_hit;
    private Material old_mat;
    public GameObject terrain;
    private TerrainGenrator generator;
    private bool can_click;

    // Use this for initialization
    void Start () {
        generator = terrain.GetComponent<TerrainGenrator>();
        can_click = true;
    }
	
	// Update is called once per frame
	void Update () {
        //Debug.Log(this.transform.position);
        //target.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, Camera.main.nearClipPlane));

        Ray myRay = new Ray(this.transform.position, this.transform.forward);
        Debug.DrawRay(myRay.origin,myRay.direction*100);
        RaycastHit hitObject;
        if (Physics.Raycast(myRay, out hitObject, 7))
        {
            if ("Chest(Clone)" == hitObject.collider.gameObject.name)
            {
                Transform hitLocation = hitObject.transform;
                MeshRenderer hit_renderer = hitLocation.GetComponent<MeshRenderer>();
                Material[] chest_mat = hit_renderer.materials;
                for (int i = 0; i < 3; i++)
                {
                    Material highlight = new Material(chest_mat[i]);
                    highlight.SetColor("_Color", new Color(1.0f, 0.0f, 0.0f));
                    hit_renderer.materials[i] = highlight;
                }
                hitLocation.GetComponents<AudioSource>()[0].enabled = false;
                hitLocation.GetComponents<AudioSource>()[1].enabled = true;

            }
            else if ("OVRPlayerController" != hitObject.collider.gameObject.name)
            {

                if (old_hit.collider != null)
                {
                    Transform old_hit_loc = old_hit.transform;
                    MeshRenderer old_hit_renderer = old_hit_loc.GetComponent<MeshRenderer>();
                    old_hit_renderer.material = old_mat;

                }

                old_hit = hitObject;


                Transform hitLocation = hitObject.transform;
                MeshRenderer hit_renderer = hitLocation.GetComponent<MeshRenderer>();
                old_mat = hit_renderer.material;
                Material highlight = new Material(hit_renderer.material);
                highlight.SetColor("_Color", new Color(1.0f, 0.0f, 0.0f));
                hit_renderer.material = highlight;
                if (can_click)
                {
                    if (CrossPlatformInputManager.GetAxis("Oculus_GearVR_RIndexTrigger") > 0.8f)
                    {


                        can_click = false;
                        float x = hitObject.transform.position.x;
                        float y = hitObject.transform.position.y;
                        float z = hitObject.transform.position.z;

                        if (Mathf.Abs(hitObject.point.x - x) == 0.5f)
                            x += Mathf.Sign(hitObject.point.x - x);
                        else if (Mathf.Abs(hitObject.point.y - y) == 0.5f)
                            y += Mathf.Sign(hitObject.point.y - y);
                        else if (Mathf.Abs(hitObject.point.z - z) == 0.5f)
                            z += Mathf.Sign(hitObject.point.z - z);

                        generator.GenerateBlock(new Vector3(x, y, z));
                        Debug.Log("1");
                        Debug.Log(can_click);
                        Debug.Log("-------------------------------------------------");
                    }
                    else if (CrossPlatformInputManager.GetAxis("Oculus_GearVR_RHandTrigger") > 0.8f)
                    {
                        can_click = false;
                        generator.BlockDestroyed(hitObject.transform.position);
                        Destroy(hitObject.transform.gameObject);
                        Debug.Log("2");
                        Debug.Log(can_click);
                        Debug.Log("-------------------------------------------------");

                    }
                }
                if (CrossPlatformInputManager.GetAxis("Oculus_GearVR_RIndexTrigger") < 0.2f && CrossPlatformInputManager.GetAxis("Oculus_GearVR_RHandTrigger") < 0.2f)
                {
                    can_click = true;
                    Debug.Log("3");
                    Debug.Log(can_click);
                    Debug.Log("-------------------------------------------------");
                }




            }
        }
        else {
            if (old_hit.collider != null)
            {
                if ("OVRPlayerController" != old_hit.collider.gameObject.name)
                {
                    Transform old_hit_loc = old_hit.transform;
                    MeshRenderer old_hit_renderer = old_hit_loc.GetComponent<MeshRenderer>();
                    old_hit_renderer.material = old_mat;
                    old_hit = new RaycastHit();
                }
                    
            }
        }
    }
}

