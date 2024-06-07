using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public bool playerInRange;
    public string ItemName;
    public bool notPickable;

    public string GetItemName()
    {
        return ItemName;
    }
    void Update()
    {
        // Pickup the item on the environment
        if (Input.GetKeyDown(KeyCode.E) && !notPickable && playerInRange && SelectionManager.instance.onTarget && SelectionManager.instance.selectedObject == gameObject)
        {
            if (InventorySystem.Instance.CheckSlotsAvailable(1))
            {

                InventorySystem.Instance.AddToInventory(ItemName);

                Destroy(gameObject);

            }
            else
            {
                Debug.Log("The Inventory is Full");
            }
        }

    }

    // Check the player near the object
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}