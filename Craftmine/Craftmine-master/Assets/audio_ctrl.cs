using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class audio_ctrl : MonoBehaviour {
    private float volume;
    private Vector2 joystick_input;
    private Image[] img_list;
    int index;
    // Use this for initialization
    void Start () {
        volume = AudioListener.volume;
        index = 0;
        GetComponentInChildren<Slider>().value = volume;
        ColorBlock temp = GetComponentInChildren<Slider>().colors;
        temp.normalColor = new Color(0.0f, 0.0f, 1.0f);
        GetComponentInChildren<Slider>().colors = temp;

        img_list = GetComponentsInChildren<Image>() as Image[];
    }
	
	// Update is called once per frame
	void Update () {
        volume = AudioListener.volume;
        GetComponentInChildren<Slider>().value = volume;
        joystick_input = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.RTouch);
        if (joystick_input[1] > 0.9f) {
            if (index == 1)
            {
                index = 0;
                ColorBlock temp = GetComponentInChildren<Slider>().colors;
                temp.normalColor = new Color(0.0f, 0.0f, 1.0f);
                GetComponentInChildren<Slider>().colors = temp;

                img_list[4].color = new Color(0, 0, 0);

            }
        }
        else if(joystick_input[1] < -0.9f)
        {
            if (index == 0)
            {
                index = 1;
                ColorBlock temp = GetComponentInChildren<Slider>().colors;
                temp.normalColor = new Color(255, 255, 255);
                GetComponentInChildren<Slider>().colors = temp;

                img_list[4].color = new Color(0.0f, 0.0f, 1.0f);
            }
        }
        else {
            if (index == 0)
            {
                joystick_input = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.RTouch);
                volume += joystick_input[0];
                if (volume > 1.0f)
                {
                    volume = 1.0f;
                }
                else if (volume < 0.0f)
                {
                    volume = 0.0f;
                }
                GetComponentInChildren<Slider>().value = volume;
                AudioListener.volume = volume;
            }
            else if (index == 1)
            {
                if (OVRInput.Get(OVRInput.Button.PrimaryThumbstick, OVRInput.Controller.RTouch))
                {
                    GetComponentInChildren<Button>().onClick.Invoke();
                }
            }
        }
        
    }
}
