using System;
using System.Collections.Generic;

public static class EventBus
{
    public static event Action OnPlayerPassedOut;
    public static event Action ResetScene;

    public static void RaiseOnPlayerPassedOut()
    {
        OnPlayerPassedOut?.Invoke();
    }
    
    public static void RaiseResetScene()
    {
        ResetScene?.Invoke();
    }
    
}