using Desktop.Commons;
using Desktop.Models;
using UnityEngine;
using UnityEngine.UI;

namespace Apps.Commons
{
    public class TopBarColorManager : MonoBehaviour
    {
        private Image _bg;

        private void Start()
        {
            _bg = GetComponent<Image>();
            UpdateColor();
            
            DesktopMvc.Instance.DesktopGeneratorController.onColorSchemeChange += UpdateColor;
        }

        private void OnDestroy()
        {
            DesktopMvc.Instance.DesktopGeneratorController.onColorSchemeChange -= UpdateColor;
        }

        private void UpdateColor()
        {
            ColorUtility.TryParseHtmlString(DesktopModel.Instance.GetColorScheme(), out Color c);
            _bg.color = c;
        }
    }
}
