namespace SensorsAndPeripherals.Controls
{
    public partial class DataGuard : ContentView
    {
        public static readonly BindableProperty HasDataProperty =
            BindableProperty.Create(nameof(HasData), typeof(bool), typeof(DataGuard), true);

        public bool HasData
        {
            get => (bool)GetValue(HasDataProperty);
            set => SetValue(HasDataProperty, value);
        }

        public static readonly BindableProperty EmptyTextProperty =
            BindableProperty.Create(nameof(EmptyText), typeof(string), typeof(DataGuard), string.Empty);

        public string EmptyText
        {
            get => (string)GetValue(EmptyTextProperty);
            set => SetValue(EmptyTextProperty, value);
        }

        public DataGuard()
        {
            ControlTemplate = new ControlTemplate(() =>
            {
                var grid = new Grid();
                var contentPresenter = new ContentPresenter();
                contentPresenter.SetBinding(IsVisibleProperty, new Binding(nameof(HasData), source: RelativeBindingSource.TemplatedParent));

                var emptyLabel = new Label
                {
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    IsVisible = true
                };
                emptyLabel.SetBinding(Label.TextProperty, new Binding(nameof(EmptyText), source: RelativeBindingSource.TemplatedParent));

                var hideLabelTrigger = new DataTrigger(typeof(Label))
                {
                    Binding = new Binding(nameof(HasData), source: RelativeBindingSource.TemplatedParent),
                    Value = true
                };
                hideLabelTrigger.Setters.Add(new Setter { Property = IsVisibleProperty, Value = false });
                emptyLabel.Triggers.Add(hideLabelTrigger);

                grid.Add(contentPresenter);
                grid.Add(emptyLabel);

                return grid;
            });
        }
    }
}