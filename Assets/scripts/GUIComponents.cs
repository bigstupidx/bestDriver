using UnityEngine;

public class GUIComponents : MonoBehaviour {

  private string textAreaString = "";

  void OnGUI()
  {
   /*
    GUIStyle style = new GUIStyle();
    style.fontSize = 35;
    style.normal.textColor = Color.black;
    style.normal.background = MakeTex(420, 165, Color.white);
    */
    textAreaString = GUI.TextArea(new Rect(20, 350, 420f, 165f), textAreaString, 2500);
    

  }

  private Texture2D MakeTex(int width, int height, Color col)
  {
    Color[] pix = new Color[width * height];

    for (int i = 0; i < pix.Length; i++)
      pix[i] = col;

    Texture2D result = new Texture2D(width, height);
    result.SetPixels(pix);
    result.Apply();

    return result;
  }
}
