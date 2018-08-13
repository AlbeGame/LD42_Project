using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIController : MonoBehaviour
{

    public Button StartButton;

    public GameObject Stomach;
    Text pointsText;
    int points;
    List<Image> stomachBugPositions = new List<Image>();

    public void Start()
    {
        foreach (Image bugPos in Stomach.GetComponentsInChildren<Image>())
        {
            if (bugPos.gameObject != Stomach.gameObject)
            {
                stomachBugPositions.Add(bugPos);
                bugPos.color = Color.clear;
            }
        }

        pointsText = Stomach.GetComponentInChildren<Text>();
    }

    public void ToggleMainMenu(bool _on)
    {
        StartButton.gameObject.SetActive(_on);
        Stomach.SetActive(!_on);
        points = 0;
        if(pointsText)
            pointsText.text = "0";
        foreach (Image bugPos in stomachBugPositions)
                bugPos.color = Color.clear;
    }

    public void StomachAdd(Sprite _image, float _percentage = -1)
    {
        points++;
        pointsText.text = points.ToString();

        if (_percentage >= 0)
        {
            Image img = stomachBugPositions[(int)(_percentage * stomachBugPositions.Count)];
            img.sprite = _image;
            img.color = Color.white;
            return;
        }
        else
            for (int i = 0; i < stomachBugPositions.Count; i++)
            {
                if (stomachBugPositions[i].color.a > 0)
                {
                    stomachBugPositions[i].sprite = _image;
                    stomachBugPositions[i].color = Color.white;
                    return;
                }
            }
    }
}
