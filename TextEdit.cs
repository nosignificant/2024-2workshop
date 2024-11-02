using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TextEdit : MonoBehaviour
{
    public TextMeshPro text;
    ShapeTile parentShape;
    // Start is called before the first frame update
    void Start()
    {
        parentShape = transform.GetComponentInParent<ShapeTile>();
    }

    // Update is called once per frame
    void Update()
    {
        int ascii = parentShape.thisA;
        text.text = ((char)ascii).ToString();
    }
}
