using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public enum CameraStyle
{
    EXPLORATION,
    COMBAT
}

public class ThirdPersonCam : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Transform player;
    public Transform playerObj;
    public Rigidbody rb;
    public PlayerMovement pm;

    public bool canUpdate;

    public float rotationSpeed;

    public Transform explorationLookAt;
    public Transform combatLookAt;

    public float explorationCameraDistance;
    public float combatCameraDistance;

    public CameraStyle currentStyle;

    public GameObject ExploringCinemachine;
    public GameObject CombatCinemachine;

    // Cinemachines values
    CinemachineFreeLook explCam;
    CinemachineFreeLook combCam;

    float explorationCamTopRigY;
    float explorationCamMiddleRigY;
    float explorationCamBottomRigY;
    
    float explorationCamTopRigRadius;
    float explorationCamMiddleRigRadius;
    float explorationCamBottomRigRadius;

    float explorationCamVectorTopY;
    float explorationCamVectorMiddleY;
    float explorationCamVectorBottomY;


    float combatCamTopRigY;
    float combatCamMiddleRigY;
    float combatCamBottomRigY;

    float combatCamTopRigRadius;
    float combatCamMiddleRigRadius;
    float combatCamBottomRigRadius;
    
    float combatCamVectorTopY;
    float combatCamVectorMiddleY;
    float combatCamVectorBottomY;


    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        explCam = ExploringCinemachine.GetComponent<CinemachineFreeLook>();
        combCam = CombatCinemachine.GetComponent<CinemachineFreeLook>();

        CalculateVectorsForCinemachineRig();
    }

    // Update is called once per frame
    void Update()
    {
        // Testing camera style switch
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchCameraStyle(CameraStyle.EXPLORATION);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchCameraStyle(CameraStyle.COMBAT);
        }

        // Rotate orientation
        Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        orientation.forward = viewDir.normalized;

        // Rotate player object
        if (pm.canRotate)
        {
            if (currentStyle == CameraStyle.EXPLORATION)
            {
                float horizontalInput = Input.GetAxisRaw("Horizontal");
                float verticalInput = Input.GetAxisRaw("Vertical");
                Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

                if (inputDir != Vector3.zero)
                {
                    playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);
                }
            }

            else if (currentStyle == CameraStyle.COMBAT)
            {
                Vector3 dirToCombatLookAt = combatLookAt.position - new Vector3(transform.position.x, combatLookAt.position.y, transform.position.z);
                orientation.forward = dirToCombatLookAt.normalized;

                playerObj.forward = dirToCombatLookAt.normalized;
            }
        }        
    }

    public void SwitchCameraStyle(CameraStyle newStyle)
    {
        if (currentStyle == newStyle) 
            return;

        ExploringCinemachine.SetActive(false);
        CombatCinemachine.SetActive(false);

        if (newStyle == CameraStyle.EXPLORATION)
        {
            ExploringCinemachine.SetActive(true);

            Vector3 playerToCamDir = (CombatCinemachine.transform.position - combatLookAt.transform.position).normalized;

            explCam.m_XAxis.Value = combCam.m_XAxis.Value;

            // Set the camera y value
            float yValue;

            if (playerToCamDir.y <= explorationCamVectorMiddleY)
            {
                float perc = Mathf.Abs(explorationCamVectorBottomY - playerToCamDir.y) / Mathf.Abs(explorationCamVectorMiddleY - explorationCamVectorBottomY);

                yValue = 0.5f * perc;
            }
            else
            {
                float perc = Mathf.Abs(explorationCamVectorMiddleY - playerToCamDir.y) / Mathf.Abs(explorationCamVectorTopY - explorationCamVectorMiddleY);

                yValue = 0.5f * (1f + perc);
            }

            explCam.m_YAxis.Value = yValue;
        }
        else if (newStyle == CameraStyle.COMBAT)
        {
            CombatCinemachine.SetActive(true);

            Vector3 playerToCamDir = (ExploringCinemachine.transform.position - explorationLookAt.transform.position).normalized;

            combCam.m_XAxis.Value = explCam.m_XAxis.Value;
            
            // Set the camera y value
            float yValue;

            if (playerToCamDir.y <= combatCamVectorMiddleY)
            {
                float perc = Mathf.Abs(combatCamVectorBottomY - playerToCamDir.y) / Mathf.Abs(combatCamVectorMiddleY - combatCamVectorBottomY);

                yValue = 0.5f * perc;
            }
            else
            {
                float perc = Mathf.Abs(combatCamVectorMiddleY - playerToCamDir.y) / Mathf.Abs(combatCamVectorTopY - combatCamVectorMiddleY);

                yValue = 0.5f * (1f + perc);
            }

            combCam.m_YAxis.Value = yValue;
        }

        currentStyle = newStyle;
    }

    // To call if cinemachine rigs are being modified at runtime
    public void CalculateVectorsForCinemachineRig()
    {
        explorationCamTopRigY = explCam.m_Orbits[0].m_Height;
        explorationCamMiddleRigY = explCam.m_Orbits[1].m_Height;
        explorationCamBottomRigY = explCam.m_Orbits[2].m_Height;

        explorationCamTopRigRadius = explCam.m_Orbits[0].m_Radius;
        explorationCamMiddleRigRadius = explCam.m_Orbits[1].m_Radius;
        explorationCamBottomRigRadius = explCam.m_Orbits[2].m_Radius;

        explorationCamVectorTopY = (new Vector3(explorationCamTopRigRadius, explorationCamTopRigY, 0).normalized).y;
        explorationCamVectorMiddleY = (new Vector3(explorationCamMiddleRigRadius, explorationCamMiddleRigY, 0).normalized).y;
        explorationCamVectorBottomY = (new Vector3(explorationCamBottomRigRadius, explorationCamBottomRigY, 0).normalized).y;


        combatCamTopRigY = combCam.m_Orbits[0].m_Height;
        combatCamMiddleRigY = combCam.m_Orbits[1].m_Height;
        combatCamBottomRigY = combCam.m_Orbits[2].m_Height;
        
        combatCamTopRigRadius = combCam.m_Orbits[0].m_Radius;
        combatCamMiddleRigRadius = combCam.m_Orbits[1].m_Radius;
        combatCamBottomRigRadius = combCam.m_Orbits[2].m_Radius;
        
        combatCamVectorTopY = (new Vector3(combatCamTopRigRadius, combatCamTopRigY, 0).normalized).y;
        combatCamVectorMiddleY = (new Vector3(combatCamMiddleRigRadius, combatCamMiddleRigY, 0).normalized).y;
        combatCamVectorBottomY = (new Vector3(combatCamBottomRigRadius, combatCamBottomRigY, 0).normalized).y;
    }
}
