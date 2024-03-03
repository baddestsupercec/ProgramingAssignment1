using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Photon.Pun;
using Unity.XR.CoreUtils;
using TMPro;

public class MovementManager : MonoBehaviour
{
    private PhotonView myView;
    public GameObject myChild;
    public GameObject myChild2;

    private float xInput;
    private float yInput;
    private float movementSpeed = 6.0f;

    private InputData inputData;
    //[SerializeField] private GameObject myObjectToMove;
    private Rigidbody myRB;
    private Transform myXRRig;

    private GameObject planet;

    private GameObject cam;
    public GameObject parent;
    public Transform parentTransform;
    private GameObject mainCam;
    private GameObject camref;
    private Vector3 moveAmount;
    private Vector3 smoothMoveVelocity;

    private TextMeshProUGUI TP;

    //public TextMeshProUGUI ui;

    private float tim = 5;

    public int teleportCredits;
    private GameObject canvas;

    // Start is called before the first frame update
    void Start()
    {
        myView = GetComponent<PhotonView>();

        myChild = transform.GetChild(0).gameObject;
        myRB = myChild.GetComponent<Rigidbody>();
        myChild2 = transform.GetChild(1).gameObject;

        GameObject myXrOrigin = GameObject.Find("XR Origin");
        parent = GameObject.Find("XRParent");
        parentTransform = parent.transform;
        camref = myXrOrigin.transform.GetChild(0).gameObject;
        cam = myXrOrigin.transform.GetChild(0).GetChild(0).gameObject;
        mainCam = myXrOrigin.transform.GetChild(0).GetChild(0).GetChild(0).gameObject;
        myXRRig = myXrOrigin.transform;
        inputData = myXrOrigin.GetComponent<InputData>();
        planet = GameObject.Find("Planet");
        teleportCredits = 0;
        canvas = GameObject.Find("Canvas");
        TP = canvas.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>();
       // if (myView.IsMine)
        //{
          //  ui = GameObject.Find("UI").GetComponent<TextMeshProUGUI>();
        //}
        myRB.constraints = RigidbodyConstraints.FreezeRotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (myView.IsMine)
        {
            tim += Time.deltaTime;
            TP.text = "TP Credits: " + teleportCredits;
            parentTransform.transform.position = myChild.transform.position;
            //camref.transform.rotation = myChild.transform.rotation;
            //Debug.Log(mainCam.transform.rotation.eulerAngles);
            //Quaternion headsetRotation2 = InputTracking.GetLocalRotation(XRNode.Head);
            if (inputData.head.TryGetFeatureValue(CommonUsages.deviceRotation, out Quaternion rot))
            {
                //myChild.transform.rotation = rot;
                //myChild.transform.rotation = Quaternion.Euler(new Vector3(myChild.transform.rotation.x, rot.eulerAngles.y, myChild.transform.rotation.z));
                //parentTransform.rotation = Quaternion.Euler(new Vector3(myChild.transform.rotation.x, parent.transform.rotation.y, myChild.transform.rotation.z));
            }
            //myChild.transform.rotation = Quaternion.Euler(new Vector3(myChild.transform.rotation.x, mainCam.transform.rotation.y, myChild.transform.rotation.z));
            //transform.rotation = mainCam.transform.rotation;

            if (inputData.rightController.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 movement))
            {
                xInput = movement.x;
                yInput = movement.y;
                Vector3 moveDir = new Vector3(xInput, 0, yInput).normalized;
                Vector3 targetMoveAmount = moveDir * movementSpeed;
                moveAmount = Vector3.SmoothDamp(moveAmount, targetMoveAmount, ref smoothMoveVelocity, .15f);
            }
            bool trigger = false;
            if (inputData.rightController.TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out trigger))
            {
                if (trigger && tim > 1 && teleportCredits > 0)
                {
                    //movementSpeed=12;
                    tim = 0;
                    myRB.position = myRB.position + mainCam.transform.forward * 10;
                    teleportCredits--;
                }
            }
            myChild.transform.rotation = Quaternion.FromToRotation(-1 * myChild.transform.up, (planet.transform.position - myChild.transform.position).normalized) * myChild.transform.rotation;
            //parentTransform.rotation = Quaternion.Euler(new Vector3(myChild.transform.rotation.x, parent.transform.rotation.y, myChild.transform.rotation.z));
            myRB.AddForce((planet.transform.position - myChild.transform.position).normalized * 10);
            //parentTransform.rotation = Quaternion.Euler(new Vector3(myChild.transform.rotation.x, parent.transform.rotation.y, myChild.transform.rotation.z));
        }
    }

    public void speedUp(){
        StartCoroutine(speedUpRoutine());
    }
    
    public IEnumerator speedUpRoutine()
    {
        movementSpeed*=2;
        yield return new WaitForSeconds(5);
        movementSpeed/=2;
    }

    private void FixedUpdate()
    {
        //myRB.AddForce(xInput * movementSpeed, 0, yInput * movementSpeed);
        if (myView.IsMine)
        {
            Vector3 localMove = mainCam.transform.TransformDirection(moveAmount) * Time.fixedDeltaTime;
            // Rigidbody rb = GetComponent<Rigidbody>();
            myRB.MovePosition(myRB.position + localMove);
            myChild2.transform.position = myChild.transform.position;
            //ui.text = myChild.transform.rotation.eulerAngles.ToString();
            camref.transform.rotation = myChild.transform.rotation;
            myChild2.transform.rotation = mainCam.transform.rotation;
        }
    }
}