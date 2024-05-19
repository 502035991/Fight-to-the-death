using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace CX
{
    public class PlayerUIPopUpManager : MonoBehaviour
    {
        [Header("Die")]
        [SerializeField] GameObject youDiedPopUpGameObject;
        [SerializeField] TextMeshProUGUI youDiedPopUpBackgrounText;
        [SerializeField] TextMeshProUGUI youDiedPopUpText;
        [SerializeField] CanvasGroup youDiedPopUpCanvasGroup;

        public void SendYouDiedPopUp()
        {
            youDiedPopUpGameObject.SetActive(true);
            youDiedPopUpBackgrounText.characterSpacing = 0;
            StartCoroutine(StretchPopUpTextOverTime(youDiedPopUpBackgrounText, 8, 8.32f));
            StartCoroutine(FadeInPopUpOverTime(youDiedPopUpCanvasGroup, 5));
            StartCoroutine(WaitThenFadeOutPopUpOverTime(youDiedPopUpCanvasGroup, 2, 5));
        }

        //根据时间拉长文本
        private IEnumerator StretchPopUpTextOverTime(TextMeshProUGUI text , float duration , float stretchAmount)
        {
            if(duration>0)
            {
                text.characterSpacing = 0;
                float timer = 0;

                yield return null;

                while(timer < duration)
                {
                    timer += Time.deltaTime;
                    text.characterSpacing = Mathf.Lerp(text.characterSpacing, stretchAmount, duration * Time.deltaTime);
                }
            }
        }

        private IEnumerator FadeInPopUpOverTime(CanvasGroup canvas,float duration)
        {
            if(duration > 0)
            {
                canvas.alpha = 0;
                float timer = 0;

                yield return null;

                while(timer < duration)
                {
                    timer = timer + Time.deltaTime;
                    canvas.alpha = Mathf.Lerp(canvas.alpha ,1 ,duration * Time.deltaTime);
                }
            }

            canvas.alpha = 1;

            yield return null;

        }
        private IEnumerator WaitThenFadeOutPopUpOverTime(CanvasGroup canvas, float duration ,float delay)
        {
            if(duration> 0)
            {
                while(delay >0)
                {
                    delay -= Time.deltaTime;
                    yield return null;
                }

                canvas.alpha = 1;
                float timer = 0;

                yield return null;

                while(timer < duration)
                {
                    timer = timer+Time.deltaTime;
                    canvas.alpha =Mathf.Lerp(canvas.alpha ,0,duration * Time.deltaTime);
                }
            }
            canvas.alpha = 0;
            yield return null;
        }

    }

}
