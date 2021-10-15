using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalScreenController : MonoBehaviour
{
    public Transform imageTransform;

    public void InitializeComponents(Transform imageTransform)
    {
        this.imageTransform = imageTransform;
    }

    public void ResetScale()
    {
        imageTransform.localScale = Vector3.zero;
    }


    #region Image popup
    public void ImagePopup()
    {
        StartCoroutine(ScaleCoroutine());
    }


    IEnumerator ScaleCoroutine()
    {

        float inc = 0.1f;
        float delay = 0.025f;


        for (float perc = 0; perc <= 1.0; perc += inc)
        {
            imageTransform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, perc);
            yield return new WaitForSeconds(delay);
        }


    }
    #endregion

}
