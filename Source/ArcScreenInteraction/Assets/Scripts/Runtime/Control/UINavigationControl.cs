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
                AppManager.Instance.EnterAction?.Invoke();
                submitEvent?.Raise();
                break;
            case NetworkInputAction.Option:
                AppManager.Instance.SettingAction?.Invoke();
                optionEvent?.Raise();
                break;
            case NetworkInputAction.Back:
                AppManager.Instance.BackAction?.Invoke();
                AppManager.Instance.BackAction2?.Invoke();
                backEvent?.Raise();
                break;
            case NetworkInputAction.Home:
                AppManager.Instance.HomeAction?.Invoke();
                homeEvent?.Raise();
                break;
            default:
                break;
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            Move(MoveDirection.Up);
        }
        else if (Input.GetKeyDown(KeyCode.K))
        {
            Move(MoveDirection.Down);
        }
        else if (Input.GetKeyDown(KeyCode.J))
        {
            Move(MoveDirection.Left);
            AppManager.Instance.LeftAction?.Invoke();
        }
        else if (Input.GetKeyDown(KeyCode.L))
        {
            Move(MoveDirection.Right);
            AppManager.Instance.RightAction?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            Submit();
            AppManager.Instance.EnterAction?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            AppManager.Instance.BackAction?.Invoke();
            AppManager.Instance.BackAction2?.Invoke();
            Debug.Log("BackAction0");
        }

        if (Input.GetKeyDown(KeyCode.F8))
        {
            AppManager.Instance.HomeAction?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            AppManager.Instance.SettingAction?.Invoke();
        }


        if (eventSystem.currentSelectedGameObject)
        {
            lastSelected = eventSystem.currentSelectedGameObject;
        }
        else
        {
            if (lastSelected != null)
            {
                eventSystem.SetSelectedGameObject(lastSelected);
            }
        }
    }
}