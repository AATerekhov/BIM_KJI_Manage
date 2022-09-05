using BIMPropotype_Lib.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSG = Tekla.Structures.Geometry3d;
using TSM = Tekla.Structures.Model;
using UI = Tekla.Structures.Model.UI;

namespace BIMPropotype_Lib.ExtentionAPI.InserPlugin
{
    public static class PrefixDirectoryExtentions
    {
        public static void InsetPlugin(this PrefixDirectory prefixDirectory) 
        {
            TSM.ModelObjectEnumerator modelEnum = new UI.ModelObjectSelector().GetSelectedObjects();
            bool metka = false;
            while (modelEnum.MoveNext())
            {
                if (modelEnum.Current is TSM.Component component)
                {
                    if (component.Name == "BIMPropotypePlugin")
                    {
                        metka = true;
                        component.SetAttribute("selectField", prefixDirectory.FieldName);
                        component.SetAttribute("selectPrototypeName", prefixDirectory.Prefix);
                        component.Modify();
                    }
                }
            }

            if (!metka)
            {
                TSM.ComponentInput componentInput = new TSM.ComponentInput();
                componentInput.AddTwoInputPositions(PickAPoint("Выберите первую точку."), PickAPoint("Выберите вторую точку."));

                var assignmentsPlugin = new TSM.Component(componentInput)
                {
                    Name = "BIMPropotypePlugin",
                    Number = TSM.BaseComponent.PLUGIN_OBJECT_NUMBER
                };

                assignmentsPlugin.SetAttribute("selectField", prefixDirectory.FieldName);
                assignmentsPlugin.SetAttribute("selectPrototypeName", prefixDirectory.Prefix);
                assignmentsPlugin.Insert();
            }                   
        }

        public static TSG.Point PickAPoint(string prompt = "Pick a point")
        {
            TSG.Point myPoint = null;
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
    }
}
