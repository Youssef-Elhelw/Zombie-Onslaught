using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using Unity.Mathematics;
using Unity.VisualScripting;
using JetBrains.Annotations;

public class ShopItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject turretPrefab;
    public GameObject whileDragging;
    private GameObject turretInstance;
    private Camera mainCamera;
    public Money money;
    public int Cost = 200;
    private bool canDrag = true;
    public bool isDragging;
    private Collider turretCollider;
    public Animator animator;
    public Animator animatorMoneyError;

    void Start()
    {

        mainCamera = Camera.main;
        if (money == null)
        {
            money = FindObjectOfType<Money>();
        }
    }

    void Update()
    {
        if (canDrag && turretInstance != null)
        {
            // Debug.Log(turretInstance.name + "Im being dragged");

            float rotateSpeed = 100f;
            if (Input.GetKey(KeyCode.R))
            {
                turretInstance.transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);
            }
            else if (Input.GetKey(KeyCode.E))
            {
                turretInstance.transform.Rotate(Vector3.up * -rotateSpeed * Time.deltaTime);
            }
        }
        if (Input.GetKeyDown(KeyCode.Mouse1) && isDragging && turretInstance != null)
        {
            turretInstance.transform.DOKill(); //kill any dotween on this object :)
            money.currentMoney += Cost;
            Destroy(turretInstance);
            turretInstance = null;
            isDragging = false;
        }

    }
    //promblem happens when i choose two different turrets //solved in line 161 :)

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (money.currentMoney < Cost)
        {
            animatorMoneyError.SetTrigger("NoMoney");
            canDrag = false;
            isDragging = false;
            return;
        }

        money.currentMoney -= Cost;
        canDrag = true;
        isDragging = true;
        // Debug.Log("i cant shoot " + isDragging);
        turretInstance = Instantiate(turretPrefab);
        turretCollider = turretInstance.GetComponent<Collider>();

        TurretAffairs turretScript = turretInstance.GetComponentInChildren<TurretAffairs>();

        turretScript.isDragging = true;//solved Alhamdulla


        if (turretCollider != null)
        {
            turretCollider.enabled = false;
        }


    }


    public void OnDrag(PointerEventData eventData)
    {

        if (!canDrag || turretInstance == null) return;
        whileDragging.SetActive(true);

        Ray ray = mainCamera.ScreenPointToRay(eventData.position);
        Plane plane = new Plane(Vector3.up, Vector3.zero);

        if (plane.Raycast(ray, out float distance) && turretInstance != null)
        {
            Vector3 worldPosition = ray.GetPoint(distance);
            turretInstance.transform.position = new Vector3(worldPosition.x, 1.12f, worldPosition.z);
        }

    }
    public quaternion origianlRotation;
    public void OnEndDrag(PointerEventData eventData)
    {
        if (!canDrag || turretInstance == null) return;
        canDrag = false;
        TurretAffairs turretScript = turretInstance.GetComponentInChildren<TurretAffairs>();

        turretScript.isDragging = false;

        whileDragging.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack).OnComplete(() =>
        {
            whileDragging.SetActive(false);
        });

        Ray ray = mainCamera.ScreenPointToRay(eventData.position);
        Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red, 2f);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.CompareTag("Grid"))
            {
                GridSlot gridManager = FindObjectOfType<GridSlot>();
                if (gridManager != null)
                {
                    Vector3 snappedPosition = gridManager.GetNearestGridPosition(hit.point);
                    snappedPosition.y = 0;
                    turretInstance.transform.position = snappedPosition;
                    AudioManager.Instance.PlaySFX(AudioManager.Instance.purchaseTurret, 0.5f);
                    AudioManager.Instance.PlaySFX(AudioManager.Instance.buildingSound, 2f);
                    origianlRotation = turretInstance.transform.rotation;
                }
            }
            else
            {
                Debug.Log("this is what i landed on " + hit.collider.name);
                Destroy(turretInstance);
                money.currentMoney += Cost;
                animator.SetTrigger("WrongPlace");
            }
        }
        else
        {

            Debug.Log("no grid found");
            Destroy(turretInstance);
            money.currentMoney += Cost;
            animator.SetTrigger("WrongPlace");
        }

        if (isDragging)
        {
            isDragging = false;
            // Debug.Log("i cant shoot " + isDragging);
        }
        if (turretCollider != null)
        {
            turretCollider.enabled = true;
        }
        turretInstance = null; //solved HeheheHAA 

    }

}
