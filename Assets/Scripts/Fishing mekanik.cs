using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Fishingmekanik : MonoBehaviour
{
    [SerializeField] Transform toppivot;
    [SerializeField] Transform botompivot;
    [SerializeField] Transform fish;
    [SerializeField] RandomObjManager questManager;
    public GameObject kait;
    
    float fishposition;
    float fishDestination;

    float fishtimer;
    [SerializeField] float timeMultiplication = 3f;

    float fishspeed;
    [SerializeField] float smoothMotion = 1f;

    [SerializeField] Transform hook;
    float hookPosition;
    [SerializeField] float Hooksize = 0.1f;
    [SerializeField] float Hookpower = 0.5f;
    float hookProgress;
    float hookpullvelocity;
    [SerializeField] float hookpullpower = 0.01f;
    [SerializeField] float hookpullGravityPower = 0.005f;
    [SerializeField] float hookprogressDegradtionpower = 0.1f;

    [SerializeField] RawImage HookImage;
    [SerializeField] Transform ProgressBar;

    public Animator Kaitanim;
    public Animator anim;
    bool pause = false;

    bool sedangMancing = false;

    // TAMBAHAN
    bool idle = true;

    [SerializeField] float failTime = 10f;

    void Start()
    {
        Resize();
    }

    private void Resize()
    {
        RectTransform rect = HookImage.rectTransform;
        float ysize = rect.rect.height;

        Vector3 ls = hook.localScale;
        float distance = Vector3.Distance(toppivot.position, botompivot.position);

        ls.y = (distance / ysize * Hooksize);
        hook.localScale = ls;
    }

    void Update()
    {
        // Jika masih idle, tunggu SPACE
        if (idle)
        {
            Idle();
            return;
        }

        if (pause)
        {
            return;
        }

        if (sedangMancing)
        {
            SedangMancing();
        }

        Fish();
        Hook();
        ProgressCheck();
    }

    // TAMBAHAN
    public void Idle()
    {
        kait.SetActive(false);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            anim.SetInteger("system", 2);
            idle = false;
            sedangMancing = true;
            kait.SetActive(true);
            Debug.Log("Mini game dimulai!");
        }
    }

    public bool IsIdle()
    {
        return idle;
    }

    private void SedangMancing()
    {
        kait.SetActive(true);

        Kaitanim.SetBool("kait idle", true);
        Kaitanim.SetBool("kait diangkat", false);
    }

    private void ProgressCheck()
    {
        Vector3 ls = ProgressBar.localScale;
        ls.y = hookProgress;
        ProgressBar.localScale = ls;

        float min = hookPosition - Hooksize / 2;
        float max = hookPosition + Hooksize / 3;

        if (min < fishposition && fishposition < max)
        {
            hookProgress += Hookpower * Time.deltaTime;
        }
        else
        {
            hookProgress -= hookprogressDegradtionpower * Time.deltaTime;

            failTime -= Time.deltaTime;

            if (failTime < 0)
            {
                Lose();
            }
        }

        if (hookProgress >= 1f)
        {
            Win();
        }

        hookProgress = Mathf.Clamp(hookProgress, 0f, 1f);
    }

    private void Hook()
    {
        if (Input.GetMouseButton(0))
        {
            hookpullvelocity += hookpullpower * Time.deltaTime;
        }

        hookpullvelocity -= hookpullGravityPower * Time.deltaTime;

        hookpullvelocity = Mathf.Clamp(hookpullvelocity, -0.02f, 0.02f);

        hookPosition += hookpullvelocity;

        hookPosition = Mathf.Clamp(hookPosition, Hooksize / 2, 1 - Hooksize / 2);

        if (hookPosition <= Hooksize / 2 || hookPosition >= 1 - Hooksize / 2)
        {
            hookpullvelocity = 0f;
        }

        hook.position = Vector3.Lerp(
            botompivot.position,
            toppivot.position,
            hookPosition
        );
    }
    private void Fish()
    {
        fishtimer -= Time.deltaTime;

        if (fishtimer < 0)
        {
            fishtimer = UnityEngine.Random.value * timeMultiplication;
            fishDestination = UnityEngine.Random.value;
        }

        fishposition = Mathf.SmoothDamp(
            fishposition,
            fishDestination,
            ref fishspeed,
            smoothMotion
        );

        fish.position = Vector3.Lerp(
            botompivot.position,
            toppivot.position,
            fishposition
        );
    }

    private void ResetGame()
    {
        idle = true;
        pause = false;

        hookProgress = 0f;
        failTime = 10f;

        hookpullvelocity = 0f;
        hookPosition = 0.5f;

        fishposition = 0.5f;
        fishDestination = Random.value;
    }

    public void Win()
    {
        anim.SetInteger("system", 3);
        anim.SetBool("menang", true);
        sedangMancing = false;
        Kaitanim.SetBool("kait idle",false);
        Kaitanim.SetBool("kait diangkat", true);
        Kaitanim.SetBool("kait idle", false);
        Debug.Log("WIN!");

        if (questManager != null)
        {
            questManager.CollectItem(null);
        }
        
        Invoke(nameof(ResetGame),0.35f);
        
    }

    public void Lose()
    {
        anim.SetInteger("system", 3);
        anim.SetBool("menang", false);
        sedangMancing = false;
        Kaitanim.SetBool("kait idle", false);
        Kaitanim.SetBool("kait diangkat", true);
        Kaitanim.SetBool("kait idle", false);
        Debug.Log("YOU LOSE!");
        Invoke(nameof(ResetGame), 0.35f);
        
    }
}