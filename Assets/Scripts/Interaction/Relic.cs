using UnityEngine;

public class Relic : PickUp
{
    [SerializeField] private GameObject rotateObject;
    
    public override void OnInteract()
    {
        InputManager.Instance.SwitchActionMap("RotateObject");
        rotateObject.SetActive(true);
    }
}
