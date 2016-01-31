using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class UILineDraw : MonoBehaviour {

	public Image imageView;

	Texture2D t;

	bool inArea;

	public bool canDraw;

	public Queue<Vector2> Points;

	public GameSystem gs;

	public Color deadColor;

	public void EnterArea()
	{
		Debug.Log("Entered Draw Area");
		inArea = true;
	}

	public void ExitArea()
	{
		Debug.Log("Exited Draw Area");
		inArea = false;
	}

	void Start()
	{
		Points = new Queue<Vector2>();
		t = new Texture2D((int)(imageView.rectTransform.rect.width/3),
		 				  (int)(imageView.rectTransform.rect.height/3));
		reset();
	}

	Vector2 LastPoint;
	bool q;
	void LateUpdate () 
	{
		if(Input.GetMouseButton(0) && inArea && canDraw)
		{
			Debug.Log("Drawing!");
			q = false;
			Vector2 p = PosToImg(Input.mousePosition);
			p /= 3;
			float d = Vector2.Distance(p, LastPoint);
			if(LastPoint.x > 0)
			{
				for(int i=0; i<(int)d; i++)
				{
					Circle(t, (int)Vector2.Lerp(p,LastPoint, i/d).x, (int)Vector2.Lerp(p,LastPoint, i/d).y, 2, Color.black);
				}
			}
			Points.Enqueue(p);
			Circle(t, (int)p.x, (int)p.y, 2, Color.black);
			t.Apply();
			imageView.sprite = Sprite.Create(t, new Rect(Vector2.zero,new Vector2(t.width,t.height)), Vector2.one/2);
			LastPoint = p;
		}
		else
		{
			if(!q)
			{
				Points.Enqueue(Vector2.one*-1);
				q = true;
			}
			LastPoint = new Vector2(-1,-1);
		}
			
	}


	Vector2 lastDraw;
	public void Draw(Vector2 p, int id)
	{
		Color c = Color.black;
		if(gs.lost.Contains(id))
			c = deadColor;
		if(p.x < 0)
		{
			lastDraw = p;
		}
		else
		{
			float d = Vector2.Distance(p, lastDraw);
			if(lastDraw.x > 0)
			{
				for(int i=0; i<(int)d; i++)
				{
					Circle(t, (int)Vector2.Lerp(p,lastDraw, i/d).x, (int)Vector2.Lerp(p,lastDraw, i/d).y, 2, c);
				}
			}
			Circle(t, (int)p.x, (int)p.y, 2, c);
			t.Apply();
			imageView.sprite = Sprite.Create(t, new Rect(Vector2.zero,new Vector2(t.width,t.height)), Vector2.one/2);
			lastDraw = p;
		}

	}

	Vector2 PosToImg(Vector2 pos)
	{
		
		float yScaleF = -(FindObjectOfType<CanvasScaler>().GetComponent<RectTransform>().rect.y)/Screen.height;
		Vector2 v = new Vector2(pos.x*(800f/Screen.width), 2*((pos.y-(Screen.height*0.1f))*yScaleF));
		//Debug.Log("Screen Pos: " + pos + "\nTexture Pos: " + v);
		return v;
	}

	public void reset()
	{
		for(int x =0; x<t.width; x++)
		{
			for(int y=0; y<t.height; y++)
			{
				t.SetPixel(x,y, Color.white);
			}
		}
		t.Apply();
	}

	public void Circle(Texture2D tex, int cx, int cy, int r, Color col)
     {
         int x, y, px, nx, py, ny, d;
         
         for (x = 0; x <= r; x++)
         {
             d = (int)Mathf.Ceil(Mathf.Sqrt(r * r - x * x));
             for (y = 0; y <= d; y++)
             {
                 px = cx + x;
                 nx = cx - x;
                 py = cy + y;
                 ny = cy - y;
 
                 tex.SetPixel(px, py, col);
                 tex.SetPixel(nx, py, col);
  
                 tex.SetPixel(px, ny, col);
                 tex.SetPixel(nx, ny, col);
 
             }
         }    
     }
}
