using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class GetTouchPoint : MonoBehaviour
{
    private Text[] text;
    private Touch touch;
    private Touch touch2;
    public float timenow;
    public float waitTime = 1f;
    private TCPSocketConnect _socket;

    // Start is called before the first frame update
    void Start()
    {
        _socket = GetComponent<TCPSocketConnect>();
        text = GetComponentsInChildren<Text>();
        timenow = Time.time;
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);
            // Update the Text on the screen depending on current position of the touch each frame
            text[1].text = "Touch Position : " + touch.position;

            if (Input.touchCount > 1)
            {
                touch2 = Input.GetTouch(1);
                // Update the Text on the screen depending on current position of the touch each frame
                text[1].text = "Touch Position 1 : " + touch.position + "Touch Position 2 : " + touch2.position;
            }

        }
        else
        {
            text[1].text = "No touch contacts";
        }

        if (timenow + waitTime < Time.time)
        {
            if (Input.touchCount > 0)
            {
                var myPosiObj = new TouchPositionObject();
                myPosiObj.pos = Input.GetTouch(0).position;
                myPosiObj.touchNumber = 1;
                var msg = Encoding.ASCII.GetBytes(myPosiObj.pos.ToString() + ":" + myPosiObj.touchNumber); 
                _socket.SendHelloMessage(msg);

                if (Input.touchCount > 1)
                {
                    myPosiObj = new TouchPositionObject();
                    myPosiObj.pos = Input.GetTouch(1).position;
                    myPosiObj.touchNumber = 1;
                    msg = Encoding.ASCII.GetBytes(myPosiObj.pos.ToString() + ":" + myPosiObj.touchNumber);
                    _socket.SendHelloMessage(msg);
                }

            }
            
        }
    }
}
