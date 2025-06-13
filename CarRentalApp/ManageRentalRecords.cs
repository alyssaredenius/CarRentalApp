using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarRentalApp
{
    public partial class ManageRentalRecords : Form
    {
        private bool isEditMode;
        private readonly CarRentalEntities _db;

        public ManageRentalRecords()
        {
            InitializeComponent();
            _db = new CarRentalEntities();
        }


        private void ManageRentalRecords_Load(object sender, EventArgs e)
        {
            try
            {
                PopulateGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while loading records: {ex.Message}");

            }
            var records = _db.CarRentalRecords.Select(q => new
            {
                Customer = q.CustomerName,
                DateOut = q.DateRented,
                DateIn = q.DateReturned,
                Id = q.id,
                q.Cost,
                Car = q.TypesOfCar.Make + " " + q.TypesOfCar.Model,
            }).ToList();
            gvRecordList.DataSource = records;
            gvRecordList.Columns["DateIn"].HeaderText = "Date In";
            gvRecordList.Columns["DateOut"].HeaderText = "Date Out";
            gvRecordList.Columns["Id"].Visible = false;
        }

        private void btnAddRecord_Click(object sender, EventArgs e)
        {
            try
            {
                var addEditRecord = new AddEditRentalRecord();
                addEditRecord.MdiParent = this.MdiParent;
                addEditRecord.Show();
                addEditRecord.FormClosed += (s, args) => PopulateGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }


        private void btnEditRecord_Click(object sender, EventArgs e)
        {
            try
            {
                // get Id of selected row
                var id = (int)gvRecordList.SelectedRows[0].Cells["Id"].Value;

                // query database for record
                var record = _db.CarRentalRecords.FirstOrDefault(q => q.id == id);

                // launch AddEditVehicle window with data
                var addEditRecord = new AddEditRentalRecord(record);
                addEditRecord.MdiParent = this.MdiParent;
                addEditRecord.Show();
                addEditRecord.FormClosed += (s, args) => PopulateGrid();
            }
            catch
            {
                MessageBox.Show("Please select a vehicle to edit.");
            }
        }

        private void btnDeleteRecord_Click(object sender, EventArgs e)
        {
            try
            {
                // get Id of selected row
                var id = (int)gvRecordList.SelectedRows[0].Cells["Id"].Value;

                // query database for record
                var record = _db.CarRentalRecords.FirstOrDefault(q => q.id == id);

                DialogResult dr = MessageBox.Show($"Are you sure you want to delete this record?",
                    "Delete", MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Warning);

                if (dr == DialogResult.Yes)
                {
                    _db.CarRentalRecords.Remove(record);
                    _db.SaveChanges();
                }

                _db.SaveChanges();
                PopulateGrid();
            }

            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
            PopulateGrid(); // Refresh grid after deletion
        }

        private void PopulateGrid()
        {
            var records = _db.CarRentalRecords.Select(q => new
            {
                Customer = q.CustomerName,
                DateOut = q.DateRented,
                DateIn = q.DateReturned,
                Id = q.id,
                q.Cost,
                Car = q.TypesOfCar.Make + " " + q.TypesOfCar.Model,
            }).ToList();
            gvRecordList.DataSource = records;
            gvRecordList.Columns["DateIn"].HeaderText = "Date In";
            gvRecordList.Columns["DateOut"].HeaderText = "Date Out";
            gvRecordList.Columns["Id"].Visible = false;

        }
    }
}
