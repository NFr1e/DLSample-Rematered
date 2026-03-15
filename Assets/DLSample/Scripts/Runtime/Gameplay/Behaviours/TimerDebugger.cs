using DLSample.Gameplay;
using DLSample.Gameplay.Behaviours;
using DLSample.Gameplay.Stream;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class TimerDebugger : GameplayObject
{
    public Text timeText;
    private GameplayTimer timer;

    protected override void OnStart()
    {
        timer = GameplayEntry.Instance.ServiceLocator.Get<GameplayTimer>();
    }

    private void Update()
    {
        if (timer != null)
        {
            timeText.text = timer.CurrentTime.ToString();
        }
    }
}
