using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.PlayerLoop;

public class PanelFade : MonoBehaviour, IPointerEnterHandler
{
   private float duration;
   public CanvasGroup panelFade;

   public void  FadeText()
   {
      panelFade = GetComponent<CanvasGroup>();
   }

   private void FixedUpdate()
   {
      if (panelFade.alpha > duration)
      {
         panelFade.alpha -= 0.005f;
      }
   }

   public void OnPointerEnter(PointerEventData eventData)
   {
      panelFade.alpha = 1.0f;
   }
   
}
