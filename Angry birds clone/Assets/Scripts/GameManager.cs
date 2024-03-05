using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float secondsToWait = 3f;

    public static GameManager instance;

    public int maxNumberOfShots = 3;
    private int usedNumberOfShots;

    private IconHandler iconHandler;

    private List<Pig> pigList = new List<Pig>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        iconHandler = FindObjectOfType<IconHandler>();

        Pig[] pigs = FindObjectsOfType<Pig>();

        //* Its taking all the pigs from the array and assigning to the pig list
        for (int i = 0; i < pigs.Length; i++)
        {
            pigList.Add(pigs[i]);
        }
    }

    public void UseShot()
    {
        usedNumberOfShots++;
        iconHandler.UseShot(usedNumberOfShots);

        CheckForLastShot();
    }

    public bool HasEnoughShots()
    {
        if (usedNumberOfShots < maxNumberOfShots)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void CheckForLastShot()
    {
        if (usedNumberOfShots == maxNumberOfShots)
        {
            StartCoroutine(CheckAfterWaitTime());
        }
    }

    private IEnumerator CheckAfterWaitTime()
    {
        yield return new WaitForSeconds(secondsToWait);

        if (pigList.Count <= 0)
        {
            WinGame();
        }
        else
        {
            LoseGame();
        }
    }

    public void RemovePig(Pig pig)
    {
        pigList.Remove(pig);
    }

    private void CheckForAllDeadPigs()
    {
        if (pigList.Count == 0)
        {
            WinGame();
        }
    }

    #region Win/Lose

    private void WinGame()
    {
        Debug.Log("Win");
    }

    private void LoseGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    #endregion
}
