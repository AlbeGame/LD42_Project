using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIController : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject GameplayMenu;

    public GameObject Stomach;
    Text pointsText;
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

        Object[] buttons = FindObjectsOfType(typeof(Button));
        foreach (Button btn in buttons)
        {
            switch (btn.name)
            {
                case "QuitButton":
                    btn.onClick.AddListener(GameManager.I.Quit);
                    break;
                case "StartButton":
                    btn.onClick.AddListener(GameManager.I.StartGame);
                    break;
                case "RestartButton":
                    btn.onClick.AddListener(GameManager.I.Restart);
                    break;
                case "Stomach":
                    btn.onClick.AddListener(StomachDigest);
                    break;
                default:
                    break;
            }
        }

        pointsText = Stomach.GetComponentInChildren<Text>();
    }

    public void ToggleMainMenu(bool _on)
    {
        MainMenu.SetActive(_on);
        GameplayMenu.SetActive(!_on);
        if (pointsText)
            pointsText.text = GameManager.I.Points.ToString();
        foreach (Image bugPos in stomachBugPositions)
            bugPos.color = Color.clear;
    }

    public void StomachDigest()
    {
        if (stomachBugPositions[stomachBugPositions.Count - 1].color.a <= 0)
            return;

        GameManager.I.PointsMultiplier += 0.3f;
        //GameManager.I.LilySpawner.SpawnInfo.DelayBetweenSpawns /= GameManager.I.PointsMultiplier;
        GameManager.I.BugsSpawner.SpawnInfo.DelayBetweenSpawns /= GameManager.I.PointsMultiplier;

        for (int i = stomachBugPositions.Count - 1; i > 0; i--)
        {
            if (stomachBugPositions[i].color.a > 0)
            {
                stomachBugPositions[i].color = Color.clear;
            }
        }
    }

    public void StomachAdd(Sprite _image, float _percentage = -1)
    {
        GameManager.I.Points += (int)GameManager.I.PointsMultiplier;
        pointsText.text = GameManager.I.Points.ToString();

        //if (_percentage >= 0)
        //{
        //    Image img = stomachBugPositions[(int)(_percentage * stomachBugPositions.Count)];
        //    img.sprite = _image;
        //    img.color = Color.white;
        //    return;
        //}
        //else
        for (int i = 0; i < stomachBugPositions.Count; i++)
        {
            if (stomachBugPositions[i].color.a == 0)
            {
                stomachBugPositions[i].sprite = _image;
                stomachBugPositions[i].color = Color.white;
                return;
            }
        }
    }
}
