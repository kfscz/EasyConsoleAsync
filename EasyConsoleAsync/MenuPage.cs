namespace EasyConsole
{
  public abstract class MenuPage : Page
  {
    protected Menu Menu { get; set; }

    public MenuPage(string title, Program program, params Option[] options)
        : base(title, program)
    {
      Menu = new Menu();

      foreach (var option in options)
        Menu.Add(option);
    }

    public override async Task DisplayAsync(CancellationToken cancellationToken)
    {
      await base.DisplayAsync(cancellationToken);

      if (Program.NavigationEnabled && !Menu.Contains("Go back"))
      {
        Menu.Add("Go back", Program.NavigateBackAsync);
      }
      await Menu.DisplayAsync(cancellationToken);
    }
  }
}
