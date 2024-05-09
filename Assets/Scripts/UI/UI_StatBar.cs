using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CX
{
    public class UI_StatBar : MonoBehaviour
    {
        private Slider slider;

        protected virtual void Awake()
        {
            slider = GetComponent<Slider>();
        }
        public virtual void SetStat(float newValue)
        {
            slider.value = newValue;
        }
        public virtual void SetMaxStat(int maxVlaue)
        {
            slider.maxValue = maxVlaue;
            slider.value = maxVlaue;
        }
    }

}
