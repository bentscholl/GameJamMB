using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerJoin : MonoBehaviour
{
    PlayerInput input;

    // Start is called before the first frame update
    void Start()
    {
        input = GetComponent<PlayerInput>();
        if (input.playerIndex == 0)
        {
            Image Combover = GameObject.Find("ComboverMenu").GetComponent<Image>();
            Combover.color = Color.white;
            Instantiate(Resources.Load("Combover"), transform);
        }
        else
        {
            Image Meathead = GameObject.Find("MeatheadMenu").GetComponent<Image>();
            Meathead.color = Color.white;
            Instantiate(Resources.Load("Meathead"), transform);
            StartCoroutine(StartGame());
        }
    }

    private IEnumerator StartGame()
    {
        Image ScreenSplit = GameObject.Find("ScreenSplit").GetComponent<Image>();
        GameObject StartPanel = GameObject.Find("StartPanel");
        TextMeshProUGUI StartText = StartPanel.GetComponentInChildren<TextMeshProUGUI>();
        StartText.text = "3";
        yield return new WaitForSeconds(1);
        StartText.text = "2";
        yield return new WaitForSeconds(1);
        StartText.text = "1";
        yield return new WaitForSeconds(1);
        StartText.text = "";
        yield return new WaitForSeconds(.2f);
        ScreenSplit.enabled = true;
        StartPanel.SetActive(false);
        Meathead.Instance.enabled = true;
        Combover.Instance.enabled = true;
    }

    
}
