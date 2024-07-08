using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]  
public class ChoppableTree : MonoBehaviour
{
    public string objectName;
    public bool playerInRange;
    public bool canBeChopped;

    public float objectMaxHealth;
    public float objectHealth;

    public Animator animator;
    public float caloriesSpent = 20;

    private void Start()
    {
        objectName = GetComponent<InteractableObject>().ItemName;    
        objectHealth = objectMaxHealth;
        animator = transform.parent.transform.parent.GetComponent<Animator>();

        if (canBeChopped)
        {
            // Set the health of the object
            GlobalState.Instance.resourceHealth = objectHealth;
            GlobalState.Instance.resourceMaxHealth = objectMaxHealth;
            GlobalState.Instance.objectName = objectName;
        }
    }

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

    // After hitting the Axe
    public void GetHit()
    {
        objectHealth -= 1;

        PlayerState.Instance.currentCalories -= caloriesSpent;

        animator.SetTrigger("shake");

        if (objectHealth <= 0)
        {
            ObjectIsBroken();
        }

        if (canBeChopped)
        {
            GlobalState.Instance.resourceHealth = objectHealth;
            GlobalState.Instance.resourceMaxHealth = objectMaxHealth;
            GlobalState.Instance.objectName = objectName;
        }
    }

    void ObjectIsBroken()
    {
        Vector3 treePosition = transform.position;

        Destroy(transform.parent.transform.parent.gameObject);
        canBeChopped = false;
        SelectionManager.instance.selectedTree = null;
        SelectionManager.instance.chopHolder.gameObject.SetActive(false);

        string firstFiveChars = objectName.Substring(0, 5);
        if (firstFiveChars == "Tree ")
        {
            SoundManager.Instance.PlaySound(SoundManager.Instance.treeFallingSound);
            Instantiate(Resources.Load<GameObject>("ChoppedTree"), new Vector3(treePosition.x, treePosition.y, treePosition.z), Quaternion.Euler(0, 0, 0));
        }
        else if(firstFiveChars == "RockM")
        {
            SoundManager.Instance.PlaySound(SoundManager.Instance.treeFallingSound); //Have to put rock broken ound
            Instantiate(Resources.Load<GameObject>("ChoppedRockMedium"), new Vector3(treePosition.x, treePosition.y, treePosition.z), Quaternion.Euler(0, 0, 0));
        }
        else if (firstFiveChars == "RockL")
        {
            SoundManager.Instance.PlaySound(SoundManager.Instance.treeFallingSound); //Have to put rock broken ound
            Instantiate(Resources.Load<GameObject>("ChoppedRockLarge"), new Vector3(treePosition.x, treePosition.y, treePosition.z), Quaternion.Euler(0, 0, 0));
        }

    }

    // Ntuarally increse the health of the Tree or Rock
    public IEnumerator naturallyHealthUp()
    {
        yield return new WaitForSeconds(0.6f);
        if(objectHealth >= objectMaxHealth / 2)
        {
            objectHealth += 0.001f;
        }
    }

    private void Update()
    {
        if(objectHealth < objectMaxHealth)
        {
            StartCoroutine(naturallyHealthUp());
        }
    }
}
