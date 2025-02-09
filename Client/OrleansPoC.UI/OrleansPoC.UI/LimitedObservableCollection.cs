namespace OrleansPoC.UI;

public class LimitedObservableCollection<T> : ObservableCollection<T>
{
    private const int MaxItems = 12;
    public new void Add(T item)
    {
        if (Count >= MaxItems)
        {
            RemoveAt(Count - 1);
        }
        
        Insert(0, item);
    }
}