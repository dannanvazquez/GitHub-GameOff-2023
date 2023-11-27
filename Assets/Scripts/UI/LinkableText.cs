using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkableText : MonoBehaviour
{
    //public string link;
   public void OpenURL(string link)
    {
        Application.OpenURL(link);
    }
}
