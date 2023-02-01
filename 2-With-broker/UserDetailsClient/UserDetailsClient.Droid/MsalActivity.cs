using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Microsoft.Identity.Client;

namespace UserDetailsClient.Droid
{
    [Activity(Exported=true)]
    [IntentFilter(new[] { Intent.ActionView },
    Categories = new[] { Intent.CategoryBrowsable, Intent.CategoryDefault },
    DataHost = "auth",
    DataScheme = "msalf899ce88-2ef8-4830-8052-fa7f72a49e07")]
    public class MsalActivity : BrowserTabActivity
    {
    }
}