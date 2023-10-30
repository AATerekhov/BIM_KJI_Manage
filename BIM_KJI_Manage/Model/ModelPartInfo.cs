using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tekla.Structures.Model;


namespace Propotype_Manage.Model
{
    public class ModelPartInfo
    {
        //TODO: Устаревший класс, найза современную замену. Струкрутирование атрибутивной информации.
        public ModelObject InObject { get; set; }
        public  ModelObject.ModelObjectEnum TeklaType { get; set; }
        public double Weight { get; set; }
        public int Layout { get; set; }
        public int Number { get; set; }

        public ModelPartInfo(ModelObject inObject)
        {
            if (inObject is Part part)
            {
                InObject = inObject;
                TeklaType = ModelObject.ModelObjectEnum.CUSTOM_PART;

                var weight = 0.0;
                InObject.GetUserProperty("bim_m", ref weight);
                Weight =    weight;
                var number = 0;
                InObject.GetUserProperty("bim_rb_n", ref number);
                Number = number;
                Number = 1;
            }
            else if (inObject is RebarGroup rebars)
            {
                InObject = inObject;
                TeklaType = ModelObject.ModelObjectEnum.REBARGROUP;

                var weight = 0.0;
                InObject.GetUserProperty("bim_m", ref weight);
                Weight = weight;
                var number = 0;
                InObject.GetUserProperty("bim_rb_n", ref number);
                Number = number;
                var layput = 0;
                InObject.GetUserProperty("bm_rb_l", ref layput);
                Layout = layput;                
            }
        }
    }
}
