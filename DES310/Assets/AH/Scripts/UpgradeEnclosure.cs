using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeEnclosure : MonoBehaviour
{
    [SerializeField]
    List<Sprite> enclosureUpgradesPanda = new List<Sprite>();
    
    [SerializeField]
    List<Texture> enclosureUpgradesPandaTextures = new List<Texture>();

    [SerializeField]
    List<Sprite> enclosureUpgradesPenguin = new List<Sprite>();

    [SerializeField]
    List<Texture> enclosureUpgradesPenguinTextures = new List<Texture>();

    //[SerializeField]
    //List<Texture> enclosureUpgradesLion = new List<Texture>();

    //[SerializeField]
    //List<Texture> enclosureUpgradesGiraffe = new List<Texture>();

    public int enclosureStage = 0;

    private SpriteRenderer sprite = null;
    private RawImage image = null;

    public bool panda = false;
    public bool penguin = false;
    //public bool lion = false;
    //public bool giraffe = false;

    void Awake()
    {
        sprite = this.transform.GetComponent<SpriteRenderer>();
        image = this.transform.GetComponent<RawImage>();
    }

    void Update()
    {
        if (panda)
        {
            penguin = false;
            //lion = false;
            //giraffe = false;
            sprite.sprite = enclosureUpgradesPanda[enclosureStage];
            image.texture = enclosureUpgradesPandaTextures[enclosureStage];
        }
        else if(penguin)
        {
            panda = false;
            //lion = false;
            //giraffe = false;
            sprite.sprite = enclosureUpgradesPenguin[enclosureStage];
            image.texture = enclosureUpgradesPenguinTextures[enclosureStage];
        }
        //else if (lion)
        //{
        //    panda = false;
        //    penguin = false;
        //    giraffe = false;
        //    image.texture = enclosureUpgradesLion[enclosureStage];
        //}
        //else if (giraffe)
        //{
        //    panda = false;
        //    lion = false;
        //    penguin = false;
        //    image.texture = enclosureUpgradesGiraffe[enclosureStage];
        //}
    }
}
