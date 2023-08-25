using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StiManager : MonoBehaviour
{
    [SerializeField]
    GameObject Prefab;
    [SerializeField]
    GameObject StiParent;
    [SerializeField]
    RenEvent ren;

    public bool Sticheck = true;

    public List<GameObject> Stilist = new List<GameObject>(); 

    public void OnClickSti(GameObject Sti)
    {
        ren.EarserImg.SetActive(false);
        ren.PencilImg.SetActive(false);
        ren.EraserSprite.SetActive(false);
        Sticheck = true;
        ren.PenTle.SetActive(false);
        ren.state = RenEvent.State.None;
        GameObject obj = Instantiate(Prefab);
        obj.SetActive(true);
        obj.transform.parent = StiParent.transform;
        obj.transform.localPosition = new Vector3(-166, 58, 16);
        ren.z -= 0.01f;
        obj.transform.position = new Vector3(obj.transform.position.x, obj.transform.position.x, ren.z);
        obj.transform.localScale = new Vector3(184, 126, 1);
        obj.GetComponent<MeshRenderer>().material = Sti.GetComponent<Stiinfo>().mat;
        ren.Wholelist.Add("Sti");
        Stilist.Add(obj);  
        
    }

    public void OffStiGroup()
    {
        for(int i=0; i<Stilist.Count; i++)
        {
            Stilist[i].GetComponent<ImageMove>().Group.SetActive(false);
        }
    }
}
