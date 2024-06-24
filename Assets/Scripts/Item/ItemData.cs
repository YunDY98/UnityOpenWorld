using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class ItemData : MonoBehaviour
{
    TMP_Text tmp_Text;
    Image image;

    void Start()
    {
        tmp_Text = GetComponent<TMP_Text>();
        image = GetComponent<Image>();
    }
}
