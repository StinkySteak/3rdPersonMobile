using StarterAssets;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.OnScreen;

public class UI_Virtual_Pad_Look : OnScreenControl, IDragHandler, IBeginDragHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Vector2 _currentPos;

    [SerializeField] private Vector2 _deltaDrag;

    [InputControl(layout = "Vector2")]
    [SerializeField] private string _inputAxis;

    [SerializeField] private StarterAssetsInputs _input;

    [SerializeField] private Vector2 _resolutionRefence = new(1920, 1080);
    [SerializeField][Range(0, 1)] private float _sensitivity = 0.5f;

    [SerializeField] private float _compensatedSensitivity;

    [SerializeField] private float _moveForwardMinDuration = 1;
    private float _pressDuration;

    private bool _isDragging;

    private bool _isPointerDown;

#if UNITY_EDITOR
    private void Start()
    {
        float ratio = GetMainGameViewSize().x / _resolutionRefence.x;

        _compensatedSensitivity = _sensitivity / ratio;
    }
#else
 private void Start()
    {
        float ratio = Screen.currentResolution.width / _resolutionRefence.x;

        _compensatedSensitivity = _sensitivity / ratio;
    }
#endif


    protected override string controlPathInternal
    {
        get => _inputAxis;
        set => _inputAxis = value;
    }

    private void LateUpdate()
    {
        if (_isDragging)
        {
            _deltaDrag.x *= -1;
            _input.look = _deltaDrag * _compensatedSensitivity;
            _isDragging = false;
            return;
        }

        _input.look = Vector2.zero;
    }

    public void OnDrag(PointerEventData eventData)
    {
        _isDragging = true;

        _deltaDrag = _currentPos - eventData.position;

        _currentPos = eventData.position;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _currentPos = eventData.position;
    }

    public static Vector2 GetMainGameViewSize()
    {
        System.Type T = System.Type.GetType("UnityEditor.GameView,UnityEditor");
        System.Reflection.MethodInfo GetSizeOfMainGameView = T.GetMethod("GetSizeOfMainGameView", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
        object Res = GetSizeOfMainGameView.Invoke(null, null);
        return (Vector2)Res;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _pressDuration = 0;
        _input.move = Vector2.zero;
        _isPointerDown = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _isPointerDown = true;
    }
    private void Update()
    {
        print($"isPointerDown: {_isPointerDown} isDragging: {_isDragging}");

        if(_isPointerDown)
        {
            if (_isDragging)
            {
                _pressDuration = -1;
                return;
            }

            if (_pressDuration < 0) return;

            _pressDuration += Time.deltaTime;

            if (_pressDuration >= _moveForwardMinDuration)
            {
                print($"Move");
                _input.move = Vector2.up;
            }
        }
        
    }
}
