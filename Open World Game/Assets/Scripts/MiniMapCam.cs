using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapCam : MonoBehaviour
{
    [SerializeField]
    private Transform Player;
    public float height;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        FollowPlayer();
    }

    public void FollowPlayer()
    {
        Vector3 pos = Player.position;

        gameObject.transform.position = new Vector3(pos.x, height, pos.z);
    }

    public void ChangeCameraZoom(float zoom)
    {
        gameObject.GetComponent<Camera>().orthographicSize = zoom; // Lerp from initial value to zoom
    }
}
