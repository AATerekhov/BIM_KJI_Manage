using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Tekla.Structures.Geometry3d;

namespace BIMPropotype_Lib.Model.Support
{
    [Serializable]
    public class SupportDistanse: SupportElement
    {
        [XmlIgnore]
        public Point Start
        {
            get { return GetFirst(); }
        }

        [XmlIgnore]
        public Point End
        {
            get { return GetSecond(); }
        }

        public SupportDistanse()
        {

        }
        public SupportDistanse(Point start, Point end)
        {
            base.Add(start);
            base.Add(end);
        }
    }
}
