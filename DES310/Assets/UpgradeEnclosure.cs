using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeEnclosure : MonoBehaviour
{
    [SerializeField]
    List<Texture> enclosureUpgrades = new List<Texture>();

    public int enclosureStage = 0;

    private RawImage image = null;

    void Awake()
    {
        image = this.transform.GetComponent<RawImage>();
    }

    void Update()
    {
        image.texture = enclosureUpgrades[enclosureStage];
    }
}
