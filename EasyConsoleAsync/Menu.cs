namespace EasyConsole;

public class Menu
{

  readonly List<Option> _options = [];

  public Task DisplayAsync(CancellationToken cancellationToken)
  {
    for (int i = 0; i < _options.Count; i++)
    {
      Console.WriteLine("{0}. {1}", i + 1, _options[i].Name);
    }
    int choice = Input.ReadInt("Choose an option:", min: 1, max: _options.Count);

    return _options[choice - 1].InvokeAsync(cancellationToken);
  }

  public Menu Add(string option, Func<CancellationToken, Task> callback) 
    => Add(new(option, callback));

  public Menu Add(string option, Action callback) 
    => Add(new(option, callback));

  public Menu Add(Option option)
  {
    _options.Add(option);
    return this;
  }

  public bool Contains(string option) 
    => _options.FirstOrDefault((op) => op.Name.Equals(option)) is not null;

}
