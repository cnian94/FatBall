using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingProgress : MonoBehaviour
{

    public Image fillImage;
    public Text textLabel;

    private float value = 0f;
    private bool isDone = false;

    private void Awake()
    {
        //DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        this.fillImage.fillAmount = 0f;
    }

    private void Update()
    {
        if (this.isDone)
        {
            if(this.OnDoneEvent != null)
            {
                this.OnDoneEvent();
            }
            this.OnDoneEvent = null;
            this.OnChangeEvent = null;
            return;
        }

        if(this.OnChangeEvent != null)
        {
            this.OnChangeEvent((int)(this.value * 100));
        }

        this.fillImage.fillAmount = this.value;
        this.textLabel.text = (this.value > 1) ? "Done!" : (int)(this.value * 100) + "%";
        this.isDone = (this.value > 1) ? true : false;
    }

    public void SetValue(float val)
    {
        this.value = val;
    }

    public float GetValue()
    {
        return this.value;
    }


    public void OnChange(ValueChange method)
    {
        this.OnChangeEvent += method;
    }

    public void OnDone(ProgressDone method)
    {
        this.OnDoneEvent += method;
    }


    public delegate void ValueChange(float value);
    private event ValueChange OnChangeEvent;

    public delegate void ProgressDone();
    private event ProgressDone OnDoneEvent;

}
