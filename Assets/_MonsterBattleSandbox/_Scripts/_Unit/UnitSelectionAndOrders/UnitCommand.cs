using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

/* ---About UnitCommand---
 * Moves units (on UnitMovement) or targets them (TryTarget)
 * Check out the MouseManager's Unity Events:
 * They call TryMove and TryTarget
*/

public class UnitCommand : MonoBehaviour
{
    [SerializeField] private UnitSelectionHandler unitSelectionHandler = null;
    [SerializeField] private LayerMask layerMask = new LayerMask();

    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;

        // If you want to disable controls on game over or pause.
        // GameOverHandler.OnGameOver += GameOverDisableCommand;
    }

    private void OnDestroy()
    {
        // GameOverHandler.OnGameOver -= GameOverDisableCommand;
    }

    public void TryTarget(GameObject targetObject)
    {
        if (!targetObject.TryGetComponent<IAttackable>(out IAttackable newAttackableTarget)) { return; }

        if (targetObject.tag == "Player")
        {
            TryMove(targetObject.transform.position);
            return;
        }

        foreach (Unit unit in unitSelectionHandler.SelectedUnits)
        {
        unit.GetAttackTarget(targetObject);
        }
    }

    public void TryMove(Vector3 point)
    {
        foreach(Unit unit in unitSelectionHandler.SelectedUnits)
        {
            unit.SetDestination(point);
        }
    }

    private void GameOverDisableCommand()
    {
        enabled = false;
    }
}
