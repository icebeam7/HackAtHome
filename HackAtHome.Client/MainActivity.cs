using Android.App;
using Android.Widget;
using Android.OS;
using HackAtHome.SAL;
using HackAtHome.Entities;

namespace HackAtHome.Client
{
    [Activity(Label = "@string/ApplicationName", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);

            var edtCorreo = FindViewById<EditText>(Resource.Id.edtCorreo);
            var edtPassword = FindViewById<EditText>(Resource.Id.edtPassword);
            var ValidateButton = FindViewById<Button>(Resource.Id.ValidateButton);

            ValidateButton.Click += async (s, e) =>
            {
                try
                {
                    var ServiceClient = new ServiceClient();
                    string StudentEmail = edtCorreo.Text;
                    string Password = edtPassword.Text;
                    var Result = await ServiceClient.AutenticateAsync(StudentEmail, Password);

                    if (Result.Status == Status.Success)
                    {
                        var MicrosoftServiceClient = new MicrosoftServiceClient();
                        LabItem item = new LabItem()
                        {
                            DeviceId = Android.Provider.Settings.Secure.GetString(ContentResolver,
                                Android.Provider.Settings.Secure.AndroidId),
                            Email = StudentEmail,
                            Lab = "Hack@Home"
                        };

                        await MicrosoftServiceClient.SendEvidence(item);

                        var Intent = new Android.Content.Intent(this, typeof(EvidencesActivity));
                        Intent.PutExtra("token", Result.Token);
                        Intent.PutExtra("fullname", Result.FullName);
                        Intent.PutExtra("email", StudentEmail);
                        StartActivity(Intent);
                    }
                    else
                    {
                        Toast.MakeText(this, $"Error: {Result.Status}", ToastLength.Long);
                    }
                }
                catch(System.Exception ex)
                {

                }
            };
        }
    }
}

