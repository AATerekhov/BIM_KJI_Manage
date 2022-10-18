using System;
using System.Xml.Serialization;
using Tekla.Structures.Model;

namespace BIMPropotype_Lib.Model
{
    [Serializable]
    public class BIMBeam:BIMPart, IUDAContainer
    {
        public Beam InBeam { get; set; }
        public SupportDistanse SupportPoints { get; set; }

        [XmlIgnore]
        public override string Name
        {
            get { return InBeam.Name; }
            set { if (InBeam != null) InBeam.Name = value; }
        }

        [XmlIgnore]
        public override string Class
        {
            get { return InBeam.Class; }
            set { if (InBeam != null) InBeam.Class = value; }
        }
        [XmlIgnore]
        public override string Profile
        {
            get { return InBeam.Profile.ProfileString; }
            set { if (InBeam != null) InBeam.Profile.ProfileString = value; }
        }
        [XmlIgnore]
        public override string Material
        {
            get { return InBeam.Material.MaterialString; }
            set { if (InBeam != null) InBeam.Material.MaterialString = value; }
        }
        public BIMBeam()
        {

        }
        public BIMBeam(Beam beam )
        {
            InBeam = beam;
            SupportPoints = new SupportDistanse(beam.StartPoint, beam.EndPoint);
            InBeam.StartPoint = null;
            InBeam.EndPoint = null;

            UDAList = new UDACollection(InBeam);
            GetRebar(InBeam.GetReinforcements());
            Pruning = new BIMPruning(InBeam.GetBooleans());
            GetBolts(InBeam.GetBolts());
            if (CheckMainPart(InBeam)) GetPutInAssembly(InBeam);
        }

        public override void InsertMirror()
        {
            InsertSuportPoint();
            base.InsertMirror(InBeam);
        }
        public override void Insert()
        {
            InsertSuportPoint();
            base.Insert(InBeam);
        }
        private void InsertSuportPoint() 
        {
            InBeam.StartPoint = SupportPoints.Start;
            InBeam.EndPoint = SupportPoints.End;
        }
    }
}
