using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public static MainCamera Instance;
    void Awake()
    {
        if (Instance != null) Destroy(Instance.gameObject);
        Instance = this;
    }
    public FollowTarget followTarget;
}
