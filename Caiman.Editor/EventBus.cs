using System.Diagnostics;

namespace Caiman.Editor;

public class EventBus
{
    private static EventBus? _instance;

    private readonly Dictionary<string, List<object?>> _handlers = new();
    public static EventBus Instance { get; } = _instance ??= new EventBus();

    public void Subscribe<T>(Action<T>? handler)
    {
        string key = typeof(T).Name;

        if (_handlers.ContainsKey(key))
        {
            _handlers[key].Add(handler);
        }
        else
        {
            _handlers.Add(key, [handler]);
        }
    }

    public void Unsubscribe<T>(Action<T>? handler)
    {
        string key = typeof(T).Name;
        if (_handlers.ContainsKey(key))
        {
            _handlers.Remove(key);
        }
        else
        {
            Debug.WriteLine($"EventBus Unsub: No handler found for {key}");
        }
    }

    public void Invoke<T>(T signal)
    {
        string key = typeof(T).Name;
        if (!_handlers.ContainsKey(key))
        {
            return;
        }

        foreach (Action<T>? handler in _handlers[key])
        {
            handler?.Invoke(signal);
        }
    }
}
