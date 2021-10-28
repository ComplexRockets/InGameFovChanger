using System.Xml;
using Assets.Scripts;
using ModApi.Ui;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderScript : MonoBehaviour
{
    private Slider _fovSlider;
    public IXmlLayout xmlLayout;
    private TextMeshProUGUI textValue;
    public void OnLayoutRebuilt(IXmlLayout xmlLayout)
    {
        this.xmlLayout = xmlLayout;
        _fovSlider = xmlLayout.GetElementById<Slider>("fov-slider");
        textValue = xmlLayout.GetElementById<TextMeshProUGUI>("fov-slider-text");

        _fovSlider.onValueChanged.AddListener(Mod.Instance.OnDesignerFovSliderEdit);
        _fovSlider.onValueChanged.AddListener(delegate (float value) { textValue.text = $"{value:N0}"; });

        _fovSlider.minValue = 5f;
        _fovSlider.maxValue = 160f;
        _fovSlider.value = Game.Instance.Settings.Game.General.FieldOfView;
        textValue.text = $"{Game.Instance.Settings.Game.General.FieldOfView:N0}";
    }
}
