using System.Collections.Generic;

public class SelectionSubject : Singleton<SelectionSubject>
{
    private List<ISelectionObserver> _observers = new List<ISelectionObserver>();

    public void Attach(ISelectionObserver observer)
    {
        _observers.Add(observer);
    }

    public void Detach(ISelectionObserver observer)
    {
        _observers.Remove(observer);
    }

    protected void Notify(WorldEntity entity)
    {
        foreach (var observer in _observers)
        {
            observer.OnSelect(entity);
        }
    }
}