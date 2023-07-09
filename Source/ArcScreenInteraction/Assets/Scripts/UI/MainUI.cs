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

        [Space(10)]
        [Header("设置面板")]
        [SerializeField] GameObject settingPanel = default;
        [SerializeField] Dropdown loopTypeDropdown = default;
        [SerializeField] Slider innerDelaySlider = default;
        [SerializeField] Text innerDelayText = default;
        [SerializeField] Slider outerDelaySlider = default;
        [SerializeField] Text outerDelayText = default;
        [SerializeField] Button confirmButton = default;

        private SystemData currentSystemData;
        private void OnEnable()
        {
            AnfangBtn.onClick.AddListener(AnfangClick);
            XiaofangBtn.onClick.AddListener(XiaofangClick);
            RenfangBtn.onClick.AddListener(RenfangClick);
            loopTypeDropdown.onValueChanged.AddListener(LoopTypeChange);
            innerDelaySlider.onValueChanged.AddListener(InnerDelayChange);
            outerDelaySlider.onValueChanged.AddListener(OuterDelayChange);
            StartCoroutine(InitData());
        }



        IEnumerator InitData()
        {
            yield return new WaitForSeconds(0.5f);
            //yield return new WaitUntil(() => MediaManager.Instance != null);
            //yield return new WaitUntil(() => MediaManager.Instance.setupDataScriptableAsset != null);
            currentSystemData = MediaManager.Instance.setupDataScriptableAsset.data;
            loopTypeDropdown.value = (int)currentSystemData.setupData.loopType;
            innerDelaySlider.value = currentSystemData.setupData.innerDelay;
            outerDelaySlider.value = currentSystemData.setupData.outerDelay;
        }

        private void LoopTypeChange(int index)
        {
            MediaManager.Instance.setupDataScriptableAsset.data.setupData.loopType = (LoopType)index;
            MediaManager.Instance.UpdateSetupAsset();
            //Debug.Log($"LoopTypeChange {index}");
        }
        private void InnerDelayChange(float v)
        {
            innerDelayText.text = v.ToString();
            MediaManager.Instance.setupDataScriptableAsset.data.setupData.innerDelay = v;
            MediaManager.Instance.UpdateSetupAsset();
        }

        private void OuterDelayChange(float v)
        {
            MediaManager.Instance.setupDataScriptableAsset.data.setupData.outerDelay = v;
            MediaManager.Instance.UpdateSetupAsset();
            outerDelayText.text = v.ToString();
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
            innerDelaySlider.onValueChanged.RemoveAllListeners();
            loopTypeDropdown.onValueChanged.RemoveAllListeners();
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
