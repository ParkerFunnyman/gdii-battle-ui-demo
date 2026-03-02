using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
#endif

namespace CharlesEngine
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Collider2D))]
    [AddComponentMenu("CE Toolbox/Event Listener")]
    public class EventListener : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        public UnityEvent OnMouseEnterEvent = new ();
        public UnityEvent OnMouseExitEvent = new ();
        public UnityEvent OnMouseClick = new ();

        private void Awake()
        {
            EnsureEventSystemAndRightInputModule();
            EnsurePhysics2DRaycaster();
        }

        public void OnPointerEnter(PointerEventData eventData) => OnMouseEnterEvent?.Invoke();
        public void OnPointerExit(PointerEventData eventData) => OnMouseExitEvent?.Invoke();
        public void OnPointerClick(PointerEventData eventData) => OnMouseClick?.Invoke();

        private static EventSystem GetOrCreateEventSystem()
        {
            var es = EventSystem.current;
            if (es != null) return es;

            es = FindFirstObjectByType<EventSystem>();
            if (es != null) return es;

            var go = new GameObject("EventSystem");
            return go.AddComponent<EventSystem>();
        }

        public static void EnsureEventSystemAndRightInputModule()
        {
            var es = GetOrCreateEventSystem();

            // Prefer LEGACY only when the Legacy Input Manager is enabled (i.e., Active Input Handling = Both).
#if ENABLE_LEGACY_INPUT_MANAGER
        var legacy = es.GetComponent<StandaloneInputModule>();
        if (legacy == null) legacy = es.gameObject.AddComponent<StandaloneInputModule>();
        legacy.enabled = true;

        // If the new module exists too, disable it (legacy wins).
#if ENABLE_INPUT_SYSTEM
        var newer = es.GetComponent<InputSystemUIInputModule>();
        if (newer != null) newer.enabled = false;
#endif

#else
            // Legacy Input Manager is NOT enabled => we must use the new Input System UI module.
#if ENABLE_INPUT_SYSTEM
            var newer = es.GetComponent<InputSystemUIInputModule>();
            if (newer == null) newer = es.gameObject.AddComponent<InputSystemUIInputModule>();
            newer.enabled = true;

            // Disable StandaloneInputModule if present (it will crash in new-only projects).
            var legacy = es.GetComponent<StandaloneInputModule>();
            if (legacy != null) legacy.enabled = false;
#else
        Debug.LogError(
            "[EventListener] Neither ENABLE_LEGACY_INPUT_MANAGER nor ENABLE_INPUT_SYSTEM is defined. " +
            "Check Player Settings > Active Input Handling.",
            es
        );
#endif
#endif
        }

        public static void EnsurePhysics2DRaycaster()
        {
            var cam = Camera.main != null ? Camera.main : FindFirstObjectByType<Camera>();
            if (cam == null)
            {
                Debug.LogWarning("No Camera found. Pointer events require a Camera with a Physics2DRaycaster.");
                return;
            }

            if (cam.GetComponent<Physics2DRaycaster>() == null)
                cam.gameObject.AddComponent<Physics2DRaycaster>();
        }
    }

}
