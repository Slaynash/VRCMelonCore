using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRC.UI;

namespace VRCMelonCore.Extensions
{
    public static class VRCUiManagerExtension
    {
        public static void ShowScreen(this VRCUiManager vrcUiManager, string screen, bool additive = false)
        {
            vrcUiManager.Method_Public_String_Boolean_0(screen, additive);
        }
        /*
        public static void ShowUi(this VRCUiManager vrcUiManager, bool _1 = true, bool _2 = true)
        {
            vrcUiManager.Method_Public_Boolean_Boolean_0(_1, _2);
        }

        public static void PlaceUi(this VRCUiManager vrcUiManager, bool _1 = false)
        {
            vrcUiManager.Method_Public_Boolean_0(_1);
        }
        */
    }
}
