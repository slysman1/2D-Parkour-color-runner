using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UI_VolumeConroller : MonoBehaviour
{
    [SerializeField] private string audioParameter;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider slider;
    [SerializeField] private float valueMultiplier;


    private void Awake()
    {
        slider.onValueChanged.AddListener(SliderValueController);
        slider.minValue = 0.0001f;
        slider.value = PlayerPrefs.GetFloat(audioParameter, slider.value);
    }
    private void OnDisable()
    {
        PlayerPrefs.SetFloat(audioParameter, slider.value);
    }
    private void SliderValueController(float value)
    {
        audioMixer.SetFloat(audioParameter, Mathf.Log10(value) * valueMultiplier);
    }
}
