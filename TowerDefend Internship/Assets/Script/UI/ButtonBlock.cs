using UnityEngine;
using UnityEngine.UI;
public class ButtonBlock : MonoBehaviour {
    public int index;
    public Text Name;

    void Start()
    {
        gameObject.transform.GetChild(0).GetComponent<Text>();
    }


    public void display(int indexbut, string name)
    {
        Name.text = name + (indexbut + 1);
       
        index = indexbut;
    }

}
