using System;

public class GameCallbacks
{
    public static event Action OnMatchFound = delegate { };
    public static event Action OnTurnsUpdated = delegate { };

    public void RaiseMatchFoundEvent()
    {
        OnMatchFound.Invoke();
    }

    public void RaiseTurnUpdateEvent()
    {
        OnTurnsUpdated.Invoke();
    }
}
