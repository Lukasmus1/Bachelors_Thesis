using System;
using Apps.Commons;
using Apps.Settings.Models;
using Desktop.Commons;
using Desktop.Models;
using Sounds.Commons;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using AudioType = Sounds.Models.AudioType;

namespace Apps.Settings.Views
{
    public class SettingsAppView : AppsCommon
    {
        [Header("Wallpaper")]
        [SerializeField] private TMP_Text currentWallpaperText;
        [SerializeField] private GameObject chooseWallpaperPopup;
        
        [Header("Color Scheme")]
        [SerializeField] private Image currentColorSchemeImage;
        [SerializeField] private TMP_Text currentColorSchemeText;
        [SerializeField] private GameObject chooseColorSchemePopup;

        [Header("Audio")]
        [SerializeField] private Slider effectsSlider;
        [SerializeField] private TMP_InputField effectsInputField;
        [SerializeField] private Slider musicSlider;
        [SerializeField] private TMP_InputField musicInputField;
        private float maxVolumeLin;
        private float minVolumeLin;
        
        [Header("Max FPS")]
        [SerializeField] private Slider fpsSlider;
        [SerializeField] private TMP_InputField fpsInputField;
        [SerializeField] private Toggle customFpsToggle;
        

        private void Start()
        {
            chooseWallpaperPopup.GetComponent<ChangeWallpaperPopupView>().onChangeWallpaper += UpdateWallpaperText;
        }

        private void OnEnable()
        {
            DesktopMvc.Instance.DesktopGeneratorController.SetDesktopFlag(gameObject.tag, true);
            
            UpdateWallpaperText();
            UpdateColorSchemeTextAndImage();
            
            maxVolumeLin = effectsSlider.maxValue;
            minVolumeLin = effectsSlider.minValue;
            UpdateSound();
            
            bool isMaxFPSUnlimited = MaxFPS.MaxFPSValue == "-1";
            fpsSlider.maxValue = (float)Screen.currentResolution.refreshRateRatio.value;
            fpsSlider.SetValueWithoutNotify(isMaxFPSUnlimited ? fpsSlider.maxValue : int.Parse(MaxFPS.MaxFPSValue));
            fpsInputField.SetTextWithoutNotify(((int)fpsSlider.value).ToString());

            customFpsToggle.isOn = !isMaxFPSUnlimited;
            customFpsToggle.onValueChanged.Invoke(!isMaxFPSUnlimited);
        }

        protected override void OnDisableChild()
        {
            DesktopMvc.Instance.DesktopGeneratorController.SetDesktopFlag(gameObject.tag, false);
        }
        
        private void OnDestroy()
        {
            chooseWallpaperPopup.GetComponent<ChangeWallpaperPopupView>().onChangeWallpaper -= UpdateWallpaperText;    
        }

        #region Wallpaper
        /// <summary>
        /// Opens the window for selecting a wallpaper.
        /// </summary>
        public void OpenChooseWallpaperPopup()
        {
            chooseWallpaperPopup.SetActive(true);
        }

        /// <summary>
        /// Updates the current wallpaper text to reflect the current wallpaper set in the desktop model.
        /// </summary>
        private void UpdateWallpaperText()
        {
            currentWallpaperText.text = DesktopMvc.Instance.DesktopGeneratorController.GetCurrentWallpaperName();
        }
        #endregion

        #region ColorScheme
        /// <summary>
        /// Opens the windows for selecting a color scheme
        /// </summary>
        public void OpenChooseColorSchemePopup()
        {
            chooseColorSchemePopup.SetActive(true);
        }

        /// <summary>
        /// Changes the current color scheme 
        /// </summary>
        public void ChangeColorScheme()
        {
            var fcp = chooseColorSchemePopup.GetComponent<FlexibleColorPicker>();
            
            DesktopMvc.Instance.DesktopGeneratorController.ChangeColorScheme(fcp.color);
            
            UpdateColorSchemeTextAndImage();
            
            CancelChooseColorSchemePopup();
        }

        /// <summary>
        /// Hides the change color scheme popup
        /// </summary>
        public void CancelChooseColorSchemePopup()
        {
            chooseColorSchemePopup.SetActive(false);
        }
        
        /// <summary>
        /// Updates the color scheme text and preview image
        /// </summary>
        private void UpdateColorSchemeTextAndImage()
        {
            currentColorSchemeText.text = DesktopModel.Instance.GetColorScheme();
            
            ColorUtility.TryParseHtmlString(DesktopModel.Instance.GetColorScheme(), out Color c);
            currentColorSchemeImage.color = c;
        }
        #endregion

        #region Audio
        
        /// <summary>
        /// Changes the sound effects volume using an input field
        /// </summary>
        public void ChangeEffectsVolumeFromText()
        {
            float value = ConvertFromLinearToLog(int.Parse(effectsInputField.text) * maxVolumeLin / 100f);
            SoundMvc.Instance.SoundController.UpdateSoundVolume(value, AudioType.Effects);
            
            UpdateEffectsSlider();
        }
        
