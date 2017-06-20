using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class GuiDrawLine : MonoBehaviour {
    
    Image image;
    public Material targetImg;
    public float adjustSpeed;

    public Vector2 pos { get; set; }
    public Vector2 size { get; set; }

    // Use this for initialization
    void Start () {
        //设置UI的长宽和方向
        //this.gameObject.AddComponent<Image>();
        image = this.GetComponent<Image>();
        image.material = targetImg;
        //image.fillMethod = Image.FillMethod.Vertical;
        this.SetRectTransform(pos, size);
        image.fillAmount = 0;

        if (adjustSpeed==0)
        {
            adjustSpeed = 0.5f;
        }
        
    }
	
	// Update is called once per frame
	void Update () {
        {
            image.fillAmount += 1.0f / adjustSpeed * Time.deltaTime;
        }
    }

    void SetRectTransform(Vector2 pos, Vector2 size)
    {
        RectTransform rect;
        //设置位置（仅一次）
        rect = this.GetComponent<RectTransform>();
        rect.position = pos;
        rect.sizeDelta = size;
    }
}
