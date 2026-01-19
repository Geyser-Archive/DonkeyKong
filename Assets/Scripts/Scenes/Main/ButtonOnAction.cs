using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ButtonOnAction : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler, IPointerUpHandler
{
    public bool Interactable
    {
        get
        {
            return this.interactable;
        }
        set
        {
            this.interactable = value;
        }
    }
 
    [SerializeField] private bool interactable = true;
    [SerializeField] private UnityEvent onClick;
    [SerializeField] private UnityEvent onDown;
    [SerializeField] private UnityEvent onEnter;
    [SerializeField] private UnityEvent onExit;
    [SerializeField] private UnityEvent onMove;
    [SerializeField] private UnityEvent onUp;

    public void OnPointerClick(PointerEventData eventData)
    {
        this.onClick.Invoke();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        this.onDown.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        this.onEnter.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        this.onExit.Invoke();
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        this.onMove.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        this.onUp.Invoke();
    }

    public void Call()
    {
        this.onDown.Invoke();
    }

    private void Start()
    {
        if (!this.interactable)
        {
            Object.Destroy(this);
        }
    }
}