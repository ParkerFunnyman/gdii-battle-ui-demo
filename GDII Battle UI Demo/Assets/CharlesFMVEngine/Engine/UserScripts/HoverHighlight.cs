using System;
using CharlesEngine;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(EventListener))]
public class HoverHighlight : MonoBehaviour
{
    private static Material _highlightMaterial;
    
    //cached values
    private SpriteRenderer _rend;
    private MaterialPropertyBlock _propBlock;
    
    private float _state; // progress of the animation (0,1)
    private const float TweenSpeed = 0.08f;
    private const float ShineSpeed = 0.08f;
    private const float ShineSpeedOut = 0.13f;
    private const float ShineIntensity = 3f;
    
    private bool _mouseOn;
    public float _Intensity = 0.3f;

    
    private bool _inShine;
    private bool _outShine;
    private static readonly int Hovered = Shader.PropertyToID("_Hovered");

    private void Awake()
    {
        var evtListener = GetComponent<EventListener>();
        if (evtListener == null)
        {
            evtListener = gameObject.AddComponent<EventListener>();
        }
        evtListener.OnMouseEnterEvent.AddListener(OnMouseEnterHandler);
        evtListener.OnMouseExitEvent.AddListener(OnMouseExitHandler);
    }

    private void Start()
    {
        _rend = GetComponent<SpriteRenderer>();
        
        if (_highlightMaterial == null)
        {
            _highlightMaterial = new Material(Shader.Find("CharlesGames/HoverShader"));
        }

        _rend.material = _highlightMaterial;
        _propBlock = new MaterialPropertyBlock();
        _state = 0;
        _rend.GetPropertyBlock(_propBlock);
        _propBlock.SetFloat(Hovered, _state);
        _propBlock.SetFloat("_MaxHovered", _Intensity);
        _rend.SetPropertyBlock(_propBlock);
    }

    void Update()
    {
        float speed = 0f;
        
        if (_inShine && _state >= ShineIntensity)
        {
            _inShine = false;
            _outShine = true;
        }

        float max =_inShine || _outShine ? ShineIntensity : 1f;
        
        if (_outShine && _state <= 0.1f)
        {
            _outShine = false;
        }
        
        bool fadeIn = (_mouseOn || _inShine) && _state < max;
        bool fadeOut = !fadeIn && (!_mouseOn || _outShine) && _state > 0;

        if (fadeIn)
        {
            speed = _inShine ? ShineSpeed : TweenSpeed;
        }else if (fadeOut)
        {
            speed = _outShine ? -ShineSpeedOut : -TweenSpeed;
        }
        
        if (speed != 0)
        {
            _state += speed;
            _state = Mathf.Clamp(_state,0,max);
            _rend.GetPropertyBlock(_propBlock);
            _propBlock.SetFloat(Hovered, _state);
            _rend.SetPropertyBlock(_propBlock);
        }
    }

    public void Shine()
    {
        _inShine = true;
    }

    public void Disable()
    {
        Debug.Log("Hovered Disable");
        _rend.GetPropertyBlock(_propBlock);
        _propBlock.SetFloat(Hovered, 0f);
        _rend.SetPropertyBlock(_propBlock);
        Destroy(this);
    }
    
    private void OnMouseEnterHandler()
    {
        _mouseOn = true;
    }

    private void OnMouseExitHandler()
    {
        _mouseOn = false;
    }
}
