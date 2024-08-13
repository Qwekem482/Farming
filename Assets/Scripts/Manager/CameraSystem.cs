using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class CameraSystem : SingletonMonoBehavior<CameraSystem>
{
    [SerializeField] Camera mainCam;
    [SerializeField] float topLimit;
    [SerializeField] float bottomLimit;
    [SerializeField] float leftLimit;
    [SerializeField] float rightLimit;
    [SerializeField] float minZoom;
    [SerializeField] float maxZoom;

    bool isMoveAllowed;
    Vector3 touchPosition;

    Transform followObjectTransform;
    Bounds objectBounds;
    Vector3 prevPosition;
    
    // Start is called before the first frame update
    void Start()
    {
        prevPosition = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        switch (Input.touchCount)
        {
            case <= 0:
                return;
            case 2:
                ZoomOperation();
                break;
            default:
                MoveOperation();
                break;
        }
    }

    void FixedUpdate()
    {
        if (followObjectTransform != null)
        {
            FollowOperation();
        }
    }

    void Zoom(float value)
    {
        mainCam.orthographicSize = Mathf.Clamp(mainCam.orthographicSize - value, minZoom, maxZoom);
    }

    void ZoomOperation()
    {
        Touch zero = Input.GetTouch(0);
        Touch one = Input.GetTouch(1);

        if (EventSystem.current.IsPointerOverGameObject(zero.fingerId) 
            || EventSystem.current.IsPointerOverGameObject(one.fingerId)) return;

        float distTouch = ((zero.position - zero.deltaPosition)
                           - (one.position - one.deltaPosition)).magnitude;
        float newDistTouch = (zero.position - one.position).magnitude;
                
        Zoom((newDistTouch - distTouch) * 0.01f);
    }

    void MoveOperation()
    {
        Touch touch = Input.GetTouch(0);

        switch (touch.phase)
        {
            case TouchPhase.Began:
                isMoveAllowed = !EventSystem.current.IsPointerOverGameObject(touch.fingerId);
                touchPosition = mainCam.ScreenToWorldPoint(touch.position);
                break;
            case TouchPhase.Moved:
                if (isMoveAllowed)
                {
                    Vector3 direction = touchPosition - mainCam.ScreenToWorldPoint(touch.position);
                    mainCam.transform.position += direction;
                }
                        
                ClampCamera();
                break;
        }
    }

    void FollowOperation()
    {
        Vector3 objectPosition = mainCam.WorldToViewportPoint
            (followObjectTransform.position + objectBounds.max);

        if (objectPosition.x <= 0.15f ||
            objectPosition.x >= 1f ||
            objectPosition.y <= 0.35f ||
            objectPosition.y > 1.1f)
        {
            Vector3 position = mainCam.ScreenToWorldPoint(followObjectTransform.position);
            mainCam.transform.position += position - prevPosition;
            prevPosition = position;
        } else
        {
            prevPosition = mainCam.ScreenToWorldPoint(followObjectTransform.position);
        }
        
        ClampCamera();
    }

    void ClampCamera()
    {
        Vector3 position = mainCam.transform.position;
        
        position = new Vector3
        (
            Mathf.Clamp(position.x, leftLimit, rightLimit),
            Mathf.Clamp(position.y, bottomLimit, topLimit),
            -10f
        );
        
        mainCam.transform.position = position;
    }

    public void SetupFollow(Transform followObject)
    {
        followObjectTransform = followObject;
        objectBounds = followObject.GetComponent<PolygonCollider2D>().bounds;
        prevPosition = mainCam.ScreenToWorldPoint(Vector3.zero);
    }

    public void Unfollow()
    {
        followObjectTransform = null;
    }

    public void Focus(Vector3 focusPosition, UnityAction onComplete = null)
    {
        Vector3 camPosition = mainCam.transform.position;
        Vector3 position = new Vector3(focusPosition.x, focusPosition.y, camPosition.z);

        if (camPosition == position)
        {
            onComplete?.Invoke();
            return;
        }
        
        mainCam.transform.DOMove(position, 0.5f)
            .OnComplete(() => onComplete?.Invoke());
        ClampCamera();
        touchPosition = camPosition;
    }
}
