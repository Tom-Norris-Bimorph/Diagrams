using GH_IO.Serialization;
using Grasshopper.Kernel;
using System.Collections.Generic;

namespace DiagramsForGrasshopper
{
    public abstract class DiagramComponentWithModifiers : DiagramComponent, IGH_VariableParameterComponent

    {
        //private int m_ItemCount;
        public List<int> Seperators = new List<int>();

        public List<ModifiersBase> Modifiers = new List<ModifiersBase>();

        public DiagramComponentWithModifiers(string a, string b, string c, string d, string e)
           : base(a, b, c, d, e) // pass through the GH_Comp
        {
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            this.RegisterInputStartingParams(pManager);
            Seperators.Add(pManager.ParamCount);
        }

        protected abstract void RegisterInputStartingParams(GH_InputParamManager pManager);

        public override void CreateAttributes()
        {
            base.CreateAttributes();
            m_attributes = new ModifiersAttributes(this);

        }

        public bool CanInsertParameter(GH_ParameterSide side, int index)
        {
            return false;
        }

        public bool CanRemoveParameter(GH_ParameterSide side, int index)
        {
            return false;
        }

        public IGH_Param CreateParameter(GH_ParameterSide side, int index)
        {
            return null;
        }

        public bool DestroyParameter(GH_ParameterSide side, int index)
        {
            return false;
        }

        public void VariableParameterMaintenance()
        {

        }

        /// <summary>
        /// Include the cached data in the archive.
        /// </summary>
        public override bool Write(GH_IWriter writer)
        {
            for (var i = 0; i < Modifiers.Count; i++)
            {
                writer.SetBoolean("Modifier" + i.ToString() + "HasBeenAdded", Modifiers[i].HasBeenAdded);
            }

            return base.Write(writer);
        }
        /// <summary>
        /// Retrieve the cached data from the archive.
        /// </summary>
        public override bool Read(GH_IReader reader)
        {

            for (var i = 0; i < Modifiers.Count; i++)
            {

                if (reader.ItemExists("Modifier" + i.ToString() + "HasBeenAdded"))
                {
                    Modifiers[i].HasBeenAdded = reader.GetBoolean("Modifier" + i.ToString() + "HasBeenAdded");

                }
            }

            return base.Read(reader);
        }

        public void GetAllValues(IGH_DataAccess DA)
        {

            for (var i = 0; i < Modifiers.Count; i++)
            {
                Modifiers[i].GetValues(DA, this);
            }
        }

        public TextModifiers GetFirstOrDefaultTextModifier()
        {

            for (var i = 0; i < Modifiers.Count; i++)
            {
                if (Modifiers[i].GetType() == typeof(TextModifiers))
                {
                    return Modifiers[i] as TextModifiers;
                }
            }
            return new TextModifiers(false, false, false, false, false, false, false, false);
        }

        public CurveModifiers GetFirstOrDefaultCurveModifier()
        {

            for (var i = 0; i < Modifiers.Count; i++)
            {
                if (Modifiers[i].GetType() == typeof(CurveModifiers))
                {
                    return Modifiers[i] as CurveModifiers;
                }
            }
            return new CurveModifiers(false, false, false, false);
        }
    }
}
