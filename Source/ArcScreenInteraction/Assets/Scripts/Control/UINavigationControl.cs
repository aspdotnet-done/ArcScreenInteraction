using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UINavigationControl : MonoBehaviour
{
    EventSystem eventSystem;
    [SerializeField]
    private MessageActionMapper messageActionMapper;
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
                break;
            case NetworkInputAction.Down:
                Move(MoveDirection.Down);
                break;
            case NetworkInputAction.Left:
                Move(MoveDirection.Left);
                break;
            case NetworkInputAction.Right:
                Move(MoveDirection.Right);
                break;
            case NetworkInputAction.Submit:
                Submit();
                break;
            case NetworkInputAction.Option:
                AppManager.Instance.SettingAction?.Invoke();
                break;
            case NetworkInputAction.Back:
                AppManager.Instance.BackAction?.Invoke();
                break;
            case NetworkInputAction.Home:
                AppManager.Instance.HomeAction?.Invoke();
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
        }
        else if (Input.GetKeyDown(KeyCode.L))
        {
            Move(MoveDirection.Right);
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            Submit();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            AppManager.Instance.BackAction?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.F8))
        {
            AppManager.Instance.HomeAction?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            AppManager.Instance.SettingAction?.Invoke();
        }
    }
}