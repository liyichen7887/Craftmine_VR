using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class menu_ctrl : MonoBehaviour
{

    private int[] button_arr = { 0, 1 };
    private int index;
    private Vector2 joystick_input;
    private Button[] button_list;
    private Image[] img_list;
    // Use this for initialization
    void Start()
    {
        index = 0;
        button_list = GetComponentsInChildren<Button>() as Button[];
        img_list = GetComponentsInChildren<Image>() as Image[];
        img_list[1].color = new Color(0.0f, 0.0f, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {

        joystick_input = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.RTouch);
        Debug.Log(joystick_input[1]);
        Button[] button_list = GetComponentsInChildren<Button>() as Button[];
        if (joystick_input[1] > 0.9f)
        {
            Debug.Log("test0");
            if (index == 1)
            {
                index = 0;
                img_list[1].color = new Color(0.0f, 0.0f, 1.0f);
                img_list[2].color = new Color(0, 0, 0);
                Debug.Log("0");
            }
        }
        else if (joystick_input[1] < -0.9f)
        {
            Debug.Log("test1");
            if (index == 0)
            {
                index = 1;
                img_list[2].color = new Color(0.0f, 0.0f, 1.0f);
                img_list[1].color = new Color(0, 0, 0);
                Debug.Log("1");
            }
        }
        if (OVRInput.Get(OVRInput.Button.PrimaryThumbstick, OVRInput.Controller.RTouch))
        {
            if (index == 0)
            {
                button_list[0].onClick.Invoke();
            }
            else if (index == 1)
            {
                button_list[1].onClick.Invoke();
            }
        }
    }
}
