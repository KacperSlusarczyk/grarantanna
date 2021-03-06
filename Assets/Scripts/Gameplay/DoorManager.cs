﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class DoorManager : MonoBehaviour
{
    [SerializeField] private SpriteRenderer door;
    [SerializeField] private Sprite closedDoor;
    [SerializeField] private Sprite openedDoor;
    [SerializeField] private GameObject keyIcon;

    public bool HasKey { get; private set; }
    private bool canEnter = false;

    private void Start()
    {
        keyIcon = GameObject.Find("KeyIcon");
        keyIcon.SetActive(false);
    }

    public void GetKey ()
    {
        HasKey = true;
        keyIcon.SetActive(true);
    }

    private void Update()
    {
        if(canEnter && HasKey)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                door.sprite = openedDoor;
                PlayerController.CurrentPlayer.GetComponent<PlayerController>().canMove = false;
                PlayerController.CurrentPlayer.GetComponent<Animator>().Play("Win");
                LevelsManager.Manager.AddCoins(CoinsCollector.Collector.currentCoinsCount, SceneManager.GetActiveScene().buildIndex);
                LevelsManager.Manager.SaveProgress();
                SoundsManager.CurrentManager.PlaySound(SoundsManager.DOOR_SOUND);
                GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>().SetCamSize(3f);

                if (FindObjectOfType<TimerLose>() != null)
                    Destroy(FindObjectOfType<TimerLose>());

                StartCoroutine(WaitForWin());
            }
        }
    }

    private IEnumerator WaitForWin ()
    {
        yield return new WaitForSecondsRealtime(2.5f);
        WinGame.CurrentScreen.Win();
    }

    private void OnTriggerEnter2D(Collider2D collision) => canEnter = true;

    private void OnTriggerExit2D(Collider2D collision) => canEnter = false;
}
