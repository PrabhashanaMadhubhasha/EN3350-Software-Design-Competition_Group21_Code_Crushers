using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public HealthSystem healthSystem;   
    public static PlayerState Instance { get; set; }

    [Header("Player Health")]
    public float currentHealth;
    public float maxHealth;

    [Header("Player Calories")]
    public float currentCalories;
    public float maxCalories;

    float distanceTravelled = 0;
    Vector3 lastPosition;

    public GameObject playerBody;
    public CharacterController characterController;

    [Header("Player Hydration")]
    public float currentHydrationPercent;
    public float maxHydrationPercent;

    public bool isHydrationActive = true;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        currentHealth = maxHealth;
        currentCalories = maxCalories;
        currentHydrationPercent = maxHydrationPercent;
        characterController = playerBody.GetComponent<CharacterController>();

    }

    void Update()
    {
        distanceTravelled += Vector3.Distance(playerBody.transform.position, lastPosition);
        lastPosition = playerBody.transform.position;

        if(distanceTravelled >= 5)
        {
            distanceTravelled = 0;
            currentCalories -= 1;
        }

        currentHealth = healthSystem.health;

        if (currentCalories < maxCalories)
        {
            StartCoroutine(naturallyCaloriesUp());
        }
    }

    // Nturally Calores incresing
    public IEnumerator naturallyCaloriesUp()
    {
        yield return new WaitForSeconds(0.001f);
        currentCalories += 0.05f;
    }

    // when we are loading we have to set loaded position and rotation of the player
    public void SetLoadPositionRotation(Vector3 position, Vector3 rotation)
    {
        characterController.enabled = false; // Disable the CharacterController
        playerBody.transform.SetPositionAndRotation(position, Quaternion.Euler(rotation));
        characterController.enabled = true;
        
    }

    public void setHealth(float health)
    {
        currentHealth = health;
        healthSystem.health = currentHealth;
    }

    public void setCalories(float calories)
    {
        currentCalories = calories;
    }

    public void setHydration(float hydration)
    {
        currentHydrationPercent = hydration;
    }
}
