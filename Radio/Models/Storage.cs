using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radio.Models
{
   public static class Storage
   {
       private static Dictionary<string,object> windowStorage = new Dictionary<string, object>();
       public static Dictionary<string, object> WindowStorage
        {
           get { return windowStorage; }
           set
           {
               windowStorage = value;
           }
       }
       private static Dictionary<string, object> vmStorage = new Dictionary<string, object>();
       public static Dictionary<string, object> VmStorage
       {
           get { return vmStorage; }
           set
           {
               vmStorage = value;
           }
       }
    }
}
