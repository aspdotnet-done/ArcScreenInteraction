using UnityEngine;
using UnityEngine.UI;

public class ToggleSelect : MonoBehaviour
{
    [SerializeField] private Toggle toggle;
    [SerializeField] private GameObject normal;
    [SerializeField] private GameObject select;

    private void Start()
    {
        toggle.onValueChanged.AddListener(OnToggleValueChanged);
    }
    private void OnToggleValueChanged(bool value)
    {
        Debug.Log("OnToggleValueChanged:" + value);
        if (value)
        {
            normal.SetActive(false);
            select.SetActive(true);
        }
        else
        {
            normal.SetActive(true);
            select.SetActive(false);
        }

    }

}
