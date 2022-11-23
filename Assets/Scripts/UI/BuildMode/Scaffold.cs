using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script for constructing a scaffold over time
/// Creator: Benjamin
/// </summary>
public class Scaffold : MonoBehaviour
{
    #region Variables

    private MeshRenderer scafRenderer;
    private Coroutine scafRoutine;
    private GameObject module;
    public GameObject Module 
    {
        get => module;
        set => module = value;
    }
    
    [SerializeField] private Material refMaterial;
    private Material actualMat;

    [SerializeField] private float timeToBuild;
    private float timer;



    #endregion

    #region Unity Events

    // Start is called before the first frame update
    void Start()
    {
        timer = timeToBuild;
        scafRenderer = this.GetComponent<MeshRenderer>();
        actualMat = new Material(refMaterial);
        scafRoutine = StartCoroutine(BuildScaffold());
    }

    #endregion

    #region Coroutines

    /// <summary>
    /// Coroutine for pacing buildingconstruction
    /// </summary>
    /// <returns></returns>
    private IEnumerator BuildScaffold()
    {
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            float buildProgess = Mathf.Lerp(0.9f, 2.1f, timer / timeToBuild);
            actualMat.SetFloat("_AlphaTreshhold", buildProgess);
            scafRenderer.material = actualMat;
            yield return null;
        }

        //Make Module visible and destroying itself
        Module.SetActive(true);
        Destroy(this.transform.parent.gameObject);
    }

    #endregion
}