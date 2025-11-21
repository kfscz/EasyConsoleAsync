using System.Diagnostics;

namespace EasyConsole;

public abstract class Program
{

  readonly Dictionary<Type, Page> _pages = new();

  protected string Title { get; set; }

  public bool BreadcrumbHeader { get; private set; }

  protected Page? CurrentPage => History.TryPeek(out var r) ? r : null;

  public Stack<Page> History { get; } = new();

  public bool NavigationEnabled { get { return History.Count > 1; } }

  protected Program(string title, bool breadcrumbHeader)
  {
    Title = title;
    BreadcrumbHeader = breadcrumbHeader;
  }

  public async Task RunAsync(CancellationToken cancellationToken = default)
  {
    try
    {
      Console.Title = Title;
      await DisplayCurrentPageAsync(cancellationToken);
    }
    catch (Exception e)
    {
      Output.WriteLine(ConsoleColor.Red, e.ToString());
    }
    finally
    {
      if (Debugger.IsAttached)
      {
        Input.ReadString("Press [Enter] to exit");
      }
    }
  }

  public void AddPage(Page page)
  {
    Type pageType = page.GetType();
    if (_pages.ContainsKey(pageType))
    {
      _pages[pageType] = page;
    }
    else
    {
      _pages.Add(pageType, page);
    }
  }

  public async Task NavigateHomeAsync(CancellationToken cancellationToken)
  {
    while (History.Count > 1)
    {
      History.Pop();
    }
    Console.Clear();
    await DisplayCurrentPageAsync(cancellationToken);
  }

  public T? SetPage<T>() where T : Page
  {
    Type pageType = typeof(T);

    if (CurrentPage is T tPage)
      return tPage;

    // leave the current page

    // select the new page
    if (!_pages.TryGetValue(pageType, out var nextPage))
      throw new KeyNotFoundException("The given page \"{0}\" was not present in the program".Format(pageType));

    // enter the new page
    History.Push(nextPage);

    return CurrentPage as T;
  }

  public async Task<T?> NavigateToAsync<T>(CancellationToken cancellationToken) where T : Page
  {
    SetPage<T>();

    Console.Clear();
    await DisplayCurrentPageAsync(cancellationToken);
    return CurrentPage as T;
  }

  public async Task<Page?> NavigateBackAsync(CancellationToken cancellationToken)
  {
    History.Pop();
    Console.Clear();
    await DisplayCurrentPageAsync(cancellationToken);
    return CurrentPage;
  }

  private async Task DisplayCurrentPageAsync(CancellationToken cancellationToken)
  {
    Debug.Assert(CurrentPage is not null);
    await (CurrentPage?.DisplayAsync(cancellationToken) ?? Task.CompletedTask);
  }
}
