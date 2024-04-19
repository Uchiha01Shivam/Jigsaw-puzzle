using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class TouchManager : MonoBehaviour
{
    [SerializeField] private GameObject currentPiece;
    [SerializeField] private LayerMask holderLayer, imgLayer;

    private void Awake()
    {
        TouchSimulation.Enable();
    }

    private void Start()
    {
        GameManager.instance.OnGameStart += EnableTouch;
        GameManager.instance.OnGameEnd += DisableTouch;
    }

    private void EnableTouch()
    {
        EnhancedTouchSupport.Enable();
        Touch.onFingerDown += Touch_onFingerDown;
        Touch.onFingerMove += Touch_onFingerMove;
        Touch.onFingerUp += Touch_onFingerUp;
    }
    private void DisableTouch()
    {
        Touch.onFingerDown -= Touch_onFingerDown;
        Touch.onFingerMove -= Touch_onFingerMove;
        Touch.onFingerUp -= Touch_onFingerUp;
        EnhancedTouchSupport.Disable();
    }
    private void OnDisable()
    {
        Touch.onFingerDown -= Touch_onFingerDown;
        Touch.onFingerMove -= Touch_onFingerMove;
        Touch.onFingerUp -= Touch_onFingerUp;
        EnhancedTouchSupport.Disable();
    }

    private void Touch_onFingerUp(Finger obj)
    {
        if(currentPiece != null)
        {
            Collider2D col = Physics2D.OverlapBox(currentPiece.transform.position,new Vector2(3.5f, 2),0,holderLayer);
 
            if(col != null)
            {
                if (col.transform.childCount == 0)
                {
                    Debug.Log(col.transform.name);
                    currentPiece.transform.position = col.transform.position;
                    currentPiece.transform.parent = col.transform;
                }
                else
                {
                    currentPiece.GetComponent<imagePieceManager>().BackToInitial();
                } 
            }
            else
            {
                currentPiece.GetComponent<imagePieceManager>().BackToInitial();
            }
            GameManager.instance.CheckAllImage();
            currentPiece.GetComponent<SpriteRenderer>().sortingOrder -= 1;
            currentPiece = null;
        }
    }

    private void Touch_onFingerMove(Finger obj)
    {
        if(currentPiece != null)
        {
            Vector2 touchPos = Camera.main.ScreenToWorldPoint(obj.currentTouch.screenPosition);
            currentPiece.transform.position = new(touchPos.x,touchPos.y,0);
        }
    }

    private void Touch_onFingerDown(Finger obj)
    {
        Collider2D collider = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(obj.currentTouch.screenPosition),imgLayer);
        if(collider != null)
        {
            if (collider.CompareTag("img"))
            {
                collider.transform.parent = null;
                collider.GetComponent<SpriteRenderer>().sortingOrder += 1;
                currentPiece = collider.gameObject;
            }
        }
    }
}
