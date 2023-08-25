using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Threading.Tasks;
using System.Linq;

public class Visitinfo : MonoBehaviour
{
    public List<List<string>> list = new List<List<string>>();

    public List<string> Ja = new List<string>();
    public List<string> F = new List<string>();
    public List<string> Mar = new List<string>();
    public List<string> Ap = new List<string>();
    public List<string> May = new List<string>();
    public List<string> Jun= new List<string>();
    public List<string> Jul = new List<string>();
    public List<string> Au = new List<string>();
    public List<string> S = new List<string>();
    public List<string> O = new List<string>();
    public List<string> N = new List<string>();
    public List<string> D = new List<string>();
    // Start is called before the first frame update
    void Start()
    {
        list.Add(Ja);
        list.Add(F);
        list.Add(Mar);
        list.Add(Ap);
        list.Add(May);
        list.Add(Jun);
        list.Add(Jul);
        list.Add(Au);
        list.Add(S);
        list.Add(O);
        list.Add(N);
        list.Add(D);

        Invoke("Startinfo", 1f);
    }

    public void Startinfo()
    {
        StartCoroutine(IEinfo("1"));
    }

    IEnumerator IEinfo(string month)
    {
        yield return new WaitForSeconds(0.01f);
        info(month);
    }

    void info(string month)
    {
        if(int.Parse(month)<10)
        {
            month = "0" + month;
        }
        if (!Directory.Exists("C:/Visit/" + month))
        {
            Directory.CreateDirectory("C:/Visit/" + month);
        }
        DirectoryInfo di = new DirectoryInfo("C:/Visit/" + month);
        switch (month)
        {
            case "01":
                foreach (FileInfo File in di.GetFiles().Reverse())
                {
                    Ja.Add(File.Name);
                }
                break;
            case "02":
                foreach (FileInfo File in di.GetFiles().Reverse())
                {
                    F.Add(File.Name);
                }
                break;
            case "03":
                foreach (FileInfo File in di.GetFiles().Reverse())
                {
                    Mar.Add(File.Name);
                }
                break;
            case "04":
                foreach (FileInfo File in di.GetFiles().Reverse())
                {
                    Ap.Add(File.Name);
                }
                break;
            case "05":
                foreach (FileInfo File in di.GetFiles().Reverse())
                {
                    May.Add(File.Name);
                }
                break;
            case "06":
                foreach (FileInfo File in di.GetFiles().Reverse())
                {
                    Jun.Add(File.Name);
                }
                break;
            case "07":
                foreach (FileInfo File in di.GetFiles().Reverse())
                {
                    Jul.Add(File.Name);
                }
                break;
            case "08":
                foreach (FileInfo File in di.GetFiles().Reverse())
                {
                    Au.Add(File.Name);
                }
                break;
            case "09":
                foreach (FileInfo File in di.GetFiles().Reverse())
                {
                    S.Add(File.Name);
                }
                break;
            case "10":
                foreach (FileInfo File in di.GetFiles().Reverse())
                {
                    O.Add(File.Name);
                }
                break;
            case "11":
                foreach (FileInfo File in di.GetFiles().Reverse())
                {
                    N.Add(File.Name);
                }
                break;
            case "12":
                foreach (FileInfo File in di.GetFiles().Reverse())
                {
                    D.Add(File.Name);
                }
                break;
        }
        if (int.Parse(month) < 12)
        {
            int a = int.Parse(month);
            a++;
            info(a.ToString());
        }
    }
}
