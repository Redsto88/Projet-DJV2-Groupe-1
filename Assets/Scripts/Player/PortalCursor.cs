using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cursor = UnityEngine.Cursor;

public class PortalCursor : MonoBehaviour
{
    [SerializeField] private new Camera camera;
    public bool canSpawnPortal;
    public bool usingMouseInput;

    [Header("For Gamepad Controls Only")] 
    public float cursorSpeed = 10;
    
    private Vector2 _cursorPos;

    private Renderer _renderer;
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;

        _renderer = GetComponent<Renderer>();
        _renderer.material.color = Color.cyan;
        _cursorPos = new Vector2(Screen.width / 2, Screen.height / 2);
    }

    // Update is called once per frame
    void Update()
    {
        if(usingMouseInput)
        {
            _cursorPos = Input.mousePosition;
        }
        else
        {
            _cursorPos += new Vector2(Input.GetAxis("GamepadCursorHorizontal"), Input.GetAxis("GamepadCursorVertical")) * (cursorSpeed * Time.deltaTime);
            _cursorPos.x = Mathf.Clamp(_cursorPos.x, 0, Screen.width - 1);
            _cursorPos.y = Mathf.Clamp(_cursorPos.y, 0, Screen.height - 1);
        }
        
        var ray = camera.ScreenPointToRay(_cursorPos);

        var layerInt = LayerMask.GetMask("Ground");
        
        
        //TODO optimiser ça
        if (Physics.Raycast(ray, out var x, Mathf.Infinity, layerInt, QueryTriggerInteraction.UseGlobal))
        {
            // if (x.collider.gameObject.layer == 3)
            // {
                canSpawnPortal = true;
                transform.position = x.point;
                if (_renderer.material.color != Color.green)
                {
                    _renderer.material.color = Color.green;
                }
            // }
            // else
            // {
            //     canSpawnPortal = false;
            //     if(_renderer.enabled)
            //     {
            //         _renderer.enabled = false;
            //     }
            // }
        }
        else if (Physics.Raycast(ray, out var y, Mathf.Infinity, ~layerInt, QueryTriggerInteraction.UseGlobal))
        {
            canSpawnPortal = false;
            transform.position = y.point;
            if(_renderer.material.color != Color.red)
            {
                _renderer.material.color = Color.red;
            }
        }
    }
}
