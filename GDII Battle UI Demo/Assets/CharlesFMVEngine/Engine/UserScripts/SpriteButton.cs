using UnityEngine;

namespace CharlesEngine
{
	[AddComponentMenu("CE Toolbox/SpriteButton")]
	[RequireComponent(typeof(SpriteRenderer))]
	[RequireComponent(typeof(EventListener))]
	public class SpriteButton : MonoBehaviour
	{
		public Sprite Normal;
		public Sprite Highlighted;
		private SpriteRenderer _renderer;

		private void Awake()
		{
			_renderer = GetComponent<SpriteRenderer>();
			Normal = _renderer.sprite;
			if (Highlighted == null)
			{
				Highlighted = Normal;
			}

			var evtListener = GetComponent<EventListener>();
			if (evtListener == null)
			{
				evtListener = gameObject.AddComponent<EventListener>();
			}
			evtListener.OnMouseEnterEvent.AddListener(OnMouseEnterHandler);
			evtListener.OnMouseExitEvent.AddListener(OnMouseExitHandler);
		}

		private void OnMouseEnterHandler()
		{
			_renderer.sprite = Highlighted;
		}

		private void OnMouseExitHandler()
		{
			_renderer.sprite = Normal;
		}

		private void Reset()
		{
			_renderer = GetComponent<SpriteRenderer>();
			if (_renderer != null)
			{
				Normal = _renderer.sprite;
			}
		}
	}
}