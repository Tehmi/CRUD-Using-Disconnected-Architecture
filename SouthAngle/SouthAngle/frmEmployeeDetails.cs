using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SouthAngle
{
    public partial class frmEmployeeDetails : Form
    {
        SqlConnection con;
        SqlDataAdapter da;
        DataSet ds;
        BindingSource bs;

        public frmEmployeeDetails()
        {
            InitializeComponent();
            con = new SqlConnection(@"Data Source=FACULTY03\APTECH;Initial Catalog=SouthAngle;Integrated Security=True");
            da = new SqlDataAdapter();
            SqlCommandBuilder cb = new SqlCommandBuilder(da);
            ds = new DataSet();
            bs = new BindingSource();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void frmEmployeeDetails_Load(object sender, EventArgs e)
        {
            if (con.State == ConnectionState.Open)
                con.Close();
            ds.Clear();
            con.Open();
            da.SelectCommand = new SqlCommand("select * from Employee",con);
            da.Fill(ds,"Employee");
            bs.Clear();
            foreach(DataRow drow in ds.Tables["Employee"].Rows)
            {
                bs.Add(drow);
            }
            DisplayRecord();
            EnableTextControls(false);
            EnableDisableControls("First");
            btnSave.Enabled = false;
            btnCancel.Enabled = false;
            btnAdd.Enabled = true;
            bs.PositionChanged += new EventHandler(bs_PositionChanged);
            lblRecord.Text = "Record: "+ (bs.Position+1) + " of "+ bs.Count;

        }

        private void EnableDisableControls(string cursorPosition)
        {
            if (cursorPosition == "First")
            {
                btnFirst.Enabled = false;
                btnPrevious.Enabled = false;
                btnNext.Enabled = true;
                btnPrevious.Enabled = true;
            }
            else if (cursorPosition == "Last")
            {
                btnFirst.Enabled = true;
                btnPrevious.Enabled = true;
                btnNext.Enabled = false;
                btnLast.Enabled = false;
            }
            else if (cursorPosition == "InBetween")
            {
                btnFirst.Enabled = true;
                btnPrevious.Enabled = true;
                btnNext.Enabled = true;
                btnLast.Enabled = true;
            }
            else if (cursorPosition == "AfterLast")
            {
                btnFirst.Enabled = false;
                btnPrevious.Enabled = false;
                btnNext.Enabled = false;
                btnLast.Enabled = false;
            }
        }

        private void EnableTextControls(bool enable)
        {
            txtName.Enabled = enable;
            txtDesignation.Enabled = enable;
            radioButton1.Enabled = enable;
            radioButton2.Enabled = enable;
            dtDOJ.Enabled = enable;
            txtContact.Enabled = enable;
            
        }

        private void DisplayRecord()
        {
            DataRow drow = (DataRow)bs.Current;
            txtName.Text = drow["Name"].ToString();
             txtDesignation.Text = drow["Designation"].ToString();
             dtDOJ.Text = drow["DOJ"].ToString();
             if (drow["Gender"].ToString().Equals("Male"))
                 radioButton1.Checked = true;
             else
                 radioButton2.Checked = true;
             txtContact.Text = drow["ContactNumber"].ToString();       
        
        }

        private void bs_PositionChanged(object sender, EventArgs e)
        {
            lblRecord.Text = "Record: " + (bs.Position + 1) + " of " + bs.Count; ;
        }

        private void btnFirst_Click(object sender, EventArgs e)
        {
            bs.MoveFirst();
            DisplayRecord();
            EnableDisableControls("First");
        }

        private void btnLast_Click(object sender, EventArgs e)
        {
            bs.MoveLast();
            DisplayRecord();
            EnableDisableControls("Last");
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
              if(bs.Position == bs.Count -1)
              {
                  MessageBox.Show("LAst Record. ","SouthAngle",MessageBoxButtons.OK,MessageBoxIcon.Information);
                  EnableDisableControls("Last");
              }
              else
              {
                  bs.MoveNext();
                  DisplayRecord();
                  EnableDisableControls("InBetween");
              }
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            if (bs.Position == 0)
            {
                MessageBox.Show("First record. ", "SouthAngle",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                EnableDisableControls("First");
            }
            else
            {
                bs.MovePrevious();
                DisplayRecord();
                EnableDisableControls("InBetween");
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            txtName.Text = "";
            txtDesignation.Text = "";
            dtDOJ.Text = "";
            radioButton1.Checked = false;
            radioButton2.Checked = false;
            txtContact.Text = "";
            btnAdd.Enabled = false;
            btnSave.Enabled = true;
            btnCancel.Enabled = true;
            EnableTextControls(true);
            EnableDisableControls("AfterLast");
            lblRecord.Text = "New Record";
            txtName.Focus();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtName.Text == "")
            {
                MessageBox.Show("Enter the name. ","SothAngle",MessageBoxButtons.OK,MessageBoxIcon.Error);
                txtName.Focus();
                return;
            }

            if (txtDesignation.Text == "")
            {
                MessageBox.Show("Enter the Designation. ", "SothAngle", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtDesignation.Focus();
                return;
            }

            if (dtDOJ.Text == "")
            {
                MessageBox.Show("Enter the Date ", "SothAngle", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dtDOJ.Focus();
                return;
            }
           
            if (txtContact.Text== "")
            {
                MessageBox.Show("Enter the Contact ", "SothAngle", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtContact.Focus();
                return;
            }
            DataRow drowemp = ds.Tables["Employee"].NewRow();
            drowemp["Name"] = txtName.Text;
            drowemp["Designation"] = txtDesignation.Text;
            drowemp["DOJ"] = dtDOJ.Text;
            if(radioButton1.Checked)
            drowemp["Gender"] = radioButton1.Text;
            else
                drowemp["Gender"] =radioButton2.Text;
            drowemp["ContactNumber"] = txtContact.Text;
            ds.Tables["Employee"].Rows.Add(drowemp);
            da.Update(ds,"Employee");
            MessageBox.Show("Record has been successfully added.","SouthAngle",MessageBoxButtons.OK,MessageBoxIcon.Information);
            frmEmployeeDetails_Load(null,null);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            frmEmployeeDetails_Load(null,null);

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
