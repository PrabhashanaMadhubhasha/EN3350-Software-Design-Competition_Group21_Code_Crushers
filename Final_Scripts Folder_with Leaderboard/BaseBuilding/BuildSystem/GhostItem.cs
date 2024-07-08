using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostItem : MonoBehaviour
{
    [Header("Colliders")]
    public BoxCollider solidCollider; // set manually
    public MeshCollider meshCollider;

    [Header("Material Related")]
    public Renderer mRenderer;
    public Renderer mRendererChild;
    public GameObject constructionObject;
    private Material semiTransparentMat; // Used for debug - insted of the full trasparent
    private Material fullTransparentMat;
    private Material selectedMaterial;

    public bool isPlaced;

    // A flag for the deletion algorithm
    public bool hasSamePosition = false;
    private void Start()
    {
        mRenderer = GetComponent<Renderer>();
        meshCollider = gameObject.GetComponent<MeshCollider>();
        constructionObject = gameObject; // GetComponent<GameObject>();
        
        // We get them from the manager, because this way the referece always exists.
        semiTransparentMat = ConstructionManager.Instance.ghostSemiTransparentMat;
        fullTransparentMat = ConstructionManager.Instance.ghostFullTransparentMat;
        selectedMaterial = ConstructionManager.Instance.ghostSelectedMat;

        // SetMaterialOfRenderer;
        SetMaterial(fullTransparentMat);

        // We disable the solid box collider - while it is not yet placed
        SetCollider(false);
    }

    private void Update()
    {
        if (ConstructionManager.Instance.inConstructionMode)
        {

            Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), ConstructionManager.Instance.player.GetComponent<Collider>());


            // We need the solid collider so the ray cast will detect it
            if (isPlaced)
            {
                SetCollider(true);
            }

            // Triggering the material
            if (ConstructionManager.Instance.selectedGhost == gameObject)
            {
                SetMaterial(selectedMaterial);
            }
            else
            {
                SetMaterial(fullTransparentMat);
            }
        }
        else
        {
            if (ConstructionManager.Instance.inAfterConstruction)
            {
                SetCollider(false);
            }
        }     
    }

    public void SetMaterial(Material materialToSet)
    {
        try
        {
            if (mRenderer != null)
            {
                SetMaterialOfRenderer(materialToSet, mRenderer);
            }
            else
            {
                for (int i = 0; i < gameObject.transform.childCount; i++)
                {

                    GameObject child = gameObject.transform.GetChild(i).gameObject;
                    mRendererChild = child.GetComponent<Renderer>();
                    SetMaterialOfRenderer(materialToSet, mRendererChild);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Unable to change the material of the renderer: " + ex.Message);
        }
    }


    public void SetMaterialOfRenderer(Material materialToSet, Renderer renderer)
    {
        Material[] materials = renderer.materials;
        if (materials.Length == 1)
        {
            renderer.material = materialToSet; //Green
        }
        else
        {
            for (int i = 0; i < materials.Length; i++)
            {
                materials[i] = materialToSet;
            }
            renderer.materials = materials;
        }
    }

    // Change the collider Active or Deactive 
    public void SetCollider(bool enable)
    {
        try
        {
            if (solidCollider != null)
            {
                solidCollider.enabled = enable;
            }

            if (meshCollider != null)
            {
                meshCollider.enabled = enable;
            }
            if (gameObject.transform.childCount > 0)
            {
                for (int i = 0; i < gameObject.transform.childCount; i++)
                {
                    GameObject child = gameObject.transform.GetChild(i).gameObject;
                    MeshCollider meshColliderChild = child.GetComponent<MeshCollider>();
                    if (meshColliderChild != null)
                    {
                        meshColliderChild.enabled = enable;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Unable to chgane the state of the collider: " + ex.Message);
        }

    }
}