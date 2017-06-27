using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using HackAtHome.SAL;

namespace HackAtHome.Client
{
    [Activity(Label = "@string/ApplicationName", Icon = "@drawable/icon")]
    public class EvidencesActivity : Activity
    {
        string token, email, fullname;
        FragmentoEvidencias fragmentoEvidencias;

        protected async override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ListaEvidencias);

            token = Intent.Extras.GetString("token");
            email = Intent.Extras.GetString("email");
            fullname = Intent.Extras.GetString("fullname");

            var txvNombre = FindViewById<TextView>(Resource.Id.txvNombre);
            txvNombre.Text = fullname;

            fragmentoEvidencias = (FragmentoEvidencias)this.FragmentManager.FindFragmentByTag("Evidencias");

            if (fragmentoEvidencias == null)
            {
                fragmentoEvidencias = new FragmentoEvidencias();
                var ServiceClient = new ServiceClient();
                fragmentoEvidencias.ListaEvidencias = await ServiceClient.GetEvidencesAsync(token);

                var FragmentTransaction = this.FragmentManager.BeginTransaction();
                FragmentTransaction.Add(fragmentoEvidencias, "Evidencias");
                FragmentTransaction.Commit();
            }

            var lsvEvidencias = FindViewById<ListView>(Resource.Id.lsvEvidencias);
            lsvEvidencias.Adapter = new CustomAdapters.EvidencesAdapter(this, fragmentoEvidencias.ListaEvidencias,
                Resource.Layout.ListItem, Resource.Id.txvTitulo, Resource.Id.txvStatus);

            lsvEvidencias.ItemClick += (s, e) =>
            {
                var evidencia = fragmentoEvidencias.ListaEvidencias[e.Position];

                var Intent = new Intent(this, typeof(DetalleEvidenciaActivity));
                Intent.PutExtra("evidence_id", evidencia.EvidenceID);
                Intent.PutExtra("evidence_title", evidencia.Title);
                Intent.PutExtra("evidence_status", evidencia.Status);
                Intent.PutExtra("token", token);
                Intent.PutExtra("fullname", fullname);
                StartActivity(Intent);
            };
        }
    }
}