using System;

public class InputSubscription : IDisposable 
{
    private Action _unsubscribe;
    
    public InputSubscription(Action unsubscribe)
    {
        _unsubscribe = unsubscribe;
    }

    public void Dispose()
    {
        _unsubscribe?.Invoke();
        _unsubscribe = null;
    }
}
