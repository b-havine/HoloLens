using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Presentation
{
    //this class holds the strings that are being shown the user in the ui.
    class GuideTitles { 

         private string yesToStart = "Go and scan any missing areas, when you are ready say 'Yes'";
    
        private string workOrCapacity = "Welcome to the Microsoft Hololens Warehouse application, \n" +
                            "say 'one' to find out how large an area can be scanned\n" +
                            "say 'two' to start putting products at a chosen location";

        private string startPicking = "area has been optimized, now you can pickup the goods from the picklist by saying: 'go'";

        private string baydoorScanned = "baydoor scanned, say 'end' if you wish to proceed to picking, say 'back' to cancel";

        public string YesToStart { get { return yesToStart; } }
        public string WorkOrCapacity { get { return workOrCapacity; } }
        public string StartPicking { get { return startPicking; } }

        public string BaydoorScanned { get { return baydoorScanned; } }

    }
}
