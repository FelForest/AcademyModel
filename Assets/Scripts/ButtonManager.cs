using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    public GameObject capture;
    public GameObject background;
    public GameObject buttons;
    public GameObject backgroundImage;
    public GameObject backbutton;
    public GameObject startbutton;
    public GameObject board;
    public Text title;


    public void ARStart()
    {
        startbutton.SetActive(false);
        background.SetActive(true);
        buttons.SetActive(true);
        title.text = "모델을 선택하세요.";
    }

    public void ChooseModel()
    {
        capture.SetActive(true);
        backbutton.SetActive(true);
        buttons.SetActive(false);
        background.SetActive(false);
        backgroundImage.SetActive(false);
        board.SetActive(false);
    }

    public void BackChoose()
    {
        capture.SetActive(false);
        backbutton.SetActive(false);
        buttons.SetActive(true);
        background.SetActive(true);
        backgroundImage.SetActive(true);
        board.SetActive(true);
    }
}
