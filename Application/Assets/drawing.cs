using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class drawing : MonoBehaviour {

    public Camera Cam;
    public GUISkin skin;
    public Texture2D pencil,pencilactive,rubber,rubberactive;
    public Image img; //blit texture
    RenderTexture rt; //Render texture 
    public GUISkin gskin;  //Skin for Color wheel
    public Texture2D colorCircle; //Color Picker Image
    public Material material; //brush material
    private Mesh mCurr; //Currently Active point size mesh
    int isEraser;
    float currSize, erasersize, pencilsize;
      Vector3 bezier_generated_point; 
    List<Vector3> three_point; //Store Last Three touch Point
    Color currColor; //Current picked color
    public LineRenderer lineRenderer; //liner Renderer Object
    private bool canDraw;
  //  public MonoBehaviour antialising;
    Rect colorCircleRect,UIRect;

    IEnumerator Start () 
    {
        //disable Eraser on start
        isEraser=0;

        //Intialise brush size on start
        erasersize=pencilsize=currSize=2;

        //Intitalise brush color
        currColor = Color.red;

        //Initialise three points list
        three_point = new List<Vector3>();

        colorCircleRect = new Rect(Screen.width - 230f, Screen.height - 200f, 230f, 200f);
        UIRect = new Rect(0f, Screen.height - 100f,400f, 100f);
      //  GL.Clear(false, true, new Color(0.0f, 0.0f, 0.0f, 0.0f));
        //Create render texture and asssign it to camera..
        Cam.targetTexture = null;
      
        rt =new  RenderTexture(Screen.width, Screen.height, 0, RenderTextureFormat.Default);
      
        yield return rt.Create();
		Debug.Log((int)img.rectTransform.sizeDelta.x);
        Texture2D t = new Texture2D((int)img.rectTransform.sizeDelta.x, (int)img.rectTransform.sizeDelta.y);
        Graphics.Blit(t, rt);
        img.sprite = Sprite.Create(t, new Rect(Vector2.zero, new Vector2(t.width,t.height)), Vector2.one/2);
        yield return 0;
        Cam.targetTexture = rt;
        mCurr = CreatePlaneMesh(currSize *0.12f);
       // antialising.enabled = true;
        //bg.SetActive(false);
    }
    void OnDisable()
    {
       
    }
    
    void Update ()
    {
        if (colorCircleRect.Contains(new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y)) || UIRect.Contains(new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y))) //Check if Mouse position inside in Color Wheel
          return;
        
        if (Input.GetMouseButtonDown(0))
        {
            canDraw = true;
          
            //mCurr = isEraser == 1 ? m[erasersize - 1] : m[pencilsize - 1];//Curr Poitn Size Mesh
       
            mCurr = CreatePlaneMesh(currSize * 0.12f);
            material.color = isEraser == 1 ? Color.white : currColor; //Set color of brush Material
            lineRenderer.material.color = isEraser == 1 ? Color.white : currColor; //Set color of brush material
            lineRenderer.SetWidth(currSize * 0.2f, currSize * 0.2f); //Set line renderer width
            Vector3 v = Cam.ScreenToWorldPoint(Input.mousePosition); //Get Word Position of Mouse Position
            bezier_generated_point = new Vector3(v.x, v.y, 0);  
            three_point.Add(new Vector3(v.x, v.y, 0)); //Add Mouse Position in three point list
            Graphics.DrawMesh(mCurr, bezier_generated_point, Quaternion.Euler(new Vector3(-90, 0, 0)), material, 8); //Draw Dot on starting of drawing
        }
        else if (Input.GetMouseButton(0) && canDraw)
        {
        	Vector3 p = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -10);
            Vector3 v = -Cam.ScreenToWorldPoint(p);
            Debug.Log(Input.mousePosition + ":" +v);
            //check three point list contains new point and distance between last and this touch position 
            if (three_point.Count == 0 || (three_point.Count > 0 && Vector3.Distance(new Vector3(v.x, v.y, 0), three_point[three_point.Count - 1]) > 0.005f))
            {
                three_point.Add(new Vector3(v.x, v.y, 0));
                if (three_point.Count > 4)
                {
                    three_point.RemoveAt(0); //remove first point in three point list
                }
                if (three_point.Count == 4) //Three point list get 4 the point 
                {
                    lineRenderer.SetVertexCount(0);  //Initialise line renderer 
                    three_point[0] = bezier_generated_point; 
                    Vector3 points1 = Vector3.zero;
                    float dist = Vector3.Distance(three_point[0], three_point[1]);
                    int vertex = 1;
                    if (dist > 0f)
                    {
                        for (float t = 0; t <= 0.33f; t += 0.01f / dist)
                        {
                            //Bezeir curve equation for 4 points
                            points1 = (1f - t) * (1f - t) * (1f - t) * three_point[0] + 3 * (1f - t) * (1f - t) * t * three_point[1] + 3 * (1f - t) * t * t * three_point[2] + (t * t * t) * three_point[3];
                            bezier_generated_point = points1;

                            lineRenderer.SetVertexCount(vertex++); //Add new point in line renderer
                            lineRenderer.SetPosition(vertex - 2, points1); //Set point in line renderer

                        }

                        //Draw Dot On joints of each line renderer
                        Graphics.DrawMesh(mCurr, bezier_generated_point, Quaternion.Euler(new Vector3(-90, 0, 0)), material,8);

                    }
                }
            }

        }

        if (Input.GetMouseButtonUp(0) &&canDraw) //Call on Mouse Up
        {
            Vector3 v = Cam.ScreenToWorldPoint(Input.mousePosition);
            if (three_point.Count == 0 || (three_point.Count > 0 && Vector3.Distance(new Vector3(v.x, v.y, 0), three_point[three_point.Count - 1]) > 0.005f))
                three_point.Add(new Vector3(v.x, v.y, 0));
            if (three_point.Count > 4)
            {
                three_point.RemoveAt(0);
            }
            Graphics.DrawMesh(mCurr, bezier_generated_point, Quaternion.Euler(new Vector3(-90, 0, 0)), material,8);
            lineRenderer.SetVertexCount(0);

            if (three_point.Count == 4) //Draw curve between 4 points
            {
                int vertex = 1;

                Vector3 points1 = Vector3.zero;
                for (float t = 0; t <= 1f; t += 0.05f)
                {

                    //Bezeir curve equation for 4 points
                    points1 = (1f - t) * (1f - t) * (1f - t) * three_point[0] 
                    + 3 * (1f - t) * (1f - t) * t * three_point[1] + 3 * (1f - t) * t * t * three_point[2] + (t * t * t) * three_point[3];

                    bezier_generated_point = points1;

                    lineRenderer.SetVertexCount(vertex++);
                    lineRenderer.SetPosition(vertex - 2, points1);
                }

            }

            else if (three_point.Count == 3) //Draw curve between 3 points
            {
                int vertex = 1;

                Vector3 points1 = Vector3.zero;
                for (float t = 0; t <= 1f; t += 0.05f)
                {
                    //bezeir curve equation for 3 points
                    points1 = (1f - t) * (1f - t) * three_point[0] + 2 * (1f - t) * t * three_point[1] + (t * t) * three_point[2]; 
                    bezier_generated_point = points1;
                    lineRenderer.SetVertexCount(vertex++);
                    lineRenderer.SetPosition(vertex - 2, points1);
                }
            }
            else if (three_point.Count == 2) //Draw Line between 2 points
            {
                int vertex = 1;
                Vector3 points1 = Vector3.zero;
                for (float t = 0; t <= 1f; t += 0.05f)
                {
                    //Line Drawing Equation
                    points1 = (1f - t) * three_point[0] + t * three_point[1]; 
                    bezier_generated_point = points1;
                    lineRenderer.SetVertexCount(vertex++);
                    lineRenderer.SetPosition(vertex - 2, points1);
                }
            }
         
            //Reset Linerender On Touch Up
            lineRenderer.SetVertexCount(0);
            three_point.Clear();

            canDraw = false;
        }
    }

    void OnGUI()
    {
   
        //Draw Renderer Texture in full screen
        if (rt)
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), rt);


       
        //Color picker wheel
        GUI.skin = gskin;
        GUILayout.Label("\t\t\t\t\t\t\t\t\t\t\t\t");
        GUILayout.Space(10);
        if (isEraser == 0)
            currColor = Color.black; //GUIColorPicker.RGBCircle(currColor, "", colorCircle, new Vector2(colorCircleRect.xMin, colorCircleRect.yMin));
       
        currSize = GUI.HorizontalSlider(new Rect(150f, Screen.height - 75f, 199f,51f), currSize, 0f,5f, skin.horizontalSlider, skin.horizontalSliderThumb);
        GUI.DrawTexture(new Rect(10f, Screen.height - 82f, 62f, 61f), isEraser ==1? pencil:pencilactive);
        if (GUI.Button(new Rect(10f, Screen.height - 82f, 62f, 61f), "",new GUIStyle()))
        {
            isEraser = 0;
        }
        GUI.DrawTexture(new Rect(80, Screen.height - 82f, 62f, 61),isEraser ==0? rubber:rubberactive);
        if (GUI.Button(new Rect(80, Screen.height - 82f, 62f, 61f), "", new GUIStyle()))
        {
            isEraser = 1;
            
        }
     
       
    }

    Mesh CreatePlaneMesh(float factor)
    {
        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[]
        {
            new Vector3( 1*factor, 0, 1*factor),
            new Vector3( 1*factor, 0, -1*factor),
            new Vector3(-1*factor, 0, 1*factor),
            new Vector3(-1*factor, 0, -1*factor),
        };

        Vector2[] uv = new Vector2[]
        {
            new Vector2(1, 1),
            new Vector2(1, 0),
            new Vector2(0, 1),
            new Vector2(0, 0),
        };

        int[] triangles = new int[]
        {
            0, 1, 2,
            2, 1, 3,
        };

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;

        return mesh;
    }
}