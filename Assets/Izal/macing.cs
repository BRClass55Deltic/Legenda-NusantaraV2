using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class macing : MonoBehaviour
{
    public GameObject pancinganinterktik;
    public GameObject kait;
    public MonoBehaviour player;
    public Animator anim;
    public string boolAnimasi = "mancing";
    public GameObject mancingUI;
    // Start is called before the first frame update
    void Start()
    {
        mancingUI.SetActive(false);
        kait.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!pancinganinterktik.activeSelf)
        {
            mancingUI.SetActive(true);
            kait.SetActive(true);
            player.enabled = false;
            player.transform.position = new Vector3(185.8949f, 22.831f, 255.3752f);
            player.transform.rotation = Quaternion.Euler(0f, 33.617f, 0f);
            anim.SetBool("mancing", true);

        }
    }
}
