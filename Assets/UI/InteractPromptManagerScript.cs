using System;
using UnityEngine;

public class InteractPromptManagerScript : MonoBehaviour
{
    private TextAsset textAsset;
    private string[] prompts;
    private string path;
    private void Awake()
    {
        path = "scene1Text";
        textAsset = Resources.Load<TextAsset>(path);
        prompts = textAsset.text.Split("|");
        Debug.Log(prompts[1]);
    }
    public string GetPromptText(int index)
    {
        string tmp;
        try 
        { 
            tmp = prompts[index];
            return prompts[index];
        }
        catch(Exception e)
        {
            Debug.Log("Brak tekstu w pliku");
        }
        return "";
    }
}
