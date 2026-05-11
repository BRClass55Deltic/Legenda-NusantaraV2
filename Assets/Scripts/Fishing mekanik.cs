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

    float fishposition;
    float fishDestination;

    float fishtimer;
    [SerializeField] float timeMultiplication = 3f;

    float fishspeed;
    [SerializeField] float smoothMotion = 1f;

    [SerializeField] Transform hook;
    float hookPosition;
    [SerializeField] float Hooksize=0.1f;
    [SerializeField] float Hookpower=0.5f;
    float hookProgress;
    float hookpullvelocity;
    [SerializeField] float hookpullpower=0.01f;
    [SerializeField] float hookpullGravityPower=0.005f;
    [SerializeField] float hookprogressDegradtionpower=0.1f;

    [SerializeField] RawImage HookImage;
    [SerializeField] Transform ProgressBar;

    bool pause=false;

    [SerializeField] float failTime = 10f;
    // Start is called before the first frame update
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
        ls.y=(distance/ysize*Hooksize);
        hook.localScale = ls;
    }

    // Update is called once per frame
    void Update()
    {
        if (pause)
        {
            return;
        }
        Fish();
        Hook();
        ProgressCheck();
    }

    private void ProgressCheck()
    {
        Vector3 ls = ProgressBar.localScale;
        ls.y = hookProgress;
        ProgressBar.localScale = ls;

        float min = hookPosition - Hooksize / 2;
        float max = hookPosition + Hooksize / 3;

        if (min < fishposition && fishposition<max)
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
        if(hookProgress >=1f)
        {
            Win();
        }

        hookProgress = Mathf.Clamp(hookProgress, 0f, 1f);

    }

    private void Hook()
    {
        if(Input.GetMouseButton(0)) 
        {
            hookpullvelocity += hookpullpower * Time.deltaTime;
            Debug.Log("Mouse ditekan");
        }
        hookpullvelocity -= hookpullGravityPower * Time.deltaTime;

        hookPosition += hookpullvelocity;
        hookPosition = Mathf.Clamp(hookPosition, Hooksize/2, 1-Hooksize/2);
        hook.position = Vector3.Lerp(botompivot.position, toppivot.position, hookPosition);
    }

    private void Fish()
    {
        fishtimer -= Time.deltaTime;
        if (fishtimer < 0)
        {
            fishtimer = UnityEngine.Random.value * timeMultiplication;

            fishDestination = UnityEngine.Random.value;
        }

        fishposition = Mathf.SmoothDamp(fishposition, fishDestination, ref fishspeed, smoothMotion);
        fish.position = Vector3.Lerp(botompivot.position, toppivot.position, fishposition);
        
    }

    private void Win()
    {
        pause = true;
        Debug.Log("WIn!");
    }

    private void Lose()
    {
        pause = true;
        Debug.Log("you lose!");
    }
}
