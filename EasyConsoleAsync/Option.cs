namespace EasyConsole;

public class Option
{
  public string Name { get; }
  readonly Func<CancellationToken, Task> _callback;

  public Option(string name, Action callback) : this(
    name: name,
    callback: cancelToken => {
      callback.Invoke();
      return Task.CompletedTask;
    })
  { }

  public Option(string name, Func<CancellationToken, Task> callback)
  {
    if (string.IsNullOrEmpty(name))
    {
      throw new ArgumentException($"'{nameof(name)}' cannot be null or empty.", nameof(name));
    }
    Name = name;
    _callback = callback ?? throw new ArgumentNullException(nameof(callback));
  }

  public Task InvokeAsync(CancellationToken cancellationToken) => _callback(cancellationToken);

  public override string ToString() => Name;

}
