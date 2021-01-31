using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[AddComponentMenu("7AGames/Button")]
public class CustomButton : Button
{
    [SerializeField]
    public bool scaleOnPressed = false;

    // Internal
    private Vector3 tempScale;
    private bool isDown = false;

	// Event delegate triggered on mouse or touch down.
	[SerializeField]
    ButtonDownEvent _onDown = new ButtonDownEvent();

    [SerializeField]
    ButtonDownEvent _onUp = new ButtonDownEvent();

	[SerializeField] public bool isRepeatable = false;
	[SerializeField] public float startRepeatTime = 0.5f;
	[SerializeField] public float repeatTime = 0.1f;

	private float startRepeatTimeInternal;
	private float repeatTimeInternal;

	protected CustomButton() { }

	protected override void Awake()
	{
		startRepeatTimeInternal = startRepeatTime;
		repeatTimeInternal = repeatTime;
		
		tempScale = transform.localScale;
	}

	private void Update()
	{
		if(isDown && isRepeatable)
		{
			startRepeatTimeInternal -= Time.deltaTime;

			if(startRepeatTimeInternal <= 0)
			{
				repeatTimeInternal -= Time.deltaTime;
				if(repeatTimeInternal <= 0)
				{
					repeatTimeInternal = repeatTime;
					//do not invoke if disabled
					if(interactable)	onClick.Invoke();
				}
			}
		}
		else
		{
			startRepeatTimeInternal = startRepeatTime;
		}
	}

	public void CutRepeatClick()
	{
		isDown = false;
		if (scaleOnPressed) ScaleUp();

	}

	public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);

        if (eventData.button != PointerEventData.InputButton.Left)
            return;

        _onDown.Invoke();

		isDown = true;

		if (scaleOnPressed) ScaleDown();
    }


    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);

        if (eventData.button != PointerEventData.InputButton.Left)
            return;

        _onUp.Invoke();

		isDown = false;

		if (scaleOnPressed) ScaleUp();
    }


    public void Disable()
    { //has to be first cut then disable interactable or ScaleUp breaks
	    CutRepeatClick();
	   interactable = false;
    }
    public void Enable()
    {
	    interactable = true;
    }

    private void ScaleDown()
    {
		if (!interactable) return;
		transform.localScale = tempScale * 0.85f;
    }

    private void ScaleUp()
    {
		if (!interactable) return;

		transform.localScale = tempScale;
    }

    public ButtonDownEvent onDown
    {
        get { return _onDown; }
        set { _onDown = value; }
    }

    public ButtonDownEvent onUp
    {
        get { return _onUp; }
        set { _onUp = value; }
    }

    [Serializable]
    public class ButtonDownEvent : UnityEvent { }



}
