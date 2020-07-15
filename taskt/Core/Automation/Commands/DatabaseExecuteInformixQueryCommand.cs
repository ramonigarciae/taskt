using System;
using System.Xml.Serialization;
using System.Data;
using System.Collections.Generic;
using taskt.UI.Forms;
using System.Windows.Forms;
using taskt.UI.CustomControls;
using IBM.Data.Informix;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Database Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to perform a database query and apply the result to a dataset")]
    [Attributes.ClassAttributes.UsesDescription("Use this command to select data from a database.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'MySql' to achieve automation.")]
    public class DatabaseExecuteInformixQueryCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the server name")]
        [Attributes.PropertyAttributes.InputSpecification("")]
        [Attributes.PropertyAttributes.SampleUsage("")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_Server { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the database name")]
        [Attributes.PropertyAttributes.InputSpecification("")]
        [Attributes.PropertyAttributes.SampleUsage("")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_Database { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the user name")]
        [Attributes.PropertyAttributes.InputSpecification("")]
        [Attributes.PropertyAttributes.SampleUsage("")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_UserID { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the password")]
        [Attributes.PropertyAttributes.InputSpecification("")]
        [Attributes.PropertyAttributes.SampleUsage("")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_Password { get; set; }



        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Define Query to Execute")]
        [Attributes.PropertyAttributes.InputSpecification("")]
        [Attributes.PropertyAttributes.SampleUsage("")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_Query { get; set; }

       
        [XmlIgnore]
        [NonSerialized]
        private DataGridView QueryParametersGridView;

        [XmlIgnore]
        [NonSerialized]
        private List<Control> QueryParametersControls;


        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Apply Result To Variable")]
        [Attributes.PropertyAttributes.InputSpecification("Select or provide a variable from the variable list")]
        [Attributes.PropertyAttributes.SampleUsage("**vSomeVariable**")]
        [Attributes.PropertyAttributes.Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        public string v_DatasetName { get; set; }

        public DatabaseExecuteInformixQueryCommand()
        {
            this.CommandName = "DatabaseExecuteInformixQueryCommand";
            this.SelectionName = "Execute Informix Database Query";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        
        }

        public override void RunCommand(object sender)
        {
            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;
            string query = "select * from alumno";
            string connectionString;
            connectionString = "Database = mi_prueba_base_datos; Host = 127.0.0.1; Server = dr_informix1210_4; Service = 10987;  UID = informix; Password = A3.cY2zd8*CBjTFM;";//Protocol = onsoctcp;



            string ConnectionString = "Host=" + "127.0.0.1" + "; " +
             "Service=" + "10987" + "; " +
             "Server=" + "dr_informix1210_4" + "; " +
             "Database=" + "mi_prueba_base_datos" + "; " +
             "User Id=" + "informix" + "; " +
             "Password=" + "A3.cY2zd8*CBjTFM" + "; ";
            //Can add other DB parameters here like DELIMIDENT, DB_LOCALE etc
            //Full list in Client SDK's .Net Provider Reference Guide p 3:13
            IfxConnection conn = new IfxConnection();
            conn.ConnectionString = ConnectionString;
            try
            {
                conn.Open();
                Console.WriteLine("Made connection!");
                Console.ReadLine();
            }
            catch (IfxException ex)
            {
                Console.WriteLine("Problem with connection attempt: "
                                  + ex.Message);
            }

            IfxConnection connection = null;

            
            try
            {
                connection.Open();

            }
            catch (IfxException ex)
            {



            }


            //Create Command
            IfxCommand cmd = new IfxCommand(query, connection);
            //Create a data reader and Execute the command

            DataTable dataTable = new DataTable();
            IfxDataAdapter adapter = new IfxDataAdapter(cmd);

            adapter.Fill(dataTable);

            dataTable.TableName = v_DatasetName;
            engine.DataTables.Add(dataTable);

            engine.AddVariable(v_DatasetName, dataTable);

            try
            {
                connection.Close();
            }
            catch (IfxException ex)
            {

            }

            }
        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_Server", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_Database", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_UserID", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_Password", this, editor));

            var queryControls = CommandControls.CreateDefaultInputGroupFor("v_Query", this, editor);
            var queryBox = (TextBox)queryControls[2];
            queryBox.Multiline = true;
            queryBox.Height = 150;
            RenderedControls.AddRange(queryControls);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_DatasetName", this, editor));
            return RenderedControls;

        }

       

        public override string GetDisplayValue()
        {
            return $"{base.GetDisplayValue()} - [ Apply Result to Variable '{v_DatasetName}', Server Name: '{v_Server}', Database Name: '{v_Database}']";
        }
    }
}