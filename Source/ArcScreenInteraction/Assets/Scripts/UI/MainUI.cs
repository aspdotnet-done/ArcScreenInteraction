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

        private Button xiaofangBtn;
        public Button XiaofangBtn
        {
            get
            {
                if (xiaofangBtn == null) xiaofangBtn = transform.Find("Selection/xiaofang/Button").GetComponent<Button>();
                return xiaofangBtn;
            }
        }

        private Button anfangBtn;
        public Button AnfangBtn
        {
            get
            {
                if (anfangBtn == null) anfangBtn = transform.Find("Selection/anfang/Button").GetComponent<Button>();
                return anfangBtn;
            }
        }
        private Button renfangBtn;
        public Button RenfangBtn
        {
            get
            {
                if (renfangBtn == null) renfangBtn = transform.Find("Selection/renfang/Button").GetComponent<Button>();
                return renfangBtn;
            }
        }


        private void OnEnable()
        {
            AnfangBtn.onClick.AddListener(AnfangClick);
            XiaofangBtn.onClick.AddListener(XiaofangClick);
            RenfangBtn.onClick.AddListener(RenfangClick);
        }

        void AnfangClick()
        {
            MediaListUI ui = UIManager.Instance.GetUI(UIType.MediaListUI) as MediaListUI;
            ui.InitMediaList(SecurityType.anfang);
            ui.ShowUI();

        }
        void XiaofangClick()
        {
            MediaListUI ui = UIManager.Instance.GetUI(UIType.MediaListUI) as MediaListUI;
            ui.InitMediaList(SecurityType.xiaofang);
            ui.ShowUI();
        }
        void RenfangClick()
        {
            MediaListUI ui = UIManager.Instance.GetUI(UIType.MediaListUI) as MediaListUI;
            ui.InitMediaList(SecurityType.renfang);
            ui.ShowUI();
        }



        private void OnDisable()
        {
            AnfangBtn.onClick.RemoveListener(AnfangClick);
            XiaofangBtn.onClick.RemoveListener(XiaofangClick);
            RenfangBtn.onClick.RemoveListener(RenfangClick);
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
