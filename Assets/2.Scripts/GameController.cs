using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }
    
    [SerializeField] private Transform _player;
    [SerializeField] private GameObject _playerMesh;
    private Animator _playerAnimator;
    private Rigidbody _hipsRB;
    Vector3 positionToMove = Vector3.zero;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        _playerAnimator = _player.GetComponent<Animator>();
        _hipsRB = _player.GetComponentInChildren<Rigidbody>();
    }

    public void OnExplosion()
    {
        _playerAnimator.enabled = false;
        StartCoroutine(CordinatePlayerPosition());

    }

    IEnumerator  CordinatePlayerPosition()
    { 
        
        while (_hipsRB.velocity.magnitude > 0.3f)
        {
         yield return null;
        }
        Debug.Log(_player.transform.position);
        Debug.Log("Aqui movi");
        yield return null;
        _player.position = Vector3.zero;


    }
   
    
    
}
