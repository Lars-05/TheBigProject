using System;
using System.Collections.Generic;

public static class EventBus
{
    public static event Action OnPlayerPassedOut;

    public static void RaiseOnPlayerPassedOut()
    {
        OnPlayerPassedOut?.Invoke();
    }
    
}