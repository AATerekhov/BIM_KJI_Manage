﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tekla.Structures;
using Tekla.Structures.Model;
using TSM = Tekla.Structures.Model;
using Tekla.Structures.Geometry3d;
using System.Xml.Serialization;
using System.IO;
using BIMPropotype_Lib.Model;
using UI = Tekla.Structures.Model.UI;
using System.Collections;

namespace BIMPropotype_Lib.Controller
{
    public class BeamLoader
    {
        public BIMAssembly InBIMAssembly { get; set; }
        public string Path { get; set; }
        public BeamLoader() { }
        public BeamLoader(TSM.Assembly InAssembly)
        {
            var SecondaryBeams = new List<Part>();
            Part MainPart = null;
            //TODO: Сделать проверку всех деталей на два типа Beam и ContourPlate.
            if (InAssembly.GetMainPart() is Part beam)//TODO: Работет только с Baem, рассмотреть варианты с пластиной.
            {
                MainPart = beam;
            }

            var ArreyChildren = InAssembly.GetSecondaries();//Получение второстепенных деталей в сборке.

            foreach (var child in ArreyChildren) 
            {
                if (child is Part beamChild)
                {
                    SecondaryBeams.Add(beamChild);
                }
            }

            InBIMAssembly = new BIMAssembly(GetAllBeams(MainPart, SecondaryBeams));

            SerializeXML();
        }


        public void GetPath()
        {
            string modelPath = string.Empty;
            TSM.Model model = new TSM.Model();
            if (model.GetConnectionStatus())
            {
                ModelInfo Info = model.GetInfo();
                modelPath = Info.ModelPath;
            }
            Path = modelPath;
        }

        public void SerializeXML()
        {
            if (Path == string.Empty || Path == null)
            {
                this.GetPath();
            }

            if (Path != string.Empty)
            {
                string path = $"{Path}\\RCP_Data\\Prototype_{InBIMAssembly.Prefix}.xml";
               if (File.Exists(path)) File.Delete(path);

                XmlSerializer formatter = new XmlSerializer(typeof(BIMAssembly));

                using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
                {
                    formatter.Serialize(fs, InBIMAssembly);
                    fs.Close();
                }
            }
        }
        /// <summary>
        /// Вставка балки из xml файла.
        /// </summary>
        /// <param name="fileName"></param>
        public void InsertPartXML(string fileName)
        {
            if (Path == string.Empty || Path == null)
            {
                this.GetPath();
            }
            if (File.Exists($"{Path}\\RCP_Data\\{fileName}.xml"))
            {
                var formatter = new XmlSerializer(typeof(BIMAssembly));

                if (this.Path != string.Empty)
                {
                    using (var fs = new FileStream($"{Path}\\RCP_Data\\{fileName}.xml", FileMode.OpenOrCreate))
                    {
                        InBIMAssembly = (BIMAssembly)formatter.Deserialize(fs);
                        InBIMAssembly.Insert();
                        TSM.Model model = new TSM.Model();
                        model.CommitChanges();
                    }
                }
            }
        }

        /// <summary>
        /// Вставка балки из xml файла.
        /// </summary>
        /// <param name="fileName"></param>
        public BIMAssembly GetPartXML(string fileName)
        {
            if (Path == string.Empty || Path == null)
            {
                this.GetPath();
            }
            if (File.Exists($"{Path}\\RCP_Data\\{fileName}.xml"))
            {
                var formatter = new XmlSerializer(typeof(BIMAssembly));

                if (this.Path != string.Empty)
                {
                    using (var fs = new FileStream($"{Path}\\RCP_Data\\{fileName}.xml", FileMode.OpenOrCreate))
                    {
                       return (BIMAssembly)formatter.Deserialize(fs);
                    }
                }
            }
            return null;
        }
        

        #region private

        private List<Part> GetAllBeams(Part MainPart, List<Part> SecondaryBeams)
        {
            var beams = new List<Part>();
            if (MainPart != null)
            {
                beams.Add(MainPart);
            }

            if (SecondaryBeams.Count > 0)
            {
                beams.AddRange(SecondaryBeams);
            }
            return beams;
        }
        #endregion // private
    }
}
