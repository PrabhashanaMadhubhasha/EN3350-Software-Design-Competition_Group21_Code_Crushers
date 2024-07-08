using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaveCitizen : MonoBehaviour
{
    NPC npc;
    Transform child;

    Constructable constructable;

    public bool inActive = true;
    // Start is called before the first frame update
    void Start()
    {
        child = transform.GetChild(0);
        // Check if the child has the NPC component
        npc = child.GetComponent<NPC>();
        // Get the Constructable component from the parent GameObject
        constructable = GetComponentInParent<Constructable>();
    }

    // Update is called once per frame
    void Update()
    {
        // If w havent already tlk about the problem with citiaen and happenes is reduced under 1.5 then citizen will leave the city and Upon API
        if(!npc.currentActiveQuest.accepted && (AssetsManager.Instance.currentCitizenHappiness < 1.5f || AssetsManager.Instance.leaveCitizensFromAPI))
        {
            if (inActive)
            {
                inActive = false;   
                // If NPC component exists, activate the GameObject
                if (npc != null)
                {
                    child.gameObject.SetActive(false);
                }
                if (constructable != null)
                {
                    constructable.DeactiveConsumptionProduction();
                }
            }
        }
        // Agin if we are ble increase the happines then citizens will appear
        if(AssetsManager.Instance.currentCitizenHappiness > 3.5f)
        {
            
            if (!inActive)
            {
                inActive = true;
                if (npc != null)
                {
                    child.gameObject.SetActive(true);
                }
                if (constructable != null)
                {
                    constructable.ActiveConsumptionProduction();
                }
            }
        }
    }
}
