using System;

public class GameCallbacks
{
    public static event Action<bool> OnMatchFound = delegate { };
    public static event Action OnTurnsUpdated = delegate { };

    public void RaiseMatchFoundEvent(bool value)
    {
        OnMatchFound.Invoke(value);
    }

    public void RaiseTurnUpdateEvent()
    {
        OnTurnsUpdated.Invoke();
    }
}
