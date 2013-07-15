using SystemDot.Configuration;
using SystemDot.Ioc;
using SystemDot.Logging;
using SystemDot.Messaging;
using SystemDot.Messaging.Configuration;
using Android.App;
using Android.Content.Res;
using Android.Widget;
using Android.OS;
using Messages;

namespace AndroidApplication1
{
    [Activity(Label = "AndroidApplication1", MainLauncher = true, Icon = "@drawable/icon")]
    public class Activity1 : Activity
    {
        int count = 1;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            var container = new IocContainer();
            container.RegisterFromAssemblyOf<Activity1>();

            AssetManagerLocator.Set(Assets);
            Configure.Messaging()
                .LoggingWith(new ConsoleLoggingMechanism { ShowInfo = true, ShowDebug = false })
                .ResolveReferencesWith(container)
                .UsingHttpTransport()
                    .AsAServerUsingAProxy("SenderServer", "Proxy")
                .OpenChannel("TestAndroidRequest")
                    .ForRequestReplySendingTo("TestReply@ReceiverServer")
                    .WithDurability()
                .Initialise();

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            Button button = FindViewById<Button>(Resource.Id.MyButton);

            button.Click += delegate
            {
                Bus.Send(new TestMessage {Text = string.Format("Message {0}", count)});
                button.Text = string.Format("{0} messages sent!", count++);
            };
        }
    }
}

