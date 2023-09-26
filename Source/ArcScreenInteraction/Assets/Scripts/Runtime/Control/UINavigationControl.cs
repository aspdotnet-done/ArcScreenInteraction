using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using ScriptableObjectArchitecture;

public class UINavigationControl : MonoBehaviour
{
    EventSystem eventSystem;
    [SerializeField]
    private MessageActionMapper messageActionMapper;
    [SerializeField] GameEvent leftEvent;
    [SerializeField] GameEvent rightEvent;
    [SerializeField] GameEvent upEvent;
    [SerializeField] GameEvent downEvent;
    [SerializeField] GameEvent submitEvent;
    [SerializeField] GameEvent optionEvent;
    [SerializeField] GameEvent backEvent;
    [SerializeField] GameEvent homeEvent;

    #region Unity Functions
    private void Awake()
    {
        eventSystem = EventSystem.current;
    }
    private void OnEnable()
    {
        NetworkInput.OnMessageReceived += OnMessageReceived;
    }
    private void OnDisable()
    {
        NetworkInput.OnMessageReceived -= OnMessageReceived;
    }
    private void Update()
    {
        string simulateInputString = Input.inputString;
        if (Input.GetKeyDown(KeyCode.I))
        {
            simulateInputString = "up";
        }
        else if (Input.GetKeyDown(KeyCode.K))
        {
            simulateInputString = "down";
        }
        else if (Input.GetKeyDown(KeyCode.J))
        {
            simulateInputString = "left";
        }
        else if (Input.GetKeyDown(KeyCode.L))
        {
            simulateInputString = "right";
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            simulateInputString = "home";
        }
        else if (Input.GetKeyDown(KeyCode.U))
        {
            simulateInputString = "confirm";
        }
        else if (Input.GetKeyDown(KeyCode.O))
        {
            simulateInputString = "option";
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            simulateInputString = "back";
        }
        OnMessageReceived(simulateInputString);


        if (eventSystem.currentSelectedGameObject)
        {
            lastSelected = eventSystem.currentSelectedGameObject;
        }
        else
        {
            if (lastSelected != null && eventSystem.currentSelectedGameObject == null)
            {
                eventSystem.SetSelectedGameObject(lastSelected);
            }
        }
    }
    #endregion
    private GameObject lastSelected;
    public void Move(MoveDirection direction)
    {
        AxisEventData data = new AxisEventData(EventSystem.current);
        data.moveDir = direction;
        data.selectedObject = EventSystem.current.currentSelectedGameObject;
        ExecuteEvents.Execute(data.selectedObject, data, ExecuteEvents.moveHandler);
    }

    public void Submit()
    {
        var selectedObject = eventSystem.currentSelectedGameObject;
        if (selectedObject == null)
            return;
        //实现按钮点击
        Debug.Log("Submit");
        ExecuteEvents.Execute(selectedObject, new BaseEventData(EventSystem.current), ExecuteEvents.submitHandler);
    }
    private void OnMessageReceived(string message)
    {
        NetworkInputAction action = messageActionMapper.GetAction(message);
        switch (action)
        {
            case NetworkInputAction.Up:
                Move(MoveDirection.Up);
                upEvent?.Raise();
                break;
            case NetworkInputAction.Down:
                Move(MoveDirection.Down);
                downEvent?.Raise();
                break;
            case NetworkInputAction.Left:
                Move(MoveDirection.Left);
                leftEvent?.Raise();
                break;
            case NetworkInputAction.Right:
                Move(MoveDirection.Right);
                rightEvent?.Raise();
                break;
            case NetworkInputAction.Submit:
                Submit();
                submitEvent?.Raise();
                break;
            case NetworkInputAction.Option:
                optionEvent?.Raise();
                break;
            case NetworkInputAction.Back:
                backEvent?.Raise();
                break;
            case NetworkInputAction.Home:
                homeEvent?.Raise();
                break;
            default:
                break;
        }
    }

}