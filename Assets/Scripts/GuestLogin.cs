using System.Collections;
using UnityEngine;
using LootLocker.Requests;
using UnityEngine.UI;

public class GuestLogin : MonoBehaviour
{
    public enum GameState {MenuIdle, LoggingIn, Error, LoggedIn, Play };

    public GameState gameState = GameState.MenuIdle;

    public Animator buttonAnimator;

    public Button button;

    // Start is called before the first frame update
    void Start()
    {
        // Start GuestLoginRoutine() here to log in in the background
    }

    public void ButtonPress()
    {
        if (ValidAnimationIsPlaying() == false)
        {
            return;
        }
        switch (gameState)
        {
            case GameState.MenuIdle:
                Login();
                break;
            case GameState.Error:
                Login();
                break;
            case GameState.LoggedIn:
                Play();
                break;
            case GameState.Play:
                buttonAnimator.SetTrigger("Back");
                gameState = GameState.MenuIdle;
                break;
            default:
                break;
        }
    }

    private void Login()
    {
        gameState = GameState.LoggingIn;
        buttonAnimator.SetTrigger("Login");
        StartCoroutine(GuestLoginRoutine());
    }

    private void Play()
    {
        gameState = GameState.Play;
        buttonAnimator.SetTrigger("Play");
    }

    private IEnumerator GuestLoginRoutine()
    {
        bool gotResponse = false;
        LootLockerSDKManager.StartGuestSession((response) =>
        {
            if (response.success)
            {
                gotResponse = true;
                gameState = GameState.LoggedIn;
            }
            else
            {
                gameState = GameState.Error;
                gotResponse = true;
            }
        });

        // Wait until we've gotten a response
        yield return new WaitWhile(() => gotResponse == false);


        // Play correct animation based on gameState
        if (gameState == GameState.Error)
        {
            buttonAnimator.SetTrigger("Error");
        }
        else if (gameState == GameState.LoggedIn)
        {
            buttonAnimator.SetTrigger("LoggedIn");
        }
    }

    bool ValidAnimationIsPlaying()
    {
        if (buttonAnimator.GetCurrentAnimatorStateInfo(0).IsName("IdleError"))
        {
            return true;
        }
        if (buttonAnimator.GetCurrentAnimatorStateInfo(0).IsName("IdlePlay"))
        {
            return true;
        }
        if (buttonAnimator.GetCurrentAnimatorStateInfo(0).IsName("IdleLogin"))
        {
            return true;
        }
        if (buttonAnimator.GetCurrentAnimatorStateInfo(0).IsName("IdleCross"))
        {
            return true;
        }
        return false;
    }

    public bool isLoggedIn;
    private void SimpleGuestLogin()
    {
        LootLockerSDKManager.StartGuestSession((response) => { if(response.success) isLoggedIn = true; });
    }

}
