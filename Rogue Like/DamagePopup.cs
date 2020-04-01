//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//public class DamagePopup : MonoBehaviour
//{
//    public string text = "";
//    public TextMesh textMesh;
//    public bool busy = false;

//    IEnumerator hide()
//    {
//        while (true)
//        {
//            yield return new WaitForSecondsRealtime(2f);
//            textMesh.text = "";
//            busy = false;
//        }
//    }
//    // Start is called before the first frame update
//    void Start()
//    {
//        busy = true;
//        textMesh = GetComponent<TextMesh>();
//        StartCoroutine(hide());
//        //StartCoroutine(Main.Despawn(2f, gameObject));
//    }

//    // Update is called once per frame
//    void Update()
//    {

//    }
//}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamagePopup : MonoBehaviour
{
    public TextMesh textMesh;
    public int secondsLeft = 0;

    IEnumerator HideTimer()
    {
        while (true)
        {
            if(secondsLeft > 0)
            {
                yield return new WaitForSecondsRealtime(1f);
                secondsLeft -= 1;
            }
            if (secondsLeft == 0)
            {
                textMesh.text = "";
            }

            yield return new WaitForEndOfFrame();
        }
    }

    void Start()
    {
        textMesh = GetComponent<TextMesh>();
        StartCoroutine(HideTimer());
    }

    void Update()
    {

    }
}

