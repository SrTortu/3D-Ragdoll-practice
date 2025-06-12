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
        // Si el player tiene CharacterController, deshabilitarlo temporalmente
        CharacterController charController = _player.GetComponent<CharacterController>();
        if (charController != null)
        {
            charController.enabled = false;
            _inputs.movement = false;
        }


        // Esperar a que la velocidad se reduzca
        while (_hipsRB.velocity.magnitude > 0.3f)
        {
            yield return null;
        }


        // Mover el controlador del player
        _player.position = _hipsRB.position;

        // Reactivar CharacterController si existía
        if (charController != null)
        {
            yield return null; // Esperar un frame
            charController.enabled = true;
        }

        Debug.Log("Controlador movido a: " + _player.transform.position);
        Debug.Log("Diferencia con mesh: " + Vector3.Distance(_player.position, _hipsRB.transform.position));

        // Resetear el ragdoll para que vuelva a la posición normal
        _hipsRB.velocity = Vector3.zero;
        _hipsRB.angularVelocity = Vector3.zero;

        // Reactivar el animator después de la sincronización
        yield return new WaitForSeconds(0.2f);
        _inputs.movement = true;
        _playerAnimator.enabled = true;
    }
}