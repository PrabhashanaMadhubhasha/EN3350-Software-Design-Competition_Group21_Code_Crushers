using UnityEngine;
using UnityEngine.UI;
using TMPro; // Uncomment if using TextMeshPro

public class TaxRateController : MonoBehaviour
{
    public Slider taxRateSlider;
    public Text taxRateText; // Use this if you are using the default Text component
    // public TextMeshProUGUI taxRateText; // Use this if you are using TextMeshPro

    public Slider reduceConsumptionRateSlider;
    public Text reduceConsumptionText;

    private float taxRateFactor;
    private float reduceConsumptionFactor;

    void Start()
    {
        // Initialize the slider and text
        taxRateSlider.onValueChanged.AddListener(OnSliderTaxValueChanged);
        taxRateFactor = taxRateSlider.value;
        UpdateTaxRateText();

        reduceConsumptionRateSlider.onValueChanged.AddListener(OnSliderReduceConsumptionValueChanged);
        reduceConsumptionFactor = reduceConsumptionRateSlider.value;
        UpdateReduceConsumptionRateText();
    }

    void OnSliderTaxValueChanged(float value)
    {
        // Update the tax rate based on the slider value
        taxRateFactor = value;
        UpdateTaxRateText();
    }

    void OnSliderReduceConsumptionValueChanged(float value)
    {
        // Update the tax rate based on the slider value
        reduceConsumptionFactor = value;
        UpdateReduceConsumptionRateText();
    }

    void UpdateTaxRateText()
    {
        // Display the current tax rate
        taxRateText.text = (taxRateFactor * 100).ToString() + " %";
        AssetsManager.Instance.taxRateFactor = taxRateFactor; 
    }

    void UpdateReduceConsumptionRateText()
    {
        // Display the current tax rate
        reduceConsumptionText.text = (reduceConsumptionFactor * 100).ToString() + " %";
        AssetsManager.Instance.reduceConsumptionFactor = reduceConsumptionFactor;
    }
}
