using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialMenu : MonoBehaviour
{
    [SerializeField]
    GameObject EntryPrefab;

    [SerializeField]
    float Radius = 300f;

    List<RadialMenuEntry> Entries;
    // Start is called before the first frame update
    void Start()
    {
        Entries = new List<RadialMenuEntry>();
    }

    void AddEntry(string pLabel)
    {
        GameObject entry = Instantiate(EntryPrefab, transform);

        RadialMenuEntry rme = entry.GetComponent<RadialMenuEntry>();
        rme.SetLabel(pLabel);

        Entries.Add(rme);

    }

    public void Open()
    {
        for(int i=0; i<4; i++)
        {
            AddEntry("Button" + i.ToString());
        }

        Rearrange();
    }

    void Rearrange()
    {
        float radiansOfSeperation = (Mathf.PI * 2) / Entries.Count;
        for(int i = 0; i < Entries.Count; i++ )
        {
            float x = Mathf.Sin(radiansOfSeperation * i) * Radius;
            float y = Mathf.Cos(radiansOfSeperation * i) * Radius;

            Entries[i].GetComponent<RectTransform>().anchoredPosition = new Vector3(x, y, 0);
        }
    }
}
