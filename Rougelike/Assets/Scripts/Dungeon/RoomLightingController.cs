using System.Collections;
using System.Collections.Generic;
using tuleeeeee.Dungeon;
using UnityEngine;
using UnityEngine.Tilemaps;
using tuleeeeee.Misc;
using tuleeeeee.environment;
using static tuleeeeee.StaticEvent.StaticEventHandler;

public class RoomLightingController : MonoBehaviour
{
    private InstantiatedRoom instantiatedRoom;

    private void Awake()
    {
        instantiatedRoom = GetComponent<InstantiatedRoom>();
    }

    private void OnEnable()
    {
        OnRoomChanged += StaticEventHandler_OnRoomChanged;
    }
    private void OnDisable()
    {
        OnRoomChanged -= StaticEventHandler_OnRoomChanged;
    }

    private void StaticEventHandler_OnRoomChanged(RoomChangedEventArgs roomChangedEventArgs)
    {
        if (roomChangedEventArgs.room == instantiatedRoom.room && !instantiatedRoom.room.isLit)
        {
            FadeInRoomLighting();

            instantiatedRoom.ActivateEnvironmentGameObjects();

            FadeInEnvironmentLighting();

            FadeInDoors();

            instantiatedRoom.room.isLit = true;
        }
    }

    private void FadeInRoomLighting()
    {
        StartCoroutine(FadeInRoomLightingRoutine(instantiatedRoom));
    }

    private void FadeInEnvironmentLighting()
    {
        Material material = new Material(GameResources.Instance.variableLitShader);

        Environment[] environmentComponents = GetComponentsInChildren<Environment>();

        foreach (Environment environmentComponent in environmentComponents)
        {
            if (environmentComponent.spriteRenderer != null)
            {
                environmentComponent.spriteRenderer.material = material;
            }

            StartCoroutine(FadeInEnvironmentLightingRoutine(material, environmentComponents));
        }
    }

    private IEnumerator FadeInEnvironmentLightingRoutine(Material material, Environment[] environmentComponents)
    {

        for (float i = 0.05f; i <= 1f; i += Time.deltaTime / Settings.fadeInTime)
        {
            material.SetFloat("Alpha_Slider", i);
            yield return null;
        }

        foreach (Environment environmentComponent in environmentComponents)
        {
            if (environmentComponent.spriteRenderer != null)
            {
                environmentComponent.spriteRenderer.material = GameResources.Instance.litMaterial;
            }
        }
    }

    private IEnumerator FadeInRoomLightingRoutine(InstantiatedRoom instantiatedRoom)
    {
        Material material = new Material(GameResources.Instance.variableLitShader);

        //instantiatedRoom.groundTilemap.GetComponent<TilemapRenderer>().material = material;

        foreach (Tilemap groundTilemap in instantiatedRoom.groundTilemaps) // more than 1 groundTilemap
        {
            groundTilemap.GetComponent<TilemapRenderer>().material = material;
        }

        instantiatedRoom.decoration1Tilemap.GetComponent<TilemapRenderer>().material = material;
        instantiatedRoom.decoration2Tilemap.GetComponent<TilemapRenderer>().material = material;
        instantiatedRoom.frontTilemap.GetComponent<TilemapRenderer>().material = material;
        instantiatedRoom.collisionTilemap.GetComponent<TilemapRenderer>().material = material;
        instantiatedRoom.minimapTilemap.GetComponent<TilemapRenderer>().material = material;

        for (float i = 0.05f; i <= 1f; i += Time.deltaTime / Settings.fadeInTime)
        {
            material.SetFloat("Alpha_Slider", i);
            yield return null;
        }

        //instantiatedRoom.groundTilemap.GetComponent<TilemapRenderer>().material = GameResources.Instance.litMaterial;
        foreach (Tilemap groundTilemap in instantiatedRoom.groundTilemaps) // more than 1 groundTilemap
        {
            groundTilemap.GetComponent<TilemapRenderer>().material = GameResources.Instance.litMaterial;
        }

        instantiatedRoom.decoration1Tilemap.GetComponent<TilemapRenderer>().material = GameResources.Instance.litMaterial;
        instantiatedRoom.decoration2Tilemap.GetComponent<TilemapRenderer>().material = GameResources.Instance.litMaterial;
        instantiatedRoom.frontTilemap.GetComponent<TilemapRenderer>().material = GameResources.Instance.litMaterial;
        instantiatedRoom.collisionTilemap.GetComponent<TilemapRenderer>().material = GameResources.Instance.litMaterial;
        instantiatedRoom.minimapTilemap.GetComponent<TilemapRenderer>().material = GameResources.Instance.litMaterial;
    }

    private void FadeInDoors()
    {
        Door[] doorArray = GetComponentsInChildren<Door>();

        foreach (Door door in doorArray)
        {
            DoorLightingController doorLightingControl = door.GetComponentInChildren<DoorLightingController>();

            doorLightingControl.FadeInDoor(door);
        }
    }
}
