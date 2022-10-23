using Assets.Scripts;
using ModApi.Ui;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderScript : MonoBehaviour
{
    private Slider _fovSlider;
    public IXmlLayout xmlLayout;
    private TextMeshProUGUI valueText;
    public void OnLayoutRebuilt(IXmlLayout xmlLayout)
    {
        this.xmlLayout = xmlLayout;
        _fovSlider = xmlLayout.GetElementById<Slider>("fov-slider");
        valueText = xmlLayout.GetElementById<TextMeshProUGUI>("fov-slider-text");

        _fovSlider.onValueChanged.AddListener(Mod.Instance.OnDesignerFovSliderEdit);
        _fovSlider.onValueChanged.AddListener(delegate (float value) { valueText.text = $"{value:N0}"; });
        xmlLayout.GetElementById("fov-slider-text").AddOnClickEvent(delegate
            {
                Debug.Log("Value Clicked");
                ModApi.Ui.InputDialogScript dialog = Game.Instance.UserInterface.CreateInputDialog();
                dialog.MessageText = "Enter Value";
                dialog.InputText = valueText.text;
                dialog.OkayClicked += delegate (ModApi.Ui.InputDialogScript d)
                {
                    d.Close();
                    Debug.Log(d.InputText);
                    if (float.TryParse(d.InputText, out var result))
                    {
                        _fovSlider.value = result;
                    }
                };
            });

        _fovSlider.minValue = 5f;
        _fovSlider.maxValue = 160f;
        _fovSlider.value = Game.Instance.Settings.Game.General.FieldOfView;
        valueText.SetText($"{Game.Instance.Settings.Game.General.FieldOfView:N0}");
    }
}
