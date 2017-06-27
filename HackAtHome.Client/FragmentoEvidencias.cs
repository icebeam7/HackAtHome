using System.Collections.Generic;
using Android.App;
using HackAtHome.Entities;
using Android.OS;

namespace HackAtHome.Client
{
    public class FragmentoEvidencias : Fragment
    {
        public List<Evidence> ListaEvidencias { get; set; }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RetainInstance = true;
        }
    }
}