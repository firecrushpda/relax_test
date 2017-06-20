using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Canvas))]
public class DrawLineManager : MonoBehaviour {
    public int lineNum;
    Canvas canvas;
	// Use this for initialization
	void Start () {
        if (lineNum == 0)
        {
            lineNum = 2;
        }
        //this.gameObject.AddComponent<Canvas>();
        //canvas = this.GetComponent<Canvas>();
        InitLinesBylineNum(lineNum, 0);
        InitLinesBylineNum(lineNum, 1);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    void InitLinesBylineNum(int lineNum, int FillMethod)
    {
        Rect[] rect = CalRectByScreen(lineNum, FillMethod);
        for (int i = 0; i < lineNum; i++)
        {
            GameObject line = new GameObject();
            line.gameObject.name =i.ToString();
            line.AddComponent<GuiDrawLine>();
            //line.AddComponent<Image>();
            line.transform.SetParent(this.transform);
            GuiDrawLine gdl = line.GetComponent<GuiDrawLine>();
            //Image img = line.GetComponent<Image>();

            gdl.pos = new Vector2(rect[i].x, rect[i].y);
            gdl.size = new Vector2(rect[i].width, rect[i].height);
            
        }
    }
    //通过屏幕大小计算每个dl的位置
    Rect[] CalRectByScreen(int lineNum,int FillMethod)
    {
        Rect[] rect = new Rect[lineNum];
        float index = 0;
        float comp = 0;
        //
        if (FillMethod == 0)
        {
           comp = Screen.width / lineNum;

            for (int i = 0; i < lineNum; i++)
            {
                index += comp;
                rect[i] = new Rect(index,0,2,Screen.height);
            }
        }
        else if (FillMethod == 1)
        {
           comp = Screen.height / lineNum;

            for (int i = 0; i < lineNum; i++)
            {
                index += comp;
                rect[i] = new Rect(0,index,Screen.width,2);
            }
        }
       
        return rect;
    }
}
