using CarRentalApp;
using CarRentalModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace CarRentalApp
{
    public partial class AddEditRentalRecord : Form
    {
        private bool isEditMode;
        private readonly CarRentalEntities _db;

        public AddEditRentalRecord()
        {
            InitializeComponent();
            lblTitle.Text = "Add New Rental Record";
            this.Text = "Add New Rental Record";
            isEditMode = false;
            _db = new CarRentalEntities();
        }

        public AddEditRentalRecord(CarRentalRecord recordToEdit)
        {
            InitializeComponent();
            _db = new CarRentalEntities();
            lblTitle.Text = "Edit Rental Record";
            this.Text = "Edit Rental Record";

            if (recordToEdit == null)
            {
                MessageBox.Show("Error: No record to edit. Please select a record first.");
                Close();
            }
            else
            {
                isEditMode = true;
                _db = new CarRentalEntities();
                PopulateFields(recordToEdit);
            }
        }

        
        private void PopulateFields(CarRentalRecord recordToEdit)
        {
                tbCustomerName.Text = recordToEdit.CustomerName;
                dtpRented.Value = (DateTime)recordToEdit.DateRented;
                dtpReturned.Value = (DateTime)recordToEdit.DateReturned;
                tbCost.Text = recordToEdit.Cost.ToString();
                lblRecordId.Text = recordToEdit.id.ToString();
            var carType = cmbCarType.Text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string customerName = tbCustomerName.Text;
                var dateOut = dtpRented.Value;
                var dateIn = dtpReturned.Value;
                double cost = Convert.ToDouble(tbCost.Text);

                var carType = cmbCarType.Text;
                var isValid = true;
                var errorMessage = "";

                if (string.IsNullOrWhiteSpace(customerName) || string.IsNullOrWhiteSpace(carType))
                {
                    isValid = false;
                    errorMessage += "Customer Name and Car Type are required.\n\r";
                }
                if (dateOut > dateIn)
                {
                    errorMessage += "Error: Illegal Date Selection\n\r";
                }


                if (isValid)
                {
                    var rentalRecord = new CarRentalRecord();
                    if (isEditMode)
                    {
                        var id = int.Parse(lblRecordId.Text);
                        var record = _db.CarRentalRecords.FirstOrDefault(q => q.id == id);
                    }

                        rentalRecord.CustomerName = customerName;
                        rentalRecord.DateRented = dateOut;
                        rentalRecord.DateReturned = dateIn;
                        rentalRecord.Cost = (decimal)cost;
                        rentalRecord.TypeOfCarID = (int)cmbCarType.SelectedValue;

                    // If not in edit move, then add the record object to the database
                   if (!isEditMode)
                        _db.CarRentalRecords.Add(rentalRecord);
                    _db.SaveChanges();

                    MessageBox.Show($"Customer Name: {customerName}\n\r" +
                    $"Date Rented: {dateOut}\n\r" +
                    $"Date Returned: {dateIn}\n\r" +
                    $"Cost: {cost}\n\r" +
                    $"Car Type: {carType}\n\r" +
                    $"THANK YOU FOR YOUR BUSINESS");
                    Close();       
                }
                else
                {
                    MessageBox.Show(errorMessage);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }


                    private void Form1_Load(object sender, EventArgs e)
                    {
                        // select * from TypesofCards
                        // var cars = carRentalEntities.TypesOfCars.ToList();           

                        var cars = _db.TypesOfCars
                            .Select(q => new {Id = q.Id, Name = q.Make + " " + q.Model})
                            .ToList();
                        cmbCarType.DisplayMember = "Name";
                        cmbCarType.ValueMember = "Id";
                        cmbCarType.DataSource = cars;
                    }
    }
}