        /// <summary>
        /// Changes the music volume using an input field
        /// </summary>
        public void ChangeMusicVolumeFromText()
        {
            float value = ConvertFromLinearToLog(int.Parse(musicInputField.text) * maxVolumeLin / 100f);
            SoundMvc.Instance.SoundController.UpdateSoundVolume(value, AudioType.Music);
            
            UpdateMusicSlider();
        }
        
        /// <summary>
        /// Changes the music volume using a slider
        /// </summary>
        public void ChangeEffectsVolumeFromSlider()
        {
            float value = ConvertFromLinearToLog(effectsSlider.value);
            SoundMvc.Instance.SoundController.UpdateSoundVolume(value, AudioType.Effects);
            
            UpdateEffectsText();
        }
        
        /// <summary>
        /// Changes the music volume using a slider
        /// </summary>
        public void ChangeMusicVolumeFromSlider()
        {
            float value = ConvertFromLinearToLog(musicSlider.value);
            SoundMvc.Instance.SoundController.UpdateSoundVolume(value, AudioType.Music);
            
            UpdateMusicText();
        }
        
        /// <summary>
        /// Updates the entire sound settings UI
        /// </summary>
        private void UpdateSound()
        {
            UpdateEffectsSlider();
            UpdateEffectsText();
            
            UpdateMusicSlider();
            UpdateMusicText();
        }
        
        /// <summary>
        /// Gets the current linear set sound effects volume 
        /// </summary>
        /// <returns>Linear sound effects volume</returns>
        private float GetEffectsValue()
        {
            return ConvertFromLogToLinear(SoundMvc.Instance.SoundController.GetEffectsVolumeValue());
        }
        
        /// <summary>
        /// Updates the effects volume slider
        /// </summary>
        private void UpdateEffectsSlider()
        {
            effectsSlider.SetValueWithoutNotify(GetEffectsValue());
        }
        
        /// <summary>
        /// Updates the effects volume text
        /// </summary>
        private void UpdateEffectsText()
        {
            effectsInputField.SetTextWithoutNotify(ConvertLinToPercentage(GetEffectsValue()));
        }

        /// <summary>
        /// Gets current linear music volume value
        /// </summary>
        /// <returns>Linear music volume</returns>
        private float GetMusicValue()
        {
            return ConvertFromLogToLinear(SoundMvc.Instance.SoundController.GetMusicVolumeValue());
        }
        
        /// <summary>
        /// Updates the music slider value
        /// </summary>
        private void UpdateMusicSlider()
        {
            musicSlider.SetValueWithoutNotify(GetMusicValue());
        }
        
        /// <summary>
        /// Updates the music volume text
        /// </summary>
        private void UpdateMusicText()
        {
            musicInputField.SetTextWithoutNotify(ConvertLinToPercentage(GetMusicValue()));
        }

        /// <summary>
        /// Converts from log10 value to linear
        /// </summary>
        /// <param name="logValue">Log 10 value</param>
        /// <returns>Linear value</returns>
        private float ConvertFromLogToLinear(float logValue)
        {
            float value = Mathf.Pow(10, logValue / 20);
            value = Mathf.Clamp(value, minVolumeLin, maxVolumeLin);
            
            return value;
        }
        
        /// <summary>
        /// Converts from linear value to log10 value
        /// </summary>
        /// <param name="linearValue">Linear float value</param>
        /// <returns>Log10 value</returns>
        private float ConvertFromLinearToLog(float linearValue) => Mathf.Log10(Mathf.Clamp(linearValue, minVolumeLin, maxVolumeLin)) * 20;

        /// <summary>
        /// Converts the linear value to percentage
        /// </summary>
        /// <param name="linValue">Linear value</param>
        /// <returns>Percentage value relative to the max linear value</returns>
        private string ConvertLinToPercentage(float linValue)
        {
            float percentageValue = linValue / maxVolumeLin * 100;
            return ((int)percentageValue).ToString();
        }
        
        #endregion

        #region FPS

        public void ToggleCustomFPS(bool value)
        {
            fpsInputField.interactable = value;
            fpsSlider.interactable = value;
            
            ChangeFramerate(value ? (int)fpsSlider.value : -1);
        }
        
        public void ChaneMaxFPS(float value)
        {
            fpsInputField.SetTextWithoutNotify(((int)value).ToString());
            
            ChangeFramerate((int)value);
        }

        public void ChangeMaxFPSInputField(string value)
        {
            if (!int.TryParse(value, out int val)) 
                return;
            
            val = Mathf.Clamp(val, 1, (int)fpsSlider.maxValue);
            fpsInputField.text = val.ToString();
            
            fpsSlider.SetValueWithoutNotify(val);
            
            ChangeFramerate(val);
        }
        
        private void ChangeFramerate(int val)
        {
            Application.targetFrameRate = val;
            MaxFPS.MaxFPSValue = val.ToString();
        }
        
        #endregion
    }
}
