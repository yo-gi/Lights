﻿using UnityEngine;
using System.Collections.Generic;

public class MainCam : MonoBehaviour
{
    public static MainCam S;

    public GameObject playerObj;

    public bool __________________;

    public bool invincible = false;

    public static int currentLevel = 1;
    public static Dictionary<int, Vector3> startTable = new Dictionary<int, Vector3>();
    public static Dictionary<int, GameObject> levelTable = new Dictionary<int, GameObject>();

    public static readonly KeyCode resetKey = KeyCode.R;
    public static readonly KeyCode invincibilityKey = KeyCode.I;

    void Awake()
    {
        S = this;

        GameObject levelObject = GameObject.Find("Level_" + currentLevel);
        while (levelObject != null)
        {
            levelTable[currentLevel] = levelObject;
            levelObject.SetActive(false);
            levelObject = GameObject.Find("Level_" + (++currentLevel));
        }
        currentLevel = 1;
        levelTable[1].SetActive(true);

        startTable[1] = new Vector3(1, 3, 0);
        startTable[2] = new Vector3(23, 3, 0);
        startTable[3] = new Vector3(55, 13, 0);
        startTable[4] = new Vector3(93, 18, 0);
    }

    // Update is called once per frame
    void Update()
    {
        // TODO: lerp the camera instead of suddenly changing position
        Transform t = playerObj.transform;
        Vector3 pos = new Vector3(t.position.x + 3, t.position.y, -10f);
        transform.position = pos;

        // TODO: Add visual indication in game for Invincibility mode
        if (Input.GetKey(invincibilityKey))
        {
            invincible = !invincible;
            print("invincibility: " + invincible);
        }
        if (Input.GetKey(resetKey))
        {
            RestartLevel();
        }
    }

    // Given an object in the Hierarchy, returns list of children filtered by tag. If T is GameObject,
    // will return the children GameObjects, but if T is something else, will return component T from
    // the children GameObjects.
    public static List<T> FilterByTag<T>(GameObject parent, string tag)
    {
        List<T> objects = new List<T>();
        foreach (Transform transform in parent.GetComponentsInChildren<Transform>())
        {
            if (transform.tag == tag)
            {
                if (typeof(T) == typeof(GameObject)) objects.Add((T)(object)transform.gameObject);
                else objects.Add(transform.gameObject.GetComponent<T>());
            }
        }
        return objects;
    }

    public static void NextLevel()
    {
        if (currentLevel == levelTable.Count)
        {
            // Win
        }
        else
        {
            SwapToLevel(currentLevel + 1);
        }
    }

    public static void RestartLevel()
    {
        Events.Broadcast(new OnResetEvent());
        SwapToLevel(currentLevel);
        Player.S.transform.position = startTable[currentLevel];
    }

    private static void SwapToLevel(int level)
    {
        levelTable[currentLevel].SetActive(false);
        levelTable[level].SetActive(true);
        currentLevel = level;
        Player.S.transform.position = startTable[currentLevel];
        Events.Broadcast(new OnLevelLoadEvent { level = currentLevel });
    }
}
