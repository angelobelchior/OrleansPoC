namespace OrleansPoC.UI;

public class LimitedObservableCollection<T>(int maxItems) : ObservableCollection<T>
{
    public new void Add(T item)
    {
        if (Count >= maxItems)
            RemoveAt(Count - 1);

        Insert(0, item);
    }
}