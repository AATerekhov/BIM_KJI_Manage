﻿using System;
using System.Xml.Serialization;
using Tekla.Structures.Model;

namespace BIMPropotype_Lib.Model
{
    [Serializable]
    public class BIMBeam:BIMPart
    {
        public CustomBeam Beam { get; set; }
        public BIMBeam() { }

        #region Interface //Интрефейс нужно перенести во ViewModel!!!
        [XmlIgnore]
        public override string Name
        {
            get { return Beam.Beam.Name; }
            set { if (Beam != null) Beam.Beam.Name = value; }
        }

        [XmlIgnore]
        public override string Class
        {
            get { return Beam.Beam.Class; }
            set { if (Beam != null) Beam.Beam.Class = value; }
        }
        [XmlIgnore]
        public override string Profile
        {
            get { return Beam.Beam.Profile.ProfileString; }
            set { if (Beam != null) Beam.Beam.Profile.ProfileString = value; }
        }
        [XmlIgnore]
        public override string Material
        {
            get { return Beam.Beam.Material.MaterialString; }
            set { if (Beam != null) Beam.Beam.Material.MaterialString = value; }
        }
        #endregion//Интрефейс нужно перенести во ViewModel!!!

        public BIMBeam(Beam beam )
        {
            Beam = new CustomBeam(beam);

            UDAList = new UDACollection(Beam.Beam);
            GetRebar(Beam.Beam.GetReinforcements());
            Pruning = new BIMPruning(Beam.Beam.GetBooleans());
            GetBolts(Beam.Beam.GetBolts());
            if (CheckMainPart(Beam.Beam)) GetPutInAssembly(Beam.Beam);
        }

        public override void InsertMirror()
        {
            Beam.GetBeam();
            base.InsertMirror(Beam.Beam);
        }
        public override void Insert()
        {
            Beam.GetBeam();
            base.Insert(Beam.Beam);
        }
    }
}
