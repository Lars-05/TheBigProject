using System;
using System.Collections.Generic;

public static class EventBus
{
    public static event Action OnPlayerPassedOut;
    
    public static event Action OnCassetteStop;
    
    public static event Action OnCassetteStart;

    public static event Action OnWin;
    public static event Action ResetScene;

    public static void RaiseOnPlayerPassedOut()
    {
        OnPlayerPassedOut?.Invoke();
    }
    
    public static void RaiseOnCassetteStarted()
    {
        OnCassetteStart?.Invoke();
    }
    
    public static void RaiseOnWin()
    {
        OnWin?.Invoke();
    }
    
    public static void RaiseOnCassetteStopped()
    {
        
        OnCassetteStop?.Invoke();
    }
    
    public static void RaiseResetScene()
    {
        ResetScene?.Invoke();
    }
    
}