using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Drivers.CharacterStats;

public class StatDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private CharacterStat _stat { get; set; }
    public CharacterStat Stat
    {
        get { return _stat; }
        set
        {
            _stat = value;
            UpdateStatValue();
        }
    }

    private string _name { get; set; }
    public string Name
    {
        get { return _name; }
        set
        {
            _name = value;
            nameText.text = _name.ToLower();
        }
    }

    [SerializeField] Text nameText;
    [SerializeField] Text valueText;
    [SerializeField] StatTooltip tooltip;

    protected bool IsPointerOver;

    private void OnValidate()
    {
        Text[] texts = GetComponentsInChildren<Text>();
        nameText = texts[0];
        valueText = texts[1];

        if (tooltip == null)
        {
            tooltip = FindObjectOfType<StatTooltip>();
        }
    }

    protected virtual void OnDisable()
    {
        if (IsPointerOver)
        {
            OnPointerExit(null);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        IsPointerOver = true;
        Debug.Log(Name);
        tooltip.ShowTooltip(Stat, Name);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        IsPointerOver = false;
        tooltip.HideTooltip();
    }

    public void UpdateStatValue()
    {
        valueText.text = _stat.Value.ToString();
    }
}