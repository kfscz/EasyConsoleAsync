namespace EasyConsole;

public abstract class Page
{
  public string Title { get; }

  public Program Program { get; }

  public Page(string title, Program program)
  {
    if (string.IsNullOrEmpty(title))
    {
      throw new ArgumentException($"'{nameof(title)}' cannot be null or empty.", nameof(title));
    }
    Title = title;
    Program = program ?? throw new ArgumentNullException(nameof(program));
  }

  public virtual Task DisplayAsync(CancellationToken cancellationToken)
  {
    if (Program.History.Count > 1 && Program.BreadcrumbHeader)
    {
      string breadcrumb = string.Empty;
      foreach (var title in Program.History.Select((page) => page.Title).Reverse())
        breadcrumb += title + " > ";
      breadcrumb = breadcrumb.Remove(breadcrumb.Length - 3);
      Console.WriteLine(breadcrumb);
    }
    else
    {
      Console.WriteLine(Title);
    }
    Console.WriteLine("---");
    return Task.CompletedTask;
  }
}
