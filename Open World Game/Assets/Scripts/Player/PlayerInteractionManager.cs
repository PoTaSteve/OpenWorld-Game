using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteractionManager : MonoBehaviour
{
    public Transform Player;

    public GameObject InteractablePopUp;

    public List<GameObject> InRangeInteractables = new List<GameObject>();
    
    public GameObject InteractableLookingAt;

    public float maxLookAngle;

    public Camera cam;

    public Canvas canvas;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckForInteractableInSight();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Interactable interactab))
        {
            InRangeInteractables.Add(other.gameObject);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (InRangeInteractables.Contains(other.gameObject))
        {
            InRangeInteractables.Remove(other.gameObject);
        }
    }

    public void CheckForInteractableInSight()
    {
        if (InRangeInteractables.Count > 0)
        {
            InteractablePopUp.SetActive(true);

            Plane[] planes = GeometryUtility.CalculateFrustumPlanes(cam);

            List<GameObject> acceptableObj = new List<GameObject>();

            foreach (GameObject obj in InRangeInteractables)
            {
                if (GeometryUtility.TestPlanesAABB(planes, obj.GetComponent<Collider>().bounds))
                {
                    Vector3 objVec = obj.transform.position - cam.transform.position;

                    if (Vector3.Angle(objVec, cam.transform.forward) < maxLookAngle)
                    {
                        acceptableObj.Add(obj);
                    }
                }
            }
            

            if (acceptableObj.Count > 1)
            {
                float minAngle = maxLookAngle;
                InteractableLookingAt = acceptableObj[0];

                foreach (GameObject obj in acceptableObj)
                {
                    float objAngle = Vector3.Angle(cam.transform.forward, obj.transform.position - cam.transform.position);
                    if (objAngle < minAngle)
                    {
                        minAngle = objAngle;
                        InteractableLookingAt = obj;
                    }
                }
            }
            else if (acceptableObj.Count == 1)
            {
                InteractableLookingAt = acceptableObj[0];
            }
            else
            {
                InteractablePopUp.SetActive(false);

                return;
            }
            
            Vector2 res = canvas.GetComponent<CanvasScaler>().referenceResolution;
            Vector3 delta = new Vector3(res.x, res.y, 0);

            InteractablePopUp.GetComponent<RectTransform>().localPosition = cam.WorldToScreenPoint(InteractableLookingAt.transform.GetChild(1).position) - delta / 2;

            InteractablePopUp.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = InteractableLookingAt.GetComponent<Interactable>().interactionName;
        }
        else
        {
            InteractablePopUp.SetActive(false);
        }
    }
}
