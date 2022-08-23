using System;
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
            var SecondaryBeams = new List<Beam>();
            Beam MainPart = null;

            if (InAssembly.GetMainPart() is Beam beam)
            {
                MainPart = beam;
            }

            var ArreyChildren = InAssembly.GetSecondaries();

            foreach (var child in ArreyChildren) 
            {
                if (child is Beam beamChild)
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
                string path = $"{Path}\\RCP_Data\\Prototype_{InBIMAssembly.Elements[0].InPart.AssemblyNumber.Prefix}.xml";
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

        private List<Beam> GetAllBeams(Beam MainPart, List<Beam> SecondaryBeams)
        {
            var beams = new List<Beam>();
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

        //private Beam TrancformBeam(Beam beam, Matrix matrix, CoordinateSystem mainCS)
        //{
        //    var vbeamCS = beam.GetCoordinateSystem();
        //    vbeamCS.Origin = mainCS.Origin;
        //    var LocalMatrix = MatrixFactory.ByCoordinateSystems(mainCS, vbeamCS);

        //    beam.StartPoint = matrix.Transform(LocalMatrix.Transform(beam.StartPoint));
        //    beam.EndPoint = matrix.Transform(LocalMatrix.Transform(beam.EndPoint));
        //    return beam;
        //}

        /// <summary>
        /// Матрица трансформации CS Part перенесенная в StartPoint
        /// </summary>
        /// <param name="inBeam"></param>
        /// <returns></returns>
        private Matrix GetMatrixBeamPoints(TSM.Beam inBeam)
        {
            var CSPart = inBeam.GetCoordinateSystem();
            Vector X = new Vector(inBeam.EndPoint - inBeam.StartPoint);
            CoordinateSystem coordinateSystem = new CoordinateSystem(inBeam.StartPoint, X, X.Cross(new Vector(0, 0, -1)) );
            //TSM.Model inModel = new TSM.Model();
            //var workHundler = inModel.GetWorkPlaneHandler();

            TransformationPlane transformationPlane = new TransformationPlane(coordinateSystem);
            var matrixtransform = transformationPlane.TransformationMatrixToLocal;

            return matrixtransform;
        }

        private Matrix GetMatrixPointAndPlane()
        {
            Point startPoint = null;
            Vector vectorZ = null;
            Vector vectorX = null;
            var Input = PickAFace("Грань");
            IEnumerator MyEnum = Input.GetEnumerator();
            while (MyEnum.MoveNext())
            {
                InputItem Item = MyEnum.Current as InputItem;
                if (Item.GetInputType() == InputItem.InputTypeEnum.INPUT_1_OBJECT)
                {
                    ModelObject M = Item.GetData() as ModelObject;
                    if (M is Beam beam) 
                    {
                        startPoint = beam.StartPoint;
                        vectorX = new Vector(beam.EndPoint - beam.StartPoint);
                    }
                       
                }
                if (Item.GetInputType() == InputItem.InputTypeEnum.INPUT_POLYGON)
                {
                    ArrayList Points = Item.GetData() as ArrayList;
                    if (Points.Count > 3)
                    {
                        vectorZ = (new Vector((Point)Points[1] - (Point)Points[0]).Cross(new Vector((Point)Points[1] - (Point)Points[2])));
                    }
                }
            }

            CoordinateSystem coordinateSystem = new CoordinateSystem(startPoint, vectorX, vectorX.Cross(-1*vectorZ));

            TransformationPlane transformationPlane = new TransformationPlane(coordinateSystem);
            var matrixtransform = transformationPlane.TransformationMatrixToLocal;

            return matrixtransform;
        }

        private Point PickAPoint(string prompt = "Pick a point")
        {
            Point myPoint = null;
            try
            {
                var picker = new UI.Picker();
                myPoint = picker.PickPoint(prompt);
            }
            catch (Exception ex)
            {
                if (ex.Message != "User interrupt")
                {
                    Console.WriteLine(ex.Message + ex.StackTrace);
                }
            }

            return myPoint;
        }
        public static UI.PickInput PickAFace(string prompt = "Pick a surface")
        {
            UI.PickInput face = null;
            try
            {
                var picker = new UI.Picker();
                face = picker.PickFace(prompt);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + ex.StackTrace);
            }

            return face;
        }
        #endregion // private
    }
}
