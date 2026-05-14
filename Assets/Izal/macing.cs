using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class macing : MonoBehaviour
{
    public GameObject pancinganinterktik;
    public GameObject kait;
    public GameObject pancingan;

    public Fishingmekanik fishingMekanik;

    public MonoBehaviour player;

    public Animator anim;

    public GameObject mancingUI;
    public MonoBehaviour Pancing;

    bool isFishing = false;

    void Start()
    {
        mancingUI.SetActive(false);
        kait.SetActive(false);

        Pancing.enabled = false;
    }

    void Update()
    {
        // Mulai mancing
        if (pancinganinterktik.activeSelf && Input.GetKeyDown(KeyCode.E))
        {
            StartFishing();
        }

        if(!pancinganinterktik.activeSelf)
        {
            anim.SetBool("sedang mancing", true);
        }

        if(pancinganinterktik.activeSelf)
        {
            anim.SetBool("sedang mancing", false);
        }

        // Keluar mancing
        if (isFishing && Input.GetKeyDown(KeyCode.Q))
        {
            StopFishing();
        }

        if (fishingMekanik.IsIdle())
        {
            kait.SetActive(false);
        }
    }


    void StartFishing()
    {
        isFishing = true;

        pancinganinterktik.SetActive(false);
        pancingan.SetActive(true);

        mancingUI.SetActive(true);
        kait.SetActive(true);

        player.enabled = false;

        player.transform.position = new Vector3(185.8949f, 22.831f, 255.3752f);

        player.transform.rotation = Quaternion.Euler(0f, 33.617f, 0f);

        Pancing.enabled = true;

        Debug.Log("Mulai mancing");
    }

    void StopFishing()
    {
        isFishing = false;

        pancinganinterktik.SetActive(true);
        pancingan.SetActive(false);

        mancingUI.SetActive(false);
        kait.SetActive(false);

        player.enabled = true;

        Pancing.enabled = false;

        Debug.Log("Berhenti mancing");
    }
}