using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniMap : MonoBehaviour
{
    public GameObject Player;
    [SerializeField]
    private GameObject mapImage;
    
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
        Vector3 playerPos = Player.transform.position;

        mapImage.GetComponent<RectTransform>().anchoredPosition = new Vector3(-playerPos.x, -playerPos.z, 0);
    }
}
