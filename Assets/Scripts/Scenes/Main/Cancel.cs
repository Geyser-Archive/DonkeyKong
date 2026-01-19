using UnityEngine;

public class Cancel : MonoBehaviour
{
    private ButtonOnAction buttonOnAction;

    private void Awake()
    {
        this.buttonOnAction = base.GetComponent<ButtonOnAction>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            //this.buttonOnAction.Call();
        }
    }
}