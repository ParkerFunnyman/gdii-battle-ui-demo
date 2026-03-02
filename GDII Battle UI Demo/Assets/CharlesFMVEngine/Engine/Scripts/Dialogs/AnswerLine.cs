using TMPro;
using UnityEngine;
using static CharlesEngine.CELogger;

namespace CharlesEngine
{
	// You can create your own implementation of IAnswerLine to achieve a unique dialog choice effect
	// The below is the default implementation.
	public class AnswerLine : MonoBehaviour, IAnswerLine
	{
		private AnswerLineEvent _onClickEvent = new AnswerLineEvent();
	
		public AnswerLineEvent OnClick => _onClickEvent;
		
		public Node Node { get; set; }

		private TextMeshPro _text;
		private void Awake()
		{
			_text = GetComponentInChildren<TextMeshPro>();
			var evtListener = GetComponent<EventListener>();
			if (evtListener == null)
			{
				evtListener = gameObject.AddComponent<EventListener>();
			}
			evtListener.OnMouseEnterEvent.AddListener(OnMouseEnterHandler);
			evtListener.OnMouseExitEvent.AddListener(OnMouseExitHandler);
			evtListener.OnMouseClick.AddListener(OnMouseClickHandler);
		}
		
		private void OnMouseEnterHandler()
		{
#if !UNITY_ANDROID && !UNITY_IOS
			transform.TweenScale(this, 1.05f, 0.1f);
#endif
		}

		private void OnMouseExitHandler()
		{
#if !UNITY_ANDROID && !UNITY_IOS
			transform.TweenScale(this, 1f, 0.1f);
#endif
		}
		
		private void OnMouseDown()
		{
#if UNITY_ANDROID || UNITY_IOS
			transform.TweenScale(this, 1.05f, 0.1f);
#endif
		}
		
		private void OnMouseUp()
		{
#if UNITY_ANDROID || UNITY_IOS
			transform.TweenScale(this, 1f, 0.1f);
#endif
		}

		private void OnMouseClickHandler()
		{
			Log("answer clicked "+name, DIALOG);

			OnClick?.Invoke(this);
		}

		public void SetActive(bool value)
		{
			gameObject.SetActive(value);
		}

		public void SetText(string text)
		{
			_text.text = text;
		}


		public bool IsActive()
		{
			return gameObject.activeSelf;
		}
	}
}