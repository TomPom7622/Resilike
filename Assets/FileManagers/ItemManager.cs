using UnityEngine;

public class ItemManager : MonoBehaviour
{
    private TextAsset itemsFile;
    private Classes.ItemDatabase allItems;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        Debug.Log("AAA");
        itemsFile = Resources.Load<TextAsset>("items");
        allItems = JsonUtility.FromJson<Classes.ItemDatabase>(itemsFile.text);
        foreach (Classes.Item item in allItems.items)
        {
           Debug.Log(item.Id);
        }
    }
    public Classes.Item getItem(int id)
    {
        return allItems.items[id];
    }
}
