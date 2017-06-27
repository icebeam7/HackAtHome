using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using HackAtHome.SAL;
using Android.Webkit;

namespace HackAtHome.Client
{
    [Activity(Label = "@string/ApplicationName", Icon = "@drawable/icon")]
    public class DetalleEvidenciaActivity : Activity
    {
        string token, fullname, evidence_status, evidence_title;
        int evidence_id;

        protected async override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.DetalleEvidencia);

            token = Intent.Extras.GetString("token");
            fullname = Intent.Extras.GetString("fullname");
            evidence_id = Intent.Extras.GetInt("evidence_id", 0);
            evidence_status = Intent.Extras.GetString("evidence_status");
            evidence_title = Intent.Extras.GetString("evidence_title");

            var txvNombre = FindViewById<TextView>(Resource.Id.txvNombre);
            txvNombre.Text = fullname;

            var txvTitulo = FindViewById<TextView>(Resource.Id.txvTitulo);
            txvTitulo.Text = evidence_title;

            var txvStatus = FindViewById<TextView>(Resource.Id.txvStatus);
            txvStatus.Text = evidence_status;

            var ServiceClient = new ServiceClient();
            var evidencia = await ServiceClient.GetEvidenceByIDAsync(token, evidence_id);

            var imgEvidencia = FindViewById<ImageView>(Resource.Id.imgEvidencia);
            Koush.UrlImageViewHelper.SetUrlDrawable(imgEvidencia, evidencia.Url);

            var webDescripcion = FindViewById<WebView>(Resource.Id.webDescripcion);
            webDescripcion.SetBackgroundColor(Android.Graphics.Color.Black);
            string webViewContent = $"<div style='color:#BCBCBC; margin:20px'>{evidencia.Description}</div>";
            webDescripcion.LoadDataWithBaseURL(null, webViewContent, "text/html", "utf-8", null);
        }
    }
}