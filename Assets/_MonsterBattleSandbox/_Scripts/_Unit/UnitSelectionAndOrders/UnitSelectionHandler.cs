using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/*  ---About UnitSelectionHandler---
 *  Handles single select and box select.
 *  Calls Select or Deselect on each Unit.
 */

public class UnitSelectionHandler : MonoBehaviour
{
    [SerializeField] private RectTransform unitSelectionArea = null;
    [SerializeField] private LayerMask layerMaskToHit = new LayerMask();
    [SerializeField] private GameObject playerPrefab;

    private Vector2 startPosition;
    private Camera mainCamera;
    private Player_MonsterSandbox player;
    public List<Unit> SelectedUnits { get; } = new List<Unit>();
    private Unit singleSelectedUnit = null;

    private void Start()
    {
        mainCamera = Camera.main;
        player = playerPrefab.GetComponent<Player_MonsterSandbox>();
        // GameOverHandler.OnGameOver += GameOverDisableSelection;
    }

    private void OnDestroy()
    {
        // GameOverHandler.OnGameOver -= GameOverDisableSelection;
    }

    private void Update()
    {

        if(Mouse.current.leftButton.wasPressedThisFrame)
        {
            StartSelectionArea();
        }

        else if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            ClearSelectionArea();
        }

        else if (Mouse.current.leftButton.isPressed)
        {
            UpdateSelectionArea();
        }
    }

    private void StartSelectionArea()
    {
        if(!Keyboard.current.leftShiftKey.isPressed)
        {
            foreach (Unit selectedUnit in SelectedUnits)
            {
                selectedUnit.Deselect();
            }

            SelectedUnits.Clear();
        }

        unitSelectionArea.gameObject.SetActive(true);
        startPosition = Mouse.current.position.ReadValue();

        UpdateSelectionArea();
    }

    private void UpdateSelectionArea()
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();

        float areaWidth = mousePosition.x - startPosition.x;
        float areaHeight = mousePosition.y - startPosition.y;

        unitSelectionArea.sizeDelta = 
            new Vector2(Mathf.Abs(areaWidth), Mathf.Abs(areaHeight));

        unitSelectionArea.anchoredPosition = startPosition + 
            new Vector2(areaWidth / 2, areaHeight / 2);

        new Vector2(areaWidth / 2, areaHeight / 2);
    }

    private void ClearSelectionArea()
    {
        unitSelectionArea.gameObject.SetActive(false);

        if(unitSelectionArea.sizeDelta.magnitude == 0)
        {
            Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMaskToHit)) { return; }

            if (!hit.collider.TryGetComponent<Unit>(out Unit unit)) { return; }

            SelectedUnits.Add(unit);

            foreach (Unit selectedUnit in SelectedUnits)
            {
                selectedUnit.Select();
            }

            return;
        }

        Vector2 min = unitSelectionArea.anchoredPosition - (unitSelectionArea.sizeDelta / 2);
        Vector2 max = unitSelectionArea.anchoredPosition + (unitSelectionArea.sizeDelta / 2);

        foreach(Unit unit in player.GetMyUnits())
        {
            if(SelectedUnits.Contains(unit)) { continue; }

            Vector3 screenPosition = mainCamera.WorldToScreenPoint(unit.transform.position);

            if (screenPosition.x > min.x && 
                screenPosition.x < max.x && 
                screenPosition.y > min.y && 
                screenPosition.y < max.y)
            {
                SelectedUnits.Add(unit);
                // Debug.Log("Unit added to list");
                unit.Select();
            }
        }
    }

    //private void GameOverDisableSelection()
    //{
    //    enabled = false;
    //}
}
