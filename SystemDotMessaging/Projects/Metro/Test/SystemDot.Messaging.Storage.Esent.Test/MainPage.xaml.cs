using SystemDot.Esent;
using SystemDot.Files;
using SystemDot.Serialisation;
using SystemDot.Storage.Changes;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace SystemDot.Messaging.Storage.Esent.Test
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var store = new EsentChangeStore(new FileSystem(), new PlatformAgnosticSerialiser());
            store.Initialise("Messaging");

            var changes = store.GetChanges("Test");

            for (int i = 0; i < 5000; i++)
            {
                store.StoreChange("Test", new TestChange { Number = i });
            }

            changes = store.GetChanges("Test");
        }
    }

    public class TestChange : Change
    {
        public int Number { get; set; }
    }
}
