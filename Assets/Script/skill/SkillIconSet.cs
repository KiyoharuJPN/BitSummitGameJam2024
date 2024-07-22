using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillIconSet : MonoBehaviour
{
    [SerializeField] Vector3 firstposi;
    [SerializeField] float scale;
    List<GameObject> iconSeter;
    List<SpriteRenderer> iconrenderer;

    [SerializeField] GameObject thisobject;

    //float iconwdth;
    float width = 27.5f;

    private void Start()
    {
        iconSeter = new List<GameObject>();
        iconrenderer = new List<SpriteRenderer>();
    }

    public void SetHaveSkillIcon(List<ISkill> iconlist)
    {
        DestroyIcon();

        thisobject = GameObject.Find("skillmanager").transform.Find("HaveSkillIcon").gameObject;
        iconSeter = new List<GameObject>();
        iconrenderer = new List<SpriteRenderer>();

        int num = 0;
        Vector3 iconseterposi = firstposi;
        foreach (ISkill Iskill in iconlist)
        {
            Debug.Log(width + "num");
            Debug.Log(iconseterposi + "posi");

            //Debug.Log(iconwdth);

            //if (iconSeter[num] == null)
            //{
            iconSeter.Add(new GameObject("iconSeter"));
                iconSeter[num].AddComponent<SpriteRenderer>();
                iconrenderer.Add(iconSeter[num].GetComponent<SpriteRenderer>());
                if(num == 0)
                {
                    iconSeter[num].transform.position = firstposi;
                } else
                {
                iconseterposi.x = iconseterposi.x + width + 2.5f;

                iconSeter[num].transform.position = iconseterposi;
                }

                iconSeter[num].transform.localScale = Vector3.one * scale;

                iconSeter[num].transform.parent = thisobject.transform;
            //}

            iconrenderer[num].sprite = Iskill.SkillData().Icon;

            //if(num == 0) { iconwdth = Iskill.SkillData().Icon.bounds.size.x; }


            num++;
        }
    }

    public void DestroyIcon()
    {
        int num = 0;

        if(iconSeter == null) return;

        foreach(GameObject gameObject in iconSeter)
        {
            Destroy(gameObject);
            Destroy(iconrenderer[num]);
            num++;
        }
    }
}
