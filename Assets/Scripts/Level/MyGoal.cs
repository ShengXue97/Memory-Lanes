using System.Collections;
using UnityEngine;

public interface IGoalListener
{
    void OnFinishLevel();
    void OnExitLevel();
}

public class MyGoal : MonoBehaviour
{
    [SerializeField]
    private float exitingTime = 2f;
    private bool isExiting;

    private IGoalListener listener;
    
    public void SetListener(IGoalListener listener)
    {
        this.listener = listener;
    }

    public void FinishLevel()
    {
        if (isExiting)
        {
            return;
        }
        isExiting = true;
        listener?.OnFinishLevel();
        StartCoroutine(ExitLevelRoutine());
    }

    IEnumerator ExitLevelRoutine()
    {
        yield return new WaitForSeconds(exitingTime);
        listener?.OnExitLevel();
    }
}
