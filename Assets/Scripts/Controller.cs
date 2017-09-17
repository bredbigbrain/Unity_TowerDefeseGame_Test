using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour {

    private GameMode gameMode;

    public float speed = 6f;

    public float maxHeight = 4;
    public float minHeight = 0.1f;

    public float max_X = 17.5f;
    public float min_X = -17.5f;

    public float max_Z = 17.5f;
    public float min_Z = -34f;

    void Start()
    {
        gameMode = GameObject.FindGameObjectWithTag("GameMode").GetComponent<GameMode>();
    }

    void Update ()
    {
        if (!(gameMode.pause | gameMode.gameOver))
        {
            if ((Input.mousePosition.x <= 2.0 | Input.GetKey(KeyCode.A) ) && transform.position.x >= min_X)
            {
                transform.position -= new Vector3(speed * Time.deltaTime, 0, 0);
            }
            if ((Input.mousePosition.x >= Screen.width - 2.0 | Input.GetKey(KeyCode.D)) && transform.position.x <= max_X)
            {
                transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
            }
            if ((Input.mousePosition.y >= Screen.height - 2.0 | Input.GetKey(KeyCode.W)) && transform.position.z <= max_Z)
            {
                transform.position += new Vector3(0, 0, speed * Time.deltaTime);
            }
            if ((Input.mousePosition.y <= 2 | Input.GetKey(KeyCode.S)) && transform.position.z >= min_Z)
            {
                transform.position -= new Vector3(0, 0, speed * Time.deltaTime);
            }
            if ((Input.mouseScrollDelta.y < 0) & (transform.position.y <= maxHeight))
            {
                transform.position += new Vector3(0, speed * Time.deltaTime * 3, 0);
            }
            if ((Input.mouseScrollDelta.y > 0) & (transform.position.y >= minHeight))
            {
                transform.position -= new Vector3(0, speed * Time.deltaTime * 3, 0);
            }
        }
    }
}
