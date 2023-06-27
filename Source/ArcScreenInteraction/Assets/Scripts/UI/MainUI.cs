using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

namespace EntityProgram
{
    public class MainUI : UI
    {


        private Transform bg;
        public Transform Bg
        {
            get
            {
                if (bg == null) bg = transform.parent.Find("BG");
                return bg;
            }
        }


        private void OnEnable()
        {

        }



        private void OnDisable()
        {

        }
        private void InitUI()
        {

        }
        public override void ShowUI()
        {


            base.ShowUI();


        }

        public override void HideUI()
        {
            base.HideUI();
        }
    }
}
