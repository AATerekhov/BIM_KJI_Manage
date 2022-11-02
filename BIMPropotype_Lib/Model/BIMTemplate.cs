using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tekla.Structures.Geometry3d;
using TSM = Tekla.Structures.Model;
using BIMPropotype_Lib.Model.Custom;

namespace BIMPropotype_Lib.Model
{
    [Serializable]
    public class BIMTemplate : IStructure
    {
        public TemplateType Type { get; set; }
        public string Key { get; set; }
        public CoordinateSystem BaseStructure { get; set; }
        public BIMTemplate Template { get; set; }
        public BIMPart Part { get; set; }
        public BIMAssembly Assembly { get; set; }
        public BIMPartChildren PartChildren { get; set; }
        public BIMJoint Joint { get; set; }
        public BIMTemplate() { }
        public BIMTemplate(IStructure element)
        {
            if (element is BIMPart bIMPart) 
            {
                Type = TemplateType.Part;
            }
            else if (element is BIMPartChildren bIMPartChildren)
            {
                Type = TemplateType.ChildrenPart;
            }
            else if (element is BIMTemplate bIMTemplate)
            {
                Type = TemplateType.Template;
            }
            else if (element is BIMJoint bIMjoint)
            {
                Type = TemplateType.Joint;
            }
            else
            {
                Type = TemplateType.no;
            }
        }
        public IStructure GetStructure() 
        {
            if (Type == TemplateType.Part) return Part;
            else if (Type == TemplateType.Template) return Template;
            else if (Type == TemplateType.ChildrenPart) return PartChildren;
            else if (Type == TemplateType.Joint) return Joint;
            else return null;
        }

        public void Insert(IStructure father) => GetStructure().Insert(father);
        public void InsertMirror(IStructure father) => GetStructure().InsertMirror(father);
    }

    public enum TemplateType 
    {
        no = 0,
        Part = 1,
        ChildrenPart = 3,
        Template = 4,
        Joint = 5,
    }
}
