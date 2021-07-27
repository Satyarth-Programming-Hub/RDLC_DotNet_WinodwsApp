using Microsoft.Reporting.WinForms;
using RDLC_DotNet_WinodwsApp.DataSets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RDLC_DotNet_WinodwsApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            this.reportViewer1.RefreshReport();
        }

        private void btnLoadReport_Click(object sender, EventArgs e)
        {
            if (txtCountry.Text == "" || txtCountry.Text == null)
            {
                MessageBox.Show("Parameter is required!!!!");
            }
            else
            {
                this.reportViewer1.ProcessingMode = ProcessingMode.Local;
                var employees = GetReportData();
                var dataSource = new ReportDataSource("Employees", employees.Tables[0]);
                this.reportViewer1.LocalReport.DataSources.Clear();
                this.reportViewer1.LocalReport.DataSources.Add(dataSource);
                this.reportViewer1.RefreshReport();

            }
        }

        private Employees GetReportData()
        {
            string constr = "Data Source = LAPTOP-TG14SPMU; Initial Catalog = SSRSDB; Integrated Security = SSPI;";
            string query = "SELECT * FROM Employee WHERE country=@Country";
            var cmd = new SqlCommand(query);
            cmd.Parameters.AddWithValue("@Country", this.txtCountry.Text);
            using (var con = new SqlConnection(constr))
            {
                using (var da = new SqlDataAdapter())
                {
                    cmd.Connection = con;
                    da.SelectCommand = cmd;
                    using (var employees = new Employees())
                    {
                        da.Fill(employees, "Employee");
                        return employees;
                    }
                }
            }
        }
    }
}