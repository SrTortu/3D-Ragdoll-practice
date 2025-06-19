using System;
using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }

    [SerializeField] private Transform _player;
    [SerializeField] private GameObject _playerMesh;
    [SerializeField] private StarterAssetsInputs _inputs;
    private Animator _playerAnimator;
    private Rigidbody _hipsRB;
    public bool playerMove = true;

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
        playerMove = false;
        StartCoroutine(CordinatePlayerPosition());
    }

    IEnumerator CordinatePlayerPosition()
    {
        CharacterController charController = _player.GetComponent<CharacterController>();
        charController.enabled = false;
        _inputs.movement = false;

        // Esperar a que la velocidad se reduzca
        while (_hipsRB.velocity.magnitude > 0.3f)
        {
            yield return null;
        }

        _player.position = _hipsRB.position;

        if (charController != null)
        {
            yield return null;
            charController.enabled = true;
        }


        // Resetear el ragdoll para que vuelva a la posición normal
        _hipsRB.velocity = Vector3.zero;
        _hipsRB.angularVelocity = Vector3.zero;
        _inputs.movement = true;
        _playerAnimator.enabled = true;
    }
}